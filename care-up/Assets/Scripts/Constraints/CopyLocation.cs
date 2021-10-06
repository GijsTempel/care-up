using UnityEngine;

public class CopyLocation : MonoBehaviour
{
    public Transform anchorObject;

    void Update()
    {
        if (anchorObject != null)
        {
            transform.position = anchorObject.position;
        }
    }
}
