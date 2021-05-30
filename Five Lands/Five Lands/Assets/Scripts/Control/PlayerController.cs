using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;
//da erro no visual studio e tambem no using, mas o unity compila e funciona na mesma lol
using UnityEngine.EventSystems;
using System;
using GameDevTV.Inventories;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        //este system serializable serve para o unity mostrar no editor. da jeito para fazer alterar coisas sem ter que abrir o visual studio
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance =1f;
        [SerializeField] float spherecastRadius = 1f;
        Health health;
        private float horizontal;
        private float H;
        private float V;
        NavMeshAgent navMeshAgent;
        Vector3 movement = Vector3.zero;
        public Camera cameraOrientation;

        bool isDraggingUI = false;   //flag que me diz se estou com um item no rato

        private void Awake() {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            CheckSpecialAbilityKeys();

            //verificar se o rato esta em cima do UI:
            if (InteractWithUI()) return ;

            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;

            //prioridades são: ataco primeiro, caso nao seja atacável, desloco-me para esse sitio
            if( WASDMove() ) return;
            if (InteractWithMovement()) return;
            //print("Nothing to do ");

            SetCursor(CursorType.None);
        }

        private void CheckSpecialAbilityKeys()
        {
            var actionStore = GetComponent<ActionStore>();
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                actionStore.Use(0, gameObject);
            }
        }

        //posso por exemplo  ter uma porta, que e um component, que reporta um icone diferente ! que responde a raycastable, da mesma maneira 
        //que tenho o CombatTarget!
        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this) )
                    {
                        SetCursor(raycastable.GetCursorType() );    //com o getter, posso distinguir o cursor entre atacar inimigos e apanhar pickups!
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), spherecastRadius);
            float[] distances = new float[hits.Length]; //vai ter as distancias de todos os objectos com que o raycast colide!
            for(int i = 0; i < hits.Length; i++)
            {
                //por no array distances, a distance de cada objecto com que o ray collide, essa informacao esta em hits.distance!
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);    //ordenar o hits de acordo com as distances
            return hits;
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingUI = false;
            }

            //da erro no visual studio e tambem no using, mas o unity compila e funciona na mesma lol
            if (  EventSystem.current.IsPointerOverGameObject())
            {
                if(Input.GetMouseButtonDown(0))
                {
                    isDraggingUI = true;
                }
                SetCursor(CursorType.UI);
                return true;
            }
            if (isDraggingUI)
            {
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
            //RaycastHit hit;
            //bool hastHit = Physics.Raycast(GetMouseRay(), out hit);  //vou ter informacao de onde carreguei na variavel hit!

            Vector3 target;
            bool hastHit = RaycastNavMesh(out target);

            if (hastHit){
                if(Input.GetMouseButton(0)){
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;  //caso nao tenha encontrado nada, posso fazer return

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;
            return true;
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