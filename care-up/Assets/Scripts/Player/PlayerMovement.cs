using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {

    [Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 8.0f;   // Speed when walking forward
        public float BackwardSpeed = 4.0f;  // Speed when walking backwards
        public float StrafeSpeed = 4.0f;    // Speed when walking sideways
        [HideInInspector] public float CurrentTargetSpeed = 8f;

        public void UpdateDesiredTargetSpeed(Vector2 input)
        {
            if (input == Vector2.zero) return;
            if (input.x > 0 || input.x < 0)
            {
                //strafe
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y < 0)
            {
                //backwards
                CurrentTargetSpeed = BackwardSpeed;
            }
            if (input.y > 0)
            {
                //forwards
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                CurrentTargetSpeed = ForwardSpeed;
            }
        }
    }

    public Camera mainCamera;
    public MovementSettings movementSettings = new MovementSettings();
    public CameraMovement cameraMovement = new CameraMovement();

    private CharacterController controller;

	void Start () {

        controller = GetComponent<CharacterController>();

        cameraMovement.Init(transform, mainCamera.transform);

	}

    void Update() {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        cameraMovement.Update(transform, mainCamera.transform);
    }

    void FixedUpdate()
    {
        Vector2 inputVector = new Vector2
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };
        
        movementSettings.UpdateDesiredTargetSpeed(inputVector);

        Vector3 movementVector = mainCamera.transform.forward * inputVector.y +
                                 mainCamera.transform.right * inputVector.x;
        movementVector *= movementSettings.CurrentTargetSpeed;
        movementVector.y = 0.0f; 

        controller.SimpleMove(movementVector);
	}
}
