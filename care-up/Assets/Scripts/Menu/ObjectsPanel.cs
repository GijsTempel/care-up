using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPanel : MonoBehaviour {
    public GameObject content;
    // Use this for initialization

    void PanelClear()
    {
        foreach (Transform b in content.transform)
        {
            GameObject.Destroy(b.gameObject);
        }
    }
    void AddObjectButton(GameObject obj)
    {
        GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/ObjectButton"), content.transform);
        button.GetComponent<ObjectPanelButton>().setObject(obj);
    }

    public void UpdatePanel()
    {
        PanelClear();
        PlayerScript ps = GameObject.FindObjectOfType<PlayerScript>();
        HandsInventory inventory = GameObject.FindObjectOfType<HandsInventory>();
        float maxDist = (GameObject.FindObjectOfType<Controls>().interactionDistance + 0.2f) * 6f;
        foreach (PickableObject p in GameObject.FindObjectsOfType<PickableObject>())
        {
            //if (p.name == "ipad")
            //    continue;
            if (p == inventory.leftHandObject || p == inventory.rightHandObject)
                continue;
            float dist = Vector3.Distance(p.transform.position, ps.transform.position);
            print(dist);
            if (dist < maxDist)
            {
                AddObjectButton(p.gameObject);
            }
        }
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
