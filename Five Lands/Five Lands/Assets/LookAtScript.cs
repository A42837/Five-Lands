using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{
    private Vector3 angle = new Vector3(0,0,0);
    private float turnSpeed = 60f;

    void LateUpdate() {

        if (Input.GetKey(KeyCode.E)){
            angle += Vector3.up * turnSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Q)){
            angle += Vector3.up * -turnSpeed * Time.deltaTime;
        }
        
        /*var rotationVector = transform.rotation.eulerAngles;
        rotationVector.y = angle.y;
        transform.rotation = Quaternion.Euler(rotationVector);*/

        transform.eulerAngles = angle;

        //angle = rotationVector;
    }
}
