using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropControl : MonoBehaviour
{
    public string dropPointName;

    public List<Vector3> GetPoints()
    {
        List<Vector3> points = new List<Vector3>();
        for(int i = 0; i < transform.childCount; i++)
            points.Add(transform.GetChild(i).transform.position);

        return points;
    }
    
}
