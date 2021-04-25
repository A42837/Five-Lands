using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                PickUp(other.GetComponent<Fighter>());
            }
        }

        private void PickUp(Fighter figher)
        {
            figher.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            //percorrer todos os filhos para os esconder tambem:
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            //tenho o rato por cima da weapon, logo retorno sempre true para pode a apanhar, Mas apenas apanho o pickup se carregar com o botão esquerdo 
            if( Input.GetMouseButtonDown(0))
            {
                PickUp(callingController.GetComponent<Fighter>());

            }
            return true;
        }
    }
}
