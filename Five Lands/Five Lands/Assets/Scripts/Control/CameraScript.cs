using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class CameraScript : MonoBehaviour
    {
        public Camera cameraTop;
        public Camera cameraThird;
        public GameObject look;
        private Vector3 angle = new Vector3(0,0,0);
        private Vector3 resetAng = new Vector3(0,0,0);

        private float turnSpeed = 60f;

        void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.C))
            {
                //print("CAMERA SWITCH");
                cameraTop.enabled = !cameraTop.enabled;
                cameraThird.enabled = !cameraThird.enabled;
            }*/

            if (Input.GetKey(KeyCode.E)){
                look.transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime); // = Quaternion.Euler(turnSpeed, 0.0f, gameObject.transform.rotation.z * -1.0f);
            }
            else if (Input.GetKey(KeyCode.Q)){
                look.transform.Rotate(Vector3.up * -turnSpeed * Time.deltaTime);
            }
            look.transform.Rotate(resetAng);
        }

        public void OnclickSetCameraRotation()
        {
            //look.transform.rotation = Quaternion.Euler(0.0f, transform.rotation.y, 0.0f);
            look.transform.localRotation = Quaternion.identity;
        }

        
    }
}
