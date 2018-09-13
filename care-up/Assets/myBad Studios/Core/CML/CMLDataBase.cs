using System.Collections.Generic;
#if !UNITY_WSA
using System.IO;
#endif

namespace MBS {

    public enum CMLCopyMode { new_id, old_id, no_id }
	public class CMLDataBase 
	{
		public string data_type;
		public Dictionary<string, string> defined;
		public List<string> data;
        public object obj;

		static public bool operator true(CMLDataBase v) { return null != v; }
		static public bool operator false(CMLDataBase v) { return null == v; }

		public string this[string field_name]
		{
			get { return String(field_name); }
			set { Set(field_name, value); }
		}

		public int ID
		{
			get { if (defined.ContainsKey("id")) return int.Parse(defined["id"]); return 0; }
			set { Set("id", value.ToString()); }
		}

		public CMLDataBase()
		{
			data = new List<string>();
			defined = new Dictionary<string, string>();
			ID = 0;
		}

		public CMLDataBase(int id)
		{
			data = new List<string>();
			defined = new Dictionary<string, string>();
			ID = id;
		}

		public void Clear()
		{
			defined.Clear();
			data.Clear();
		}

		virtual public bool Remove(string name = "value")
		{
			bool result = defined.ContainsKey(name);
			if (result)
				defined.Remove(name);

			return result;
		}

		virtual public string ToString(bool include_id = true)
		{
			string result = "<" + data_type + ">";
			foreach (var d in defined)
				if (d.Key != "id" || include_id)
					result += "\n\t[" + d.Key + "]" + d.Value;

			foreach (string s in data)
				if (s != string.Empty)
					result += "\n\t" + s;

			return result;
		}

		virtual public string[] Keys
		{
			get
			{
				if (defined.Count == 0)
					return null;
				string[] results = new string[defined.Count];
				defined.Keys.CopyTo(results, 0);
				return results;
			}
		}

		virtual public string[] Values
		{
			get
			{
				if (defined.Count == 0)
					return null;
				string[] results = new string[defined.Count];
				defined.Values.CopyTo(results, 0);
				return results;
			}
		}

        virtual public void Add( int qty, string name="value" ) => Seti( name, Int( name ) + qty );
        virtual public void Addf( float qty, string name = "value" ) => Setf( name, Float( name ) + qty );
        virtual public void Addl( long qty, string name = "value" ) => Setl( name, Long( name ) + qty );
        virtual public void Subtract( int qty, string name = "value" ) => Seti( name, Int( name ) - qty );
        virtual public void Subtractf( float qty, string name = "value" ) => Setf( name, Float( name ) - qty );
        virtual public void Subtractl( long qty, string name = "value" ) => Setl( name, Long( name ) - qty );

        virtual public void Set(string name, string data)
		{
			if (defined.ContainsKey(name))
				defined[name] = data;
			else
				defined.Add(name, data);
		}
        virtual public void Seti( string name, int data ) => Set( name, data.ToString() );
        virtual public void Setl( string name, long data ) => Set( name, data.ToString() );
        virtual public void Setf(string name, float data) => Set(name, data.ToString());

        virtual public int Int( string named = "value" )
        {
            string result = string.Empty;
            if ( defined.TryGetValue( named, out result ) )
            {
                int value;
                if ( int.TryParse( result, out value ) )
                    return value;
                else
                    return 0;
            }

            return 0;
        }

        virtual public long Long( string named = "value" )
        {
            string result = string.Empty;
            if ( defined.TryGetValue( named, out result ) )
            {
                long value;
                if ( long.TryParse( result, out value ) )
                    return value;
                else
                    return 0;
            }
            return 0;
        }

        virtual public float Float(string named = "value")
		{
			string result = string.Empty;
			if (defined.TryGetValue(named, out result))
			{
				float value;
				if (float.TryParse(result, out value))
					return value;
				else
					return 0;
			}

			return 0;
		}

		virtual public string String(string named = "value")
		{
			string result = string.Empty;
			if (defined.TryGetValue(named, out result))
				return result;

			return string.Empty;
		}

		virtual public bool Bool(string named = "value")
		{
			string result = string.Empty;
			int i = 0;
			if (defined.TryGetValue(named, out result))
			{
				result = result.Trim();
				if (result.ToLower() == "true")
					return true;

				if (int.TryParse(result, out i))
					return (i > 0);
			}
			return false;
		}
			
		//add data to the unordered list
		virtual public void AddToData(string value) => data.Add(value);
		
		//add multiple fields to this CMLData at once
		virtual public void ProcessCombinedFields(string combined)
		{
			if (combined == string.Empty)
				return;

			string[] fields = combined.Split(';');
			foreach (string field in fields)
			{
				if (field.IndexOf('=') == -1)
				{
					AddToData(field.Trim());
				}
				else
				{
					string[] keyVal = field.Split('=');
					Set(keyVal[0].Trim(), keyVal[1].Trim());
				}
			}
		}

		//returns a duplicate of this CMLDataBase object
		virtual public CMLDataBase Copy(CMLCopyMode mode = CMLCopyMode.no_id, string id_value = "-1")
		{
			CMLDataBase result = new CMLDataBase();
			CopyTo (result, mode, id_value);
			return result;
		}

		//copies the data from this object into the data sections of another CMLDataBase object
		virtual public void CopyTo(object t, CMLCopyMode mode = CMLCopyMode.no_id, string id_value = "-1")
		{
			CMLDataBase target = (CMLDataBase)t;
			target.Clear ();
			target.data.Clear ();

			target.data_type = this.data_type;
			foreach (var data in this.defined)
			{
				if (data.Key != "id")
				{
					target.Set(data.Key, data.Value);
				}
				else
				{
					switch (mode)
					{
						//keep the original id....
					case CMLCopyMode.old_id:
						target.Set("id", data.Value);
						break;
						
					case CMLCopyMode.new_id:
						target.Set("id", id_value);
						break;
						
					case CMLCopyMode.no_id:
						target.Remove("id");
						break;
					}
				}
			}

			foreach(string s in this.data)
				target.AddToData(s);
			t = target;
		}

	}

}