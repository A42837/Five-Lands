using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {

        private void Update()
        {
            //por o texto sempre a apontar para a camera activa ! funciona para ambas as cameras
            transform.forward = Camera.main.transform.forward;
        }

    }
}
