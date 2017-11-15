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

    GameObject moveBackButton;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        mouseLook.Init(transform, cam.transform);

        if (GameObject.Find("Preferences") != null)
        {
            prefs = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        }

        
        GetComponent<Crosshair>().enabled = ( prefs == null ) ? false : prefs.VR;

        controls = GameObject.Find("GameLogic").GetComponent<Controls>();

        groups = new List<WalkToGroup>(
            GameObject.FindObjectsOfType<WalkToGroup>());

        fadeTex = Resources.Load<Texture>("Sprites/Black");

        moveBackButton = GameObject.Find("MoveBackButton");
        moveBackButton.SetActive(false);
    }


    private void Update()
    {
        if (prefs != null)
        {
            if (!prefs.VR)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
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
    }

    public void WalkToGroup(WalkToGroup group)
    {
        if (away)
        {
            ToggleAway();
            savedPos = transform.position;
            savedRot = transform.GetChild(0).GetChild(0).rotation;
            transform.position = group.position;
            if ( prefs == null || (prefs != null && !prefs.VR))
            {
                transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(group.rotation);
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
        moveBackButton.SetActive(!away);
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

    public void MoveBackButton()
    {
        if (!away)
        {
            ToggleAway();
            transform.position = savedPos;
            if (prefs == null || (prefs != null && !prefs.VR))
            {
                transform.GetChild(0).GetChild(0).rotation = savedRot;
            }
        }
    }
}
