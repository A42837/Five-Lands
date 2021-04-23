using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{
    public class CameraThird : MonoBehaviour
    {
        public Transform target;
        public float damping = 4;
        private Vector3 offset;
        private float currentAngle;
        private float desiredAngle;
        private float angle;
        private Quaternion rotation;
        private float horizontal;
        public float turnSpeed = 4.0f;
        public Transform lookat;

        void Start()
        {
            transform.position = new Vector3(0, 0, 0);
            //offset = target.position;

            offset = new Vector3(-2.9f, 2f, -1.82f);
        }


        void LateUpdate()
        {
            /*currentAngle = transform.eulerAngles.y;
            desiredAngle = target.eulerAngles.y;

            angle = Mathf.LerpAngle(currentAngle, desiredAngle, damping * Time.deltaTime);

            rotation = Quaternion.Euler(0, angle, 0);

            transform.position = target.position + (rotation * offset);

            //transform.LookAt(target.transform);*/


            /*horizontal = Input.GetAxisRaw("Mouse X") * 4;
            //transform.Rotate(0, horizontal, 0);

            rotation = Quaternion.Euler(0, horizontal, 0);

            transform.position = target.position + (rotation * offset);*/

            //caso prima o botão direito do rato, viro a camera e o personagem segue! tipo MMO!
            if(Input.GetMouseButton(1)){
                offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
            }
            //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
            transform.position = target.position + offset;
            transform.LookAt(lookat.position);

            //rotation = Quaternion.Euler(0, Input.GetAxis("Mouse X") * turnSpeed, 0);
            //transform.position = target.position + Quaternion.Euler(0, Input.GetAxis("Mouse X") * turnSpeed, 0) * offset;
            //transform.LookAt(target.position);
        }
    }
}
