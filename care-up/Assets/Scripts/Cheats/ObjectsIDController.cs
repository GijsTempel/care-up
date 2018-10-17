using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsIDs
{
	public int ID;
	public int State;
	public string Name;

	public ObjectsIDs(string name, int id, int state)
	{
		Name = name;
		ID = id;
		State = state;
	}

	public string GetName()
    {
		return (Name);
    }

	public int GetID()
    {
        return (ID);
    }

	public int GetState()
    {
        return (State);
    }

}

public class ObjectsIDController : MonoBehaviour
{
	public bool Cheat = false;
	public ObjectsIDs[] Objects =
	{
		new ObjectsIDs("syringePack", 30, 0),
		new ObjectsIDs("Syringe", 2, 1),
		new ObjectsIDs("AbsorptionNeedle", 20, 0),
		new ObjectsIDs("InjectionNeedle", 20, 0),
        new ObjectsIDs("cloth_02_folded", 33, 0),
        new ObjectsIDs("water_syringe_pack", 35, 0),
        new ObjectsIDs("water_syringe_pack_no_cover", 35, 1),
        new ObjectsIDs("water_syringe_pack_empty", 35, 2),
        new ObjectsIDs("water_syringe_pack_cover", 36, 0),

		new ObjectsIDs("lube_syringe_pack", 35, 0),
        new ObjectsIDs("lube_syringe_pack_noCover", 35, 1),
        new ObjectsIDs("lube_syringe_pack_empty", 35, 2),
        new ObjectsIDs("lube_syringe_pack_cover", 36, 0),
		new ObjectsIDs("cloth_02_folded", 33, 0),


    };


	public int FindByName(string name)
	{
		int n = 0;
		foreach (ObjectsIDs Obj in Objects)
        {
			if (Obj.Name == name)
			{
				return (n);
			}
			n++;
        }
		return (-1);
	}

	public ObjectsIDs GetObject(int n)
	{
		if (n < 0 || n > Objects.Length)
		{
			return (null);
		}
		return Objects[n];

	}

	public int GetID(int n)
	{
		if (n < 0 || n > Objects.Length)
		{
			return (-1);
		}
		return (Objects[n].ID);
	}


	public int GetState(int n)
    {
		if (n < 0 || n > Objects.Length)
        {
            return (-1);
        }
        return (Objects[n].State);
    }
    

	public string GetName(int n)
    {
        if (n < 0 && n > Objects.Length)
        {
            return ("");
        }
        return (Objects[n].Name);
    }
    

	// Use this for initialization
	void Start () {
		
	}

}

   