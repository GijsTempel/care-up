using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrashContainer : MonoBehaviour
{
    Transform actionTriggerHolder;
    private Dictionary<string, ActionModule_ActionTrigger> objectsAllowed = new Dictionary<string, ActionModule_ActionTrigger>();

    public List<string> testList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        actionTriggerHolder = transform.Find("ActionTriggers");
        UpdateObjectsAllowed();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision == null)
            return;
        if (collision.GetComponent<PickableObject>() == null)
            return;
        if (collision.GetComponent<PickableObject>().IsMounted())
            return;
        if (objectsAllowed.Keys.Contains(collision.name))
        { 
            if (objectsAllowed[collision.name].AttemptTrigger())
                DestroyObject(collision.gameObject);
        }
    }


    void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    void UpdateObjectsAllowed()
    {
        objectsAllowed.Clear();
        testList.Clear();
        for (int i = 0; i < actionTriggerHolder.childCount; i++)
        {
            ActionModule_ActionTrigger a = actionTriggerHolder.GetChild(i).GetComponent<ActionModule_ActionTrigger>();
            if (a != null)
            {
                foreach (string n in new string[] {a.LeftActionManagerObject, a.RightActionManagerObject})
                {
                    if (n != "" && !(objectsAllowed.Keys.Contains(n)))
                    {
                        objectsAllowed.Add(n, a);
                        testList.Add(n);
                    }
                }
            }
        }
    }
}
