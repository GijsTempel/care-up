/*
	Custom Markup Language (CML) V2.0 Copyright @ myBad Studios 2012
	http://www.mybadstudios.com

	CML was created to provide a simple format for storing, retrieving and navigating
	sets of data. Data sets can be classified by type and datablocks of
	such sets can be placed in files in any order. You are not limited to
	a single dataset in a single file and are free to create as many as you need.
	
	CML allows for two different methods of adding data to a data block.
	
	1. The shorthand form:
	<dataset_name>[field_name[=value][;]...]

	Example:
	<book>name=Romeo and Juliet;author=Shakespear;price=5.99;owner=;A note or two
	
	2. Normal form:
	<dataset_name>
	[field_name][value]
	[...]
	
	Example:
	<book>
	[name]Romeo and Juliet
	[author]	Shakespear
	[price] 5.99
	[owner]
	A note or two
*/

/*
	UPDATE: CML v3.0
	----------------
	CML has now become a generic class. This means it's use is slightly more 
	complex but it's usefullness has expanded exponentially because of it.

	CML has now been split into 2 versions:
	1. CMLBase contains the bulk of the functionality but contains no Unity specific
	code and can thus be used with .NET applications outside of Unity

	2. UnityCML is the new name for the old CML class and reintroduces the unity functionality
	back into CMLBase. If you want to extend CML then derive from this class. If you just want to
	use CML like you have been doing up till now then you can still use the original CML type.

	The fundamental change between the old CML and this new version is that you can now
	create your own classes derived from CMLData and use your own, custom classes as
	the data source which CML tracks for you.

	For example, the normal functionality of CMLData has been moved into a class called
	CMLDataBase and continas no Unity specific code, thus making it compatible with
	.NET applications outside of Unity.
	CMLData is derived from CMLDataBase and adds back the familiar functions to convert
	strings into common Unity data types.
	You can now create your own classes derived from either of these and assign them
	to CML as easily as:
		UnityCML<CMLData> my_var;
		MyCML<CMLData> my_var;
		MyCML<myData> my_var
		CML my_var;
*/

/*
	UPDATE: CML v4.0
	----------------
    Upgraded the code to C#6 syntax.
    Added support for Long and Color data types
    Changed cmlData and cmlDataBase classes to CMLData and CMLDataBase respectively
    Added support to do addition and subtraction in the CMLData class
    Added "public object obj" member to the CMLData class
    Marked the following as obsolete: Vector, Vect2, Rect4, Quat
    Added the following replacements: Vector3, Vector2, Rect, Quaternion respectively
*/
using UnityEngine;
#if !UNITY_WSA
using System.IO;
#endif

namespace MBS
{
	public class CML : UnityCML<CMLData> { }
	public class UnityCML<T> : CMLBase<T> where T : CMLDataBase, new()
    {
		public UnityCML()
		{
			base.Initialize();
		}

		public UnityCML(string filename)
        {
            if (string.Empty != filename)
            {
                if (!LoadFile(filename))
                    Initialize();
            }
            else
            {
                base.Initialize();
            }
        }
			
        //load CML from PlayerPrefs
        virtual public bool Load(string PrefName)
        {
            string cml = PlayerPrefs.GetString(PrefName, string.Empty);
            if (cml == string.Empty)
                return false;

            Parse(cml);
            return true;
        }

        //load CML from a Resources folder
        virtual public bool LoadFile(string filename)
        {
            TextAsset FileResource = (TextAsset)Resources.Load(filename, typeof(TextAsset));
            if (null != FileResource)
                return Parse(FileResource.text);

            return false;
        }

        //save this CML data to PlayerPrefs
        virtual public bool Save(string PrefName, bool include_id = true)
        {

            PlayerPrefs.SetString(PrefName, ToString(include_id));
            return true;
        }
			
    }
}
