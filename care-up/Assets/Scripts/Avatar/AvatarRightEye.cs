using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRightEye : MonoBehaviour
{
    public UVWarp leftEye;
    // Start is called before the first frame update
    Vector2[] uvs;
    private void Start()
    {
        uvs = gameObject.GetComponent<MeshFilter>().mesh.uv;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2[] new_uvs = new Vector2[uvs.Length];
        Vector2 _shift = leftEye._shift;
        if (_shift.y == -0.125f)
            _shift.y = -0.25f;
        else if (_shift.y == -0.25f)
            _shift.y = -0.125f;
        for (int i = 0; i < gameObject.GetComponent<MeshFilter>().mesh.uv.Length; i++)
        {
            new_uvs[i] = uvs[i] + leftEye.offset + _shift;
        }
        gameObject.GetComponent<MeshFilter>().mesh.uv = new_uvs;
    }
}
