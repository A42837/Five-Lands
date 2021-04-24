using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Resources;

namespace RPG.Control{
    public class PlayerController : MonoBehaviour
    {
        enum CursorType
        {
            None,
            Movement,
            Combat
        }

        //este system serializable serve para o unity mostrar no editor. da jeito para fazer alterar coisas sem ter que abrir o visual studio
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        [SerializeField] CursorMapping[] cursorMappings = null;

        Health health;
        private float horizontal;
        private float H;
        private float V;
        NavMeshAgent navMeshAgent;
        Vector3 movement = Vector3.zero;
        public Camera cameraOrientation;

        private void Awake() {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update(){

            if (health.IsDead()) return;

            //prioridades são: ataco primeiro, caso nao seja atacável, desloco-me para esse sitio
            if (InteractWithCombat()) return;
            if( WASDMove() ) return;
            if (InteractWithMovement()) return;
            //print("Nothing to do ");

            SetCursor(CursorType.None);
        }

        private bool InteractWithCombat()
         {
            //throw new NotImplementedException();
            RaycastHit[] hits = Physics.RaycastAll( GetMouseRay() );
            foreach(RaycastHit hit in hits){
                CombatTarget target =  hit.transform.GetComponent<CombatTarget>();
                if(target == null) continue;
                
                if(!GetComponent<Fighter>().CanAttack(target.gameObject)){
                    //se nao conseguir atacar, vou para o proximo target
                    continue;
                } 
                if(Input.GetMouseButton(0) ){
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                //mudar o cursor:
                SetCursor(CursorType.Combat);
                return true;
             }
             return false;
         }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = getCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping getCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private bool InteractWithMovement() {
            RaycastHit hit;
            bool hastHit = Physics.Raycast(GetMouseRay(), out hit);  //vou ter informacao de onde carreguei na variavel hit!

            if (hastHit){
                if(Input.GetMouseButton(0)){
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool WASDMove(){


            movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if(movement.magnitude> 0.01f){
                movement = transform.position + Quaternion.Euler(0, cameraOrientation.transform.eulerAngles.y, 0) * movement;
                GetComponent<Mover>().MoveTo(movement, 1f); //1f e para ir a velocidade maximaa
                return true;
            }
            else{
                return false;
            }

        }
    }

}