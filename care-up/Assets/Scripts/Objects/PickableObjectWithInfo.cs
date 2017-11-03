using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickableObjectWithInfo : PickableObject {

    public abstract void SaveInfo(ref Vector3 left, ref Vector3 right);

    public abstract void LoadInfo(Vector3 left, Vector3 right);
}
