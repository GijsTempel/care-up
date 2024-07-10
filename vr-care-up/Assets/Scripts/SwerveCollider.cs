using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveCollider : MonoBehaviour
{
    public SwerveActionTrigger swerveActionTrigger;

    private void OnTriggerEnter(Collider collision)
    {
        swerveActionTrigger.SwerveCollision(this, collision.name);
    }

}
