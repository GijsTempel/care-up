﻿//
// Fingers Gestures
// (c) 2015 Digital Ruby, LLC
// http://www.digitalruby.com
// Source code may be used for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRubyShared
{
    public class DemoScriptJoystick : MonoBehaviour
    {
        [Tooltip("Fingers Joystick Script")]
        public FingersJoystickScript JoystickScript;
        
        [Tooltip("Whether joystick moves to touch location")]
        public bool MoveJoystickToGestureStartLocation;

        private PlayerScript player;

        private void Start()
        {
            player = GameObject.FindObjectOfType<PlayerScript>();
        }

        private void Awake()
        {
            JoystickScript.JoystickExecuted = JoystickExecuted;
            JoystickScript.MoveJoystickToGestureStartLocation = MoveJoystickToGestureStartLocation;
        }

        private void JoystickExecuted(FingersJoystickScript script, Vector2 amount)
        {
            //Debug.LogFormat("Joystick: {0}", amount);
 
            player.freeLook = true;
            player.LookRotationUpdate(amount);
        }
    }
}
