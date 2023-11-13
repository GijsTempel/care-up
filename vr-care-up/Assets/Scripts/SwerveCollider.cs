using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveCollider : MonoBehaviour
{
    public SwerveActionTrigger swerveActionTrigger;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("@SwerveCollision:" + collision.name + " " + Random.Range(0, 9999).ToString());
        swerveActionTrigger.SwerveCollision(this, collision.name);
    }

}
