using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShowHideDeleteAction : MonoBehaviour
{
    public bool toShow = true;
    public bool toDelete = false;
    public float waitTime = 0f;
    bool timeOutStarted = false;
    public string ControlObjectName = "";
    public List<string> ObjNames;
    public bool meshRenderer = false;

    void Update()
    {
        if (timeOutStarted)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                Proceed();
                timeOutStarted = false;
            }
        }
    }

    public void StartTimeout()
    {
        timeOutStarted = true;
    }

    void Proceed()
    {
        if (ControlObjectName == "-" || ControlObjectName == "")
        {
            foreach (string name in ObjNames)
            {
                if (GameObject.Find(name) != null)
                {
                    ShowHideObj(GameObject.Find(name));
                }
            }
        }
        else
        {
            GameObject controlObject = GameObject.Find(ControlObjectName);
            PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
            if (player != null)
            {
                GameObject leftObj = player.GetObjectInHand(true);
                GameObject rightObj = player.GetObjectInHand(false);

                if (leftObj != null && leftObj.name == ControlObjectName && leftObj.GetComponent<ShowHideObjects>() != null)
                    controlObject = leftObj;
                else if (rightObj != null && rightObj.name == ControlObjectName && rightObj.GetComponent<ShowHideObjects>() != null)
                    controlObject = rightObj;

            }
            if (controlObject != null && ObjNames.Count != 0)
            {
                if (controlObject.GetComponent<ShowHideObjects>() != null)
                {
                    ShowHideObjects ControlObject = controlObject.GetComponent<ShowHideObjects>();
                    foreach (string name in ObjNames)
                    {
                        ControlObject._show(name, toShow, meshRenderer);
                    }
                }
            }
        }
    }

    void ShowHideObj(GameObject _obj)
    {
        if (toDelete)
        {
            Destroy(_obj);
        }
        if (meshRenderer && (_obj.GetComponent<MeshRenderer>() != null || _obj.GetComponent<SkinnedMeshRenderer>() != null))
        {
            if (!toShow)
            {
                if (_obj.GetComponent<MeshRenderer>() != null)
                    _obj.GetComponent<MeshRenderer>().enabled = false;
                else
                    _obj.GetComponent<SkinnedMeshRenderer>().enabled = false;

            }
            else
            {
                foreach (MeshRenderer m in _obj.GetComponents<MeshRenderer>())
                {
                    m.enabled = true;
                }
                foreach (SkinnedMeshRenderer m in _obj.GetComponents<SkinnedMeshRenderer>())
                {
                    m.enabled = true;
                }
            }
        }
        else
        {
            _obj.SetActive(toShow);
            
        }
    }

}
