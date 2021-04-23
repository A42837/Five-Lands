using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Resources;

namespace RPG.Control{
    public class AIController : MonoBehaviour{

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        //este patrol path tem que ser puxado para o inimigo na cena ! manualmente
        [SerializeField] PatrolPath PatrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;  //20% do maximo speed
        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;

        //estados dos inimigos: guardar a localização inicial, suspeitar e patrulha
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;       // esta variavel diz qual e o meu proximo waypoint

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
        }

        private void Start() {
            //a posicao onde o guarda começa o jogo, é o que ele vai estar guardar, e a voltar para quando o player sai de range!:
            guardPosition = transform.position;
        }

        private void Update()
        {

            if (health.IsDead()) return;
            if (InAttackRangeOfPlayer(player) && fighter.CanAttack(player))
            {
                //ATACK STATE
                //print(gameObject.name + " Chaseeee!");
                AttackBehaviour();
            }

            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //SUSPICION STATE
                SuspicionBehaviour();
            }

            else
            {
                //GUARDING INITIAL POSITION STATE
                //caso o player sair de range, volto para a posição inicial!
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if(PatrolPath != null){
                if(AtWaypoint()){
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(timeSinceArrivedAtWaypoint > waypointDwellTime ){
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
            
        }

        private Vector3 GetCurrentWaypoint()
        {
            return PatrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = PatrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint() );
            return distanceToWaypoint < waypointTolerance;

        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer(GameObject player)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        //para visualizar Gizmos, apenas do objecto que tiver selecionado 
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
            
        }
    }
    
}


