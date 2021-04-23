using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CInematicTrigger : MonoBehaviour
    {
        bool alreadyTrigged = false;
        public Camera cameraTop;
        public Camera cameraThird;
        private void OnTriggerEnter(Collider other)
        {
            if(!alreadyTrigged && other.tag == "Player")
            {
                //caso esteja com a camera de 3º pessoa, dou disable para ficar com a topDown
                if (cameraThird.isActiveAndEnabled)
                {
                    cameraTop.enabled = !cameraTop.enabled;
                    cameraThird.enabled = !cameraThird.enabled;
                }
                alreadyTrigged = true;
                GetComponent<PlayableDirector>().Play();
            }
            
        }
    }
}
