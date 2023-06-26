using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class PickableObject : MonoBehaviour
{
    bool isKinematic = false;
    private PlayerScript player;
    private Transform transformToFallow;

    public void Drop()
    {
        transformToFallow = null;
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().isKinematic = isKinematic;
}
    
    public bool PickUp(Transform handTransform)
    {
        FallowTransform(handTransform);
        Debug.Log("@ ## " + name + ":" + Random.Range(0, 9999).ToString());
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        return true;
    }

    public void FallowTransform(Transform trans)
    {
        transformToFallow = trans;
    }

    private void Update()
    {
        if (transformToFallow != null)
        {
            transform.position = transformToFallow.position;
            transform.rotation = transformToFallow.rotation;
        }

    }
    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
            isKinematic = gameObject.GetComponent<Rigidbody>().isKinematic;
        player = GameObject.FindObjectOfType<PlayerScript>();
    }
}
