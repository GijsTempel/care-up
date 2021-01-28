using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        [HideInInspector]
        public bool lookOnly = false;

        public bool clampVerticalRotation = true;
        public bool clampHorisontalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public float MinimumY = -90F;
        public float MaximumY = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;
        
        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;

        public bool savedRot = false;
        private Quaternion savedCamRot;
        private Quaternion savedCharRot;

        public Quaternion SavedCamRot
        {
            get { return savedCamRot; }
        }

        public Quaternion SavedCharRot
        {
            get { return savedCharRot; }
        }

        // windows
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;

        //touches (android, possibly other phones)
        public float XTouchSensetivity = 0.03f;
        public float YTouchSensetivity = 0.03f;

        // OSX
        public float XMacSensetivity = 50f;
        public float YMacSensetivity = 50f;

        // iOS
        public float XiOSSensetivity = 1.0f;
        public float YiOSSensetivity = 1.0f;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }
        
        public float LookRotation(Transform character, Transform camera, Vector2 amount)
        {
            float yRot = 0.0f, xRot = 0.0f;

            /*if (Input.touchCount > 0)
            {
                xRot = Input.GetTouch(0).deltaPosition.y * XTouchSensetivity;
                yRot = Input.GetTouch(0).deltaPosition.x * YTouchSensetivity;
            }
            else
            {
                #if UNITY_STANDALONE_OSX
                    xRot = Input.GetAxisRaw("Mouse Y") * XMacSensetivity;
                    yRot = Input.GetAxisRaw("Mouse X") * YMacSensetivity;
                #elif UNITY_IOS
                    xRot = Input.GetAxisRaw("Mouse Y") * XiOSSensetivity;
                    yRot = Input.GetAxisRaw("Mouse X") * YiOSSensetivity;
                #else
                    xRot = Input.GetAxisRaw("Mouse Y") * XSensitivity;
                    yRot = Input.GetAxisRaw("Mouse X") * YSensitivity;
                #endif
            }*/

            yRot = amount.x * YSensitivity;
            xRot = amount.y * XSensitivity;

            if (lookOnly)
            {
                m_CameraTargetRot *= Quaternion.Euler(-xRot, yRot, 0f);
                m_CameraTargetRot = Quaternion.Euler(m_CameraTargetRot.eulerAngles.x, m_CameraTargetRot.eulerAngles.y, 0f);
            }
            else
            {
                m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
                m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);
            }

            if(clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);
            if(clampHorisontalRotation)
                m_CameraTargetRot = ClampRotationAroundYAxis(m_CameraTargetRot);

            if (lookOnly)
            {
                if (smooth)
                {
                    camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                        smoothTime * Time.deltaTime);
                }
                else
                {
                    camera.localRotation = m_CameraTargetRot;
                }
            }
            else
            {
                if (smooth)
                {
                    character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                        smoothTime * Time.deltaTime);
                    camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                        smoothTime * Time.deltaTime);
                }
                else
                {
                    character.localRotation = m_CharacterTargetRot;
                    camera.localRotation = m_CameraTargetRot;
                }
            }

            //UpdateCursorLock();

            return new Vector2(xRot, yRot).magnitude;
        }

        public void SaveRot(Transform character, Transform camera)
        {
            savedCamRot = camera.rotation;
            savedCharRot = character.rotation;
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if(!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        Quaternion ClampRotationAroundYAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);

            angleY = Mathf.Clamp(angleY, MinimumY, MaximumY);

            q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

            return q;
        }

        public void SetMode(bool value, Quaternion cam)
        {
            lookOnly = value;
            if (!value)
            {
                m_CameraTargetRot = cam;
            }
        }
    }
}
