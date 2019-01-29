using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class RigidbodyFirstPersonController : MonoBehaviour
    {
        [HideInInspector]
        public bool tutorial_movementLock = false;
        [HideInInspector]
        public float tutorial_totalLookAround = 0.0f;
        [HideInInspector]
        public float tutorial_totalMoveAround = 0.0f;
        
        public Camera cam;
        public MouseLook mouseLook = new MouseLook();
        
        private Rigidbody m_RigidBody;
        //private CapsuleCollider m_Capsule; // never used
        
        //PlayerPrefsManager prefs;
        
        
        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            //m_Capsule = GetComponent<CapsuleCollider>(); // never used
            mouseLook.Init (transform, cam.transform);

            //prefs = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();


        }


        private void Update()
        {
            // commented unreachable code here
            //if ( false )//prefs.VR )
            //    RotateView();

            if (tutorial_movementLock)
                return;


        }

        private void RotateView()
        {
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            // why do we even have contorller in project still?
            tutorial_totalLookAround += mouseLook.LookRotation (transform, cam.transform, Vector2.zero);
            
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
        }
        
    }
}
