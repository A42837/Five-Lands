using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.SceneManagent;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        //enum e uma drop down list no unity editor
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }
        //esta variavel e o numero da scene que e para fazer load. tambem podia usar o nome mas depois se mudo o nome ja nao funciona
        //para ver este numero vou as build settings e arrasto a scene para la e sei o numero!
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;

        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;


        private void OnTriggerEnter(Collider other)
        {
      
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if(sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }


            DontDestroyOnLoad(gameObject);//tem que estar na root, nao pode destar agarrado a nada!

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

            //Save current level
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            //SceneManager.LoadScene(sceneToLoad);    
            yield return SceneManager.LoadSceneAsync(sceneToLoad);  //faz o load da cena assincronamente e quando acabar retorna!

            //Load current Level
            wrapper.Load();

            Portal otherPortal = GetOtherPortal();  //o portal onde vou aparecer
            UpdatePlayer(otherPortal);
            //UpdateCompanion(otherPortal);

            //checkpoint para a posicao do player
            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);  //esperar que a camera fique fixa!
            yield return fader.FadeIn(fadeInTime);
            
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
        /*private void UpdateCompanion(Portal otherPortal)
        {
            GameObject companion = GameObject.FindWithTag("Companion");
            companion.GetComponent<NavMeshAgent>().enabled = false;
            companion.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            companion.transform.position = otherPortal.spawnPoint.position;
            companion.transform.rotation = otherPortal.spawnPoint.rotation;
            companion.GetComponent<NavMeshAgent>().enabled = true;
        }*/

        private Portal GetOtherPortal()
        {
            foreach ( Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }
            return null;
        }
    }
}
