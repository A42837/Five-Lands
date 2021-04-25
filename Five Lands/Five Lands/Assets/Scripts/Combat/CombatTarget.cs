using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resources;
using RPG.Control;

namespace RPG.Combat{


    //este require poe o script da vida automaticamente quando ponho este combat target!
    [RequireComponent(typeof(Health))]

    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            if (!GetComponent<Fighter>().CanAttack(gameObject))
            {
                //se nao conseguir atacar, vou para o proximo target
                return false;
            }
            if (Input.GetMouseButton(0))
            {
                GetComponent<Fighter>().Attack(gameObject);
            }
            
            return true;
        }
    }
}
