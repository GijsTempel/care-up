using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AssetDebugPanel : MonoBehaviour
{
    bool slideState = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateListOfObjects()
    {
        //GameObject ActionStep = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/ActionStepButton"), content);
        //ActionStep.GetComponent<ActionStepButton>().setAction(a);
        Transform content = transform.Find("ObjectList/Viewport/Content").transform;

        foreach (Transform child in content)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (InteractableObject o in Resources.FindObjectsOfTypeAll<InteractableObject>())
        {
            GameObject assetStat = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/AssetStat"), content);
            string ASLabel = "";
            Color textColor = Color.white;
            switch (o.assetSource)
            {
                case InteractableObject.AssetSource.None:
                    ASLabel = "N";

                    break;
                case InteractableObject.AssetSource.Included:
                    ASLabel = "I";
                    textColor = Color.yellow;
                    break;
                case InteractableObject.AssetSource.Resources:
                    ASLabel = "R";
                    textColor = Color.red;
                    break;
                case InteractableObject.AssetSource.Bundle:
                    ASLabel = "B";
                    textColor = Color.green;
                    break;
                default:
                    break;
            }
            string t = ASLabel + " " + o.name ;
            assetStat.transform.Find("Text").GetComponent<Text>().text = t;
            assetStat.transform.Find("Text").GetComponent<Text>().color = textColor;

            Image ico = assetStat.transform.Find("Image").GetComponent<Image>();

            Sprite l = Resources.Load("Sprites/Prefab_Icons/" + o.name, typeof(Sprite)) as Sprite;
            if (l != null)
                ico.sprite = l;
        }
    }

    public void toggleSlide()
    {
        slide(!slideState);
    }

    public void slide(bool value)
    {
        if (slideState != value)
        {
            slideState = value;
            if (value)
                GetComponent<Animator>().SetTrigger("in");
            else
                GetComponent<Animator>().SetTrigger("out");

        }
    }
}
