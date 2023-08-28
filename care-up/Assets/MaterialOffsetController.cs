using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOffsetController : MonoBehaviour
{
    public GameObject boneToControllOffset;
    public GameObject controlledObject;
    public float offsetX = 0;
    public float offsetY = 0;
    public bool offsetControll = false;
    Renderer objectRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (controlledObject != null)
        {
            
            objectRenderer = controlledObject.GetComponent<Renderer>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        offsetX = boneToControllOffset.transform.localPosition.x;
        offsetY = boneToControllOffset.transform.localPosition.z;

        if (offsetControll)
        {
            if (controlledObject.activeSelf)
                objectRenderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);
        }
    }
}
