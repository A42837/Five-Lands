using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat{


    //este require poe o script da vida automaticamente quando ponho este combat target!
    [RequireComponent(typeof(Health))]

    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        //posso por exemplo  ter uma porta, que e um component, que reporta um icone diferente !
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {

            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject) )
            {
                //se nao conseguir atacar, vou para o proximo target
                return false; ;
            }
            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}
