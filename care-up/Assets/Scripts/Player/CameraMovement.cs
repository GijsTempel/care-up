using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CameraMovement {

    public float sensetivity = 2.0f;
    public float minViewAngle = -90.0f;
    public float maxViewAngle = 90.0f;
    public float smoothTime = 5.0f;

    private Quaternion playerAngle;
    private Quaternion cameraAngle;

    public void Init(Transform player, Transform camera)
    {
        playerAngle = player.localRotation;
        cameraAngle = camera.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update(Transform player, Transform camera)
    {
        float yRotation = Input.GetAxis("Mouse X") * sensetivity;
        float xRotation = Input.GetAxis("Mouse Y") * sensetivity;

        playerAngle *= Quaternion.Euler(0f, yRotation, 0f);
        cameraAngle *= Quaternion.Euler(-xRotation, 0f, 0f);

        cameraAngle = ClampRotationAroundXAxis(cameraAngle);

        player.localRotation = Quaternion.Slerp(player.localRotation, playerAngle, smoothTime * Time.deltaTime);
        camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraAngle, smoothTime * Time.deltaTime);

        CursorLockUpdate();
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float xAngle = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        xAngle = Mathf.Clamp(xAngle, minViewAngle, maxViewAngle);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * xAngle);

        return q;
    }

    private void CursorLockUpdate()
    {
        if( Input.GetKeyUp(KeyCode.Escape) )
        {
            GameObject.Find("GameLogic").GetComponent<IngameMenu>().Open();
       
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
