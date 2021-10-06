using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectPanelButton : MonoBehaviour {
    GameObject obj;


    public void setObject(GameObject _obj)
    {
        obj = _obj;
        Sprite x = Resources.Load("Sprites/Prefab_Icons/x", typeof(Sprite)) as Sprite;
        GameObject button_icon = transform.Find("Image").gameObject;
        Sprite icon = Resources.Load("Sprites/Prefab_Icons/" + obj.name, typeof(Sprite)) as Sprite;
        if (icon != null)
        {
            button_icon.GetComponent<Image>().sprite = icon;
        }

    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ObjectAction()
    {
        if (obj != null)
        {
            if (obj.GetComponent<PickableObject>() != null)
            {
                // never used
                //bool picked = false;
                HandsInventory inventory = GameObject.FindObjectOfType<HandsInventory>();
                PickableObject pk = obj.GetComponent<PickableObject>();
                if (pk == inventory.leftHandObject || pk == inventory.rightHandObject)
                    return;
                if (inventory.leftHandObject == null)
                {
                    if (inventory.PickItem(pk, PlayerAnimationManager.Hand.Left))
                        pk.CreateGhostObject();
                }
                else if (inventory.rightHandObject == null)
                {
                    if (inventory.PickItem(pk, PlayerAnimationManager.Hand.Right))
                        pk.CreateGhostObject();
                }
            }
        }
    }
}
