using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
    
public class PlayerScript : MonoBehaviour {
    [HideInInspector]
    public bool tutorial_movementLock = false;
    [HideInInspector]
    public float tutorial_totalLookAround = 0.0f;
    [HideInInspector]
    public float tutorial_totalMoveAround = 0.0f;

    public Camera cam;
    public MouseLook mouseLook = new MouseLook();

    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Capsule;

    PlayerPrefsManager prefs;
    Controls controls;

    public bool away = true;
    private Vector3 savedPos;
    private Quaternion savedRot;
    private List<WalkToGroup> groups;

    private bool fade;
    private float fadeTime = 1f;
    private float fadeTimer = 0.0f;
    Texture fadeTex;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        mouseLook.Init(transform, cam.transform);

        if (GameObject.Find("Preferences") != null)
        {
            prefs = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        }

        GetComponent<Crosshair>().enabled = prefs.VR;

        controls = GameObject.Find("GameLogic").GetComponent<Controls>();

        groups = new List<WalkToGroup>(
            GameObject.FindObjectsOfType<WalkToGroup>());

        fadeTex = Resources.Load<Texture>("Sprites/Black");
    }


    private void Update()
    {
        if ( prefs.VR )
            RotateView();
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (tutorial_movementLock)
            return;

        if (controls.MouseClicked())
        {
            if (controls.SelectedObject != null && 
                controls.SelectedObject.GetComponent<WalkToGroup>())
            {
                WalkToGroup(controls.SelectedObject.GetComponent<WalkToGroup>());
            }
        }

        if (controls.keyPreferences.Teleport.Pressed())
        {
            if (!away)
            {
                ToggleAway();
                transform.position = savedPos;
                if (!prefs.VR)
                {
                    transform.rotation = savedRot;
                }
            }
        }
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        tutorial_totalLookAround += mouseLook.LookRotation(transform, cam.transform);

        // Rotate the rigidbody velocity to match the new direction that the character is looking
        Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
        m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
    }

    public void WalkToGroup(WalkToGroup group)
    {
        if (away)
        {
            ToggleAway();
            savedPos = transform.position;
            savedRot = transform.rotation;
            transform.position = group.position;
            if (!prefs.VR)
            {
                transform.rotation = Quaternion.Euler(group.rotation);
            }
        }
    }

    private void ToggleAway()
    {
        fade = true;
        away = !away;
        foreach (WalkToGroup g in groups)
        {
            g.enabled = away;
            g.GetComponent<Collider>().enabled = away;
        }
    }

    private void OnGUI()
    {
        if (fade)
        {
            if (fadeTimer > fadeTime)
            {
                fadeTimer = 0.0f;
                fade = false;
            }
            else
            {
                GUI.color = new Color(0.0f, 0.0f, 0.0f, 1.0f -  
                    Mathf.InverseLerp(0.0f, fadeTime, fadeTimer));
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTex);
                fadeTimer += Time.deltaTime;
            }
        }
    }
}
