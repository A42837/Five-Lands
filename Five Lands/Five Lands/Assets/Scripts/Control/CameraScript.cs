using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class CameraScript : MonoBehaviour
    {
        public Camera cameraTop;
        public Camera cameraThird;

        void Start()
        {
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                //print("CAMERA SWITCH");
                cameraTop.enabled = !cameraTop.enabled;
                cameraThird.enabled = !cameraThird.enabled;
            }
        }
    }
}
