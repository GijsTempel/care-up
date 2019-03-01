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
    public bool buildActionList = false;
    public List<GameObject> hidenObjects;

    public ObjectsIDs[] Objects =
    {
        new ObjectsIDs("syringePack", 30, 0),
        new ObjectsIDs("Syringe", 2, 1),
        new ObjectsIDs("AbsorptionNeedle", 20, 0),
        new ObjectsIDs("InjectionNeedle", 20, 0),
        new ObjectsIDs("cloth_02_folded", 33, 0),
        new ObjectsIDs("cloth_02", 53,1),
        new ObjectsIDs("water_syringe_pack", 35, 0),
        new ObjectsIDs("water_syringe_pack_no_cover", 35, 1),
        new ObjectsIDs("water_syringe_pack_empty", 35, 2),
        new ObjectsIDs("water_syringe_pack_cover", 36, 0),

        new ObjectsIDs("lube_syringe_pack", 48, 0),
        new ObjectsIDs("lube_syringe_pack_noCover", 48, 1),
        new ObjectsIDs("lube_syringe_pack_empty", 48, 2),
        new ObjectsIDs("lube_syringe_pack_cover", 48, 0),
        new ObjectsIDs("w0_A", -5, 0),
        new ObjectsIDs("m1_A", -6, 0),
        new ObjectsIDs("Sink", -10, 0),
        new ObjectsIDs("GauzeTrayFull", 38,0),
		new ObjectsIDs("cotton_ball", 40,0),
		new ObjectsIDs("catheter_bag_packed", 42,0),
		new ObjectsIDs("catheter_bag_packed_opened", 42,1),
		new ObjectsIDs("catheter_bag_packed_empty", 42,2),
		new ObjectsIDs("catheter", 43,0),
        new ObjectsIDs("catheter_inner", 44,0),
        new ObjectsIDs("x_test", 101,0),
        new ObjectsIDs("CWB_inner_open", 50,0),
        new ObjectsIDs("CWB_inner_open_in2", 50,1),
        new ObjectsIDs("catheter_bag_twisted", 46,0),
        new ObjectsIDs("fixator_folded", 52,0),
        new ObjectsIDs("fixation_folded_buttons", 53,0),
        new ObjectsIDs("Sink_active2", -15,0),
        new ObjectsIDs("PaperTowelInHand", 60,5),
		new ObjectsIDs("MedicineBag", 102,00),
        new ObjectsIDs("BandAidPackage", 105,00),

        //


    };

    public GameObject getFromHidden(string _name)
    {
        if (_name != "" && hidenObjects.Count > 0)
        {
            foreach (GameObject go in hidenObjects)
            {
                if (go != null)
                {
                    if (_name == go.name)
                        return go;
                }
            }
        }
        return null;
    }



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



    public int GetIDByName(string name)
    {
        int byName = FindByName(name);
        if (byName != -1)
        {
            ObjectsIDs o = GetObject(byName);
            return o.GetID() * 100 + o.GetState();
        }

        return (0);
    }


    // Use this for initialization
    void Start () {

	}

}
