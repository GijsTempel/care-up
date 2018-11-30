using System;
using System.Collections;
using System.Collections.Generic;
#if !UNITY_WSA
using System.IO;
#endif

namespace MBS {

	public class CMLBase<T> where T : CMLDataBase, new()  {
		const float __CMLVersion = 4.0f;

		public List<T> Elements;
		public T Node;

		static public float CMLVersion => __CMLVersion;

		public T this[int index] => Count > index ? Elements[index] : null;

        virtual public T Last => (Elements.Count == 0) ? null : Elements[Elements.Count - 1]; 
		virtual public T First => (Elements.Count == 0) ? null : Elements[0]; 
		virtual public int Count => null == Elements ? 0 : Elements.Count; 

		public CMLBase()
		{
			Initialize();
		}

		//clears the CML data
		virtual public void Initialize()
		{
			Node = null;
			if (null != Elements)
				Elements.Clear();
			Elements = new List<T>();
		}

		//find a node by it's ID
		virtual public int FindNodeByID(int id, int offset = 0)
		{
			for (int i = offset; i < Count; i++)
				if (Elements[i].ID == id)
				{
					//if the node was found, point to it
					return i;
				}
			return -1;
		}

		//create a new CMLDataBase node
		virtual public bool AddNode(string data_type, string add_data = "")
		{
			if (data_type == string.Empty)
				return false;

			T new_one = new T();
			new_one.ID = Elements.Count;
			Elements.Add(new_one);

			Last.data_type = data_type;
			Last.ProcessCombinedFields(add_data);
			return true;
		}

		//CML files are supposed to have sequential nodes but since the
		//data author can override this, do a test to see if the specified
		//node is the one requested or else go look for it.
		//Since a single file can have multiple sections and each section
		//could start it's index from 1, you can also provide the section's
		//node index as an offset from which to start searching
		public bool PositionAtNode(int index, int offset = 0)
		{
			if (index + offset < 0 || index + offset >= Count)
				return false;

			if (Elements[index + offset].ID == index)
			{
				Node = Elements[index + offset];
				return true;
			}

			int found_index = FindNodeByID(index, offset);
			if (found_index < 0)
				return false;
			Node = Elements[found_index];
			return true;
		}

		//return the first node of type data_type
		virtual public T GetFirstNodeOfType(string data_type)
		{
			if (Count == 0)
				return null;

			for (int i = 0; i < Count; i++)
				if (Elements[i].data_type == data_type)
					return Elements[i];
			return null;
		}

		//return the ID of the first node of type data_type
		virtual public int GetFirstNodeOfTypei(string data_type)
		{
			if (Count == 0)
				return -1;

			for (int i = 0; i < Count; i++)
				if (Elements[i].data_type == data_type)
					return i;
			return -1;
		}

		public bool PositionAtFirstNodeOfType(string data_type)
		{
			T result = GetFirstNodeOfType(data_type);
			if (null != result)
				PositionAtNode(result.ID);

			return (null != result);
		}

		//remove the node at index 
		virtual public bool RemoveNode(int index)
		{
			if (Count <= index)
				return false;

			//position Node on the next node
			if (null != Node)
			if (Node.ID == index)
			if (index < Count - 1)
				Node = Elements[index + 1];
			else
				//if there isn't a "next" node, first check if there is a "previous" node
				//and go there if there is...
				if (Node.ID == 0)
					Node = null;
				else
					Node = Elements[index - 1];

			Elements.RemoveAt(index);
			for (int i = index; i < Elements.Count; i++)
				Elements[i].ID = i;

			return true;
		}

		//add a node at the current marker position
		virtual public bool InsertNode(string data_type, string add_data = "")
		{
			if (null != Node)
				return InsertNode(Node.ID, data_type, add_data);
			else
				return AddNode(data_type, add_data);
		}

		//add a node a specific index
		virtual public bool InsertNode(int index, string data_type, string add_data = "")
		{
			if (data_type == string.Empty)
				return false;

			if (Count == 0)
				return AddNode(data_type, add_data);

			T new_one = new T();
			new_one.ID = index;
			Elements.Insert(index, new_one);

			Node = Elements[index];
			Node.data_type = data_type;
			Node.ProcessCombinedFields(add_data);
			for (int i = index + 1; i < Count; i++)
				Elements[i].ID = i;

			return true;
		}

		#if !UNITY_WSA
		//load CML from a specified file path
		virtual public bool OpenFile(string FileName)
		{
			StreamReader f = new StreamReader(FileName);
			if (null == f)
				return false;

			string file = string.Empty;
			string line = string.Empty;
			while ((line = f.ReadLine()) != null)
				file += line + "\n";
			return Parse(file);
		}

		//save this CML data to a specified file path. When saving, prefer to include CML version info
		//if it is not included already, add to the bottom of the file to avoid breaking data that was
		//setup to use specific node IDs. Optionally, simply don't include this info.
		virtual public bool SaveFile(string FileName, bool include_id = true)
		{
			StreamWriter f = new StreamWriter(File.Open(FileName, FileMode.Create));
			if (null == f)
				return false;

			foreach (T node in Elements)
				f.WriteLine(node.ToString(include_id) + "\n");
			f.Close();
			return true;
		}
		#endif


		//return this entire CML data as a formatted string that can either be printed or saved
		virtual public string ToString(bool include_id = true)
		{
			string pref = string.Empty;
			foreach (T node in Elements)
				pref += node.ToString(include_id) + "\n";

			return pref;
		}

		virtual public bool Parse(string data)
		{
			if (data == string.Empty)
				return false;

			Initialize();
			string[] lines = data.Split('\n');
			foreach (string line in lines)
			{
				//find and remove comments from the parser
				//take into account URL strings '://'
				string l = line;
				int commentIndex = l.IndexOf("//");
				if (commentIndex > -1)
				{
					int urlIndex = 0;
					if (commentIndex > 0)
					{
						while (l.IndexOf("://", urlIndex) == commentIndex - 1)
						{
							urlIndex = commentIndex + 1;
							commentIndex = l.IndexOf("//", urlIndex);
						}
					}

					if (commentIndex > -1)
						l = l.Substring(0, commentIndex);
				}
				l = l.Trim();

				//if the line is empty, do nothing with it...
				//ignore lines that close off data blocks...
				if (l == string.Empty //|| l.IndexOf("</") == 0
				) continue;

				//be default add data to the last datatype created
				int tagopen = l.IndexOf('[');
				int tagclose = l.IndexOf(']');
				if (tagclose > 1 && tagopen == 0)
				{
					if (null != Last)
					{
						string keyname = l.Substring(1, tagclose - 1).Trim();
						l = l.Substring(tagclose + 1, l.Length - (tagclose + 1)).Trim();
						if (null != Last)
						{
							Last.Set(keyname, l);
						}
					}
				}
				else
				{
					//check to see if this is the definition of a new datatype's constant fields
					//or wether this is new data for a particular datatype
					tagopen = l.IndexOf('<');
					tagclose = l.IndexOf('>');
					if (tagclose > 1 && tagopen == 0)
					{
						string key_name = l.Substring(1, tagclose - 1).Trim();
						string key_value = l.Substring(tagclose + 1, l.Length - (tagclose + 1));
						AddNode(key_name, key_value);
					}
					else
					{
						//else add it as raw data...
						if (null != Last)
							Last.AddToData(l);
					}
				}
			}

			Node = (Count > 0) ? First : null;
			return true;
		}

		//remove all CMLDataBase from this CML data
		virtual public void Clear()
		{
			foreach (T d in Elements)
				d.Clear();
			Elements.Clear();
		}

		//append a CMLDataBase variable to the CML data
		virtual public bool CopyNode(T existing)
		{
			T copy = existing.Copy() as T;

			T new_one = new T();
			new_one.ID = Elements.Count;
			Elements.Add(new_one);

			Last.data_type = copy.data_type;
			foreach (var key in copy.defined)
				Last.defined.Add(key.Key, key.Value);
			foreach (string s in copy.data)
				Last.data.Add(s);
			return true;
		}


		#region marker_navigation
		//move the marker to various positions in the CML data
		virtual public bool PositionAtFirstNode()
		{
			if (Count > 0)
				Node = Elements[0];
			else
				return false;
			return true;
		}

		virtual public bool PositionAtLastNode()
		{
			if (Count > 0)
				Node = Elements[Count - 1];
			else
				return false;
			return true;
		}

		virtual public bool PositionAtNextNode()
		{
			if (null == Node)
				return false;

			int index = Node.ID + 1;
			return PositionAtNode(index);
		}

		virtual public bool PositionAtPreviousNode()
		{
			if (null == Node)
				return false;

			int index = Node.ID - 1;
			return PositionAtNode(index);
		}

		//position the marker at a node with a specific ID, even if it is
		//not at the correct array index. Use when you have multiple CML
		//files loaded into the same CML variable or if you manually change
		//the ID values of nodes. NOTE: never do that !
		virtual public bool PositionAtID(int id)
		{
			if (id < 0)
				return false;

			int index = FindNodeByID(id);
			if (index == -1)
				return false;

			Node = Elements[index];
			return true;
		}
		#endregion

		//remove the node at the current marker position
		virtual public bool RemoveCurrentNode()
		{
			return RemoveNode(Node.ID);
		}

		//test if this CML variable contains at least 1 node of type data_type
		virtual public bool ContainsANodeOfType(string data_type)
		{
			return GetFirstNodeOfTypei(data_type) >= 0;
		}

		//return the last node of type data_type
		virtual public T GetLastNodeOfType(string data_type)
		{
			int location = GetLastNodeOfTypei(data_type);
			if (location >= 0) 
				return Elements[location];
			return null;
		}
        
		//fetch all nodes under a specific node.
		//collection of nodes will end when it reaches the end of file or
		//it finds another node of the same data_type as the parent node
		//does not fetch child nodes recursively
		virtual public List<T> Children(int index = -1)
		{
			List<int> ignore = null;
			List<T> result = null;
			ChildLooper (index, false, out ignore, out result );
			return result;
		}

		//recursively fetch all nodes under a specific node.
		//collection of nodes will end when it reaches the end of file or
		//it finds another node of the same data_type as the parent node
		virtual public List<T> AllChildNodes(int index = -1)
		{
			List<int> ignore = null;
			List<T> result = null;
			ChildLooper (index, false, out ignore, out result, true);
			return result;
		}

		//fetch the node IDs of all nodes under a specific node.
		//collection of nodes will end when it reaches the end of file or
		//it finds another node of the same data_type as the parent node
		//does not fetch child nodes recursively
		virtual public List<int> Childreni(int index = -1)
		{

			List<int> result = null;
			List<T> ignore = null;
			ChildLooper (index, true, out result, out ignore );
			return result;
		}

		//recursively fetch the node IDs of all nodes under a specific node.
		//collection of nodes will end when it reaches the end of file or
		//it finds another node of the same data_type as the parent node
		virtual public List<int> AllChildNodesi(int index = -1)
		{
			List<int> result = null;
			List<T> ignore = null;
			ChildLooper (index, true, out result, out ignore, true);
			return result;
		}

		//return the ID of the last node of type data_type
		virtual public int GetLastNodeOfTypei(string data_type)
		{
			if (Count == 0)
				return -1;
			
			for (int i = Count - 1; i >= 0; i--)
				if (Elements[i].data_type == data_type)
					return i;
			return -1;
		}
		
		bool ClosingNode(string parent, string child)
		{
			child = child.Trim ();
			parent = parent.Trim ();
			return child == parent || child == string.Format ("/{0}", parent);
		}
		
		//find the children of the parent node and return either a List<int> or List<CMLData> depending on the requirement
		void ChildLooper(int index, bool get_int, out List<int> ints, out List<T> datas, bool all = false)
		{
			List<int> li = new List<int> ();
			List<T> lc = new List<T>();
			
			//if a node is not specified, try to use the currently active Node
			if (index == -1)
			{
				if (null == Node)
				{
					goto quit;
				}
				else
				{
					index = Node.ID;
				}
			}
			
			//if an invalid node is selected, return nothing
			//also return nothing if the very last node was selected
			if (Count <= index + 1) 
				goto quit;
			
			//see what the data type is of the first child node...		
			string 
				parent_data_type = Elements[index].data_type,
				data_type = Elements[index + 1].data_type;
			
			//if the child node is of the same type as the parent node
			//then consider the parent childless		
			if (ClosingNode(parent_data_type, data_type))
				goto quit;
			
			//loop through all remaining databclocks and return each that matches
			//the data type of the first child node, stopping at the first datablock
			//that matches the original data type
			for (++index; index < Count; index++)
			{
				if (ClosingNode(parent_data_type, Elements[index].data_type)) break; 
				if (!all && Elements[index].data_type != data_type) continue;
				
				if (get_int)
					li.Add (index);
				else
					lc.Add (Elements[index]);				
			}
			if (get_int) {
				ints = li;
				datas = null;
			} else {
				datas = lc;
				ints = null;
			}
			return;
			
		quit:
				ints = null;
			datas = null;
		}
		
		//find all nodes that contain the specified field. If a value is provided,
		//return all nodes to contain the field with the selected value
		virtual public List<T> NodesWithField(string field, string val = "")
		{
			List<T> Result = new List<T>();
			for (int index = 0; index < Count; index++)
			{
				foreach (var k in Elements[index].defined)
					if (k.Key == field)
				{
					if (val == string.Empty)
						Result.Add(Elements[index]);
					else
						if (k.Value == val)
							Result.Add(Elements[index]);
					break;
				}
			}
			if (Result.Count > 0)
				return Result;
			
			return null;
		}
		
		//find the first node that contains the specified field. If a value
		//is provided, return the first node to contain the field with the selected value
		virtual public T NodeWithField(string field, string val = "")
		{
			List<T> results = NodesWithField(field, val);
			if (null != results)
				return results[0];
			return null;
		}
		
		//Return all nodes of type type_name, starting from the node with ID starting_index and stopping
		//as soon as it finds a node of type stop_at_data_node if specified.
		virtual public List<T> AllNodesOfType(string type_name, int starting_index = 0, string stop_at_data_type = "")
		{
			List<T> result = new List<T>();
			if (starting_index >= Count)
				return result;
			
			for (int i = starting_index; i < Count; i++)
			{
				if (stop_at_data_type != string.Empty && Elements[i].data_type == stop_at_data_type)
					break;
				
				if (Elements[i].data_type == type_name)
					result.Add(Elements[i]);
			}
			return result;
		}
		
		//Return the node ID's of all nodes of type type_name, starting from the node with ID starting_index and stopping
		//as soon as it finds a node of type stop_at_data_node if specified.
		virtual public List<int> AllNodesOfTypei(string type_name, int starting_index = 0, string stop_at_data_type = "")
		{
			List<int> result = new List<int>();
			if (starting_index >= Count)
				return result;
			
			for (int i = starting_index; i < Count; i++)
			{
				if (stop_at_data_type != string.Empty && Elements[i].data_type == stop_at_data_type)
					break;
				
				if (Elements[i].data_type == type_name)
					result.Add(i);
			}
			
			return result;
		}

		//impliment the ability to use "foreach" on CML data types
		public IEnumerator GetEnumerator() => new CMLIb<T>(Elements);
		
		//append another CML data file to the end of this one
		virtual public bool Join(CMLBase<T> other, CMLCopyMode copy_mode = CMLCopyMode.no_id)
		{
			if (null == other || other.Count == 0)
				return false;
			
			foreach (T d in other)
			{
				T new_one = d.Copy(copy_mode, Elements.Count.ToString()) as T;
				Elements.Add(new_one);
			}
			
			return true;
		}
	}

	//this is created merely to allow for the us of "foreach" when navigating through CMl data
	class CMLIb<T> : IEnumerator where T : CMLDataBase
	{
		List<T> Elements;
		int i = -1;
		
		public CMLIb(List<T> data)
		{
			Elements = data;
		}
		
		public bool MoveNext() => (++i < Elements.Count) ?  true : false;
        public void Reset() => i = -1;
		public object Current => Elements[i]; 		
	}

}