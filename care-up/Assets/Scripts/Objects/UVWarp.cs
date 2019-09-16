using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVWarp : MonoBehaviour
{
    public enum axis
    {
        X,
        Y,
        Z,
        mX,
        mY,
        mZ,
        None
    };

    public float gridStep = 0;

    public Vector2 offset;
    public Transform _from;
    public Transform _to;
    Vector2 lastOffset;
    Vector2[] uvs;
    Vector3 lastToPos = new Vector3();
    public axis XAxis;
    public axis YAxis;
    public int _type = 0;
    int last_type = 0;
    public Vector2 _shift;

    void Start()
    {
        uvs = gameObject.GetComponent<MeshFilter>().mesh.uv;
    }

    // Update is called once per frame
    void Update()
    {
        //if(_to != null)
        //{
        //    print(_to.localPosition);
        //}
        if (offset != lastOffset || lastToPos != _to.localPosition || last_type != _type)
        {
            _shift = new Vector2();

            if (_to != null)
            {
                switch (XAxis)
                {
                    case axis.X:
                        _shift.x = _to.localPosition.x;
                        break;
                    case axis.Y:
                        _shift.x = _to.localPosition.y;
                        break;
                    case axis.Z:
                        _shift.x = _to.localPosition.z;
                        break;
                    case axis.mX:
                        _shift.x = -_to.localPosition.x;
                        break;
                    case axis.mY:
                        _shift.x = -_to.localPosition.y;
                        break;
                    case axis.mZ:
                        _shift.x = -_to.localPosition.z;
                        break;
                }
                switch (YAxis)
                {
                    case axis.X:
                        _shift.y = _to.localPosition.x;
                        break;
                    case axis.Y:
                        _shift.y = _to.localPosition.y;
                        break;
                    case axis.Z:
                        _shift.y = _to.localPosition.z;
                        break;
                    case axis.mX:
                        _shift.y = -_to.localPosition.x;
                        break;
                    case axis.mY:
                        _shift.y = -_to.localPosition.y;
                        break;
                    case axis.mZ:
                        _shift.y = -_to.localPosition.z;
                        break;
                }
                if (gridStep > 0)
                {
                    _shift.y = (int)(_shift.y / gridStep) * gridStep;
                    _shift.x = (int)(_shift.x / gridStep) * gridStep;

                }
                //print(_shift);
            }
            _shift.x += _type * gridStep;
            Vector2[] new_uvs = new Vector2[uvs.Length];
            for (int i = 0; i < gameObject.GetComponent<MeshFilter>().mesh.uv.Length; i++)
            {
                new_uvs[i] = uvs[i] + offset + _shift;
            }
            gameObject.GetComponent<MeshFilter>().mesh.uv = new_uvs;
        }
        lastOffset = offset;
        last_type = _type;
        if (_to != null)
        {
            lastToPos = _to.localPosition;
        }
    }
}
