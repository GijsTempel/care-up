using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class PickableObject : MonoBehaviour
{
    private float poseTransitionDuration = 0.2f;
    private float routineTime = float.PositiveInfinity;

    private Vector3 startPos;
    private Quaternion startRotation;


    bool isKinematic = false;
    private PlayerScript player;
    private Transform transformToFallow;

    public void Drop()
    {
        transformToFallow = null;
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().isKinematic = isKinematic;
}
    
    public bool PickUp(Transform handTransform, float transuitionDuration = 0.2f)
    {
        FallowTransform(handTransform, transuitionDuration);
        Debug.Log("@ ## " + name + ":" + Random.Range(0, 9999).ToString());
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        return true;
    }

    public void FallowTransform(Transform trans, float transuitionDuration = 0.2f)
    {
        poseTransitionDuration = transuitionDuration;
        startPos = transform.position;
        startRotation = trans.rotation;
        routineTime = 0f;
        transformToFallow = trans;
    }

    private void Update()
    {
        float lerpValue = routineTime / poseTransitionDuration;
        if (transformToFallow != null)
        {
            Vector3 p = Vector3.Lerp(startPos, transformToFallow.position, lerpValue);
            Quaternion r = Quaternion.Lerp(startRotation, transformToFallow.rotation, lerpValue);

            transform.position = p;
            transform.rotation = r;

        }
        routineTime += Time.deltaTime;

    }
    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
            isKinematic = gameObject.GetComponent<Rigidbody>().isKinematic;
        player = GameObject.FindObjectOfType<PlayerScript>();
    }
}
