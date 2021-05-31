using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control{
    public class AIController : MonoBehaviour{

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float aggroCooldownTime = 3f;
        //este patrol path tem que ser puxado para o inimigo na cena ! manualmente
        [SerializeField] PatrolPath PatrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] float shoutDistance = 5f;  //distancia a que os inimigos perto vao ser chamados !
        [Tooltip("esta flag serve para AI que nao seja aggresivas, tipo cavalos, vacas...")]
        [SerializeField] bool amIAggressive = true;
        [Tooltip("This flag is used when the AI is a dragon type")]
        [SerializeField] bool amIADragon = false;
        [Tooltip("This is the weapon config dragons use when attacking from a range. The Dragon will cast the fireball at half the chase distance or greater (never bigger than the chase distance itself)")]
        [SerializeField] WeaponConfig dragonRangedAttack = null;


        bool hasBeenAggroedRecently = false;


        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;  //20% do maximo speed
        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;

        //estados dos inimigos: guardar a localização inicial, suspeitar e patrulha
        LazyValue<Vector3> guardPosition;
        //mathf infinity como valor inicial para que quando começa, é como se nunca os tivesse visto!
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int currentWaypointIndex = 0;       // esta variavel diz qual e o meu proximo waypoint

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start() {
            //a posicao onde o guarda começa o jogo, é o que ele vai estar guardar, e a voltar para quando o player sai de range!:
            guardPosition.ForceInit();
        }

        private void Update()
        {

            if (health.IsDead()) return;

            float dist = Vector3.Distance(player.transform.position, this.transform.position);
            if (Input.GetKeyDown(KeyCode.T))
            {
                print("dist--->" + dist);
            }
            if (amIADragon && dist >= (chaseDistance / 2) && dist < chaseDistance)
            {
                AttackBehaviour();
                fighter.EquipWeapon(dragonRangedAttack);
                return;
            }
            if (IsAggrevated(player) && fighter.CanAttack(player))
            {
                //ATACK STATE
                //print(gameObject.name + " Chaseeee!");
                AttackBehaviour();
                fighter.EquipWeapon(GetComponent<Fighter>().getDefaultWeapon());
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

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;

            if(timeSinceAggrevated >= aggroCooldownTime && timeSinceLastSawPlayer >= suspicionTime)
            {
                hasBeenAggroedRecently = false;
            }

        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

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

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            //vou usar um spherecast! é parecido com o raycast, mas tem uma margem de erro, não precisa de ser exatamente no sitio certo!
            //neste caso, faço um spherecast para cima, como so quero os enemies a votla do enemieOriginal digo que a distancia que a spherecast vai
            //percorrer é zero
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null || !ai.amIAggressive) continue;  //se for uma AI não-aggresiva, tipo cavalo, vaca

                ai.AggroAllies();
            }
        }

        public void AggroAllies()
        {
            if (hasBeenAggroedRecently == true) return;
            if(hasBeenAggroedRecently == false)
            {
                print("I will join the FIGHT !!");
                timeSinceAggrevated = 0f;
                timeSinceLastSawPlayer = 0f;
                hasBeenAggroedRecently = true;
            }
        }

        private bool IsAggrevated(GameObject player)
        {
            //tem que se por tambem para o pet, actualmente so funciona para o player!
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroCooldownTime;
        }

        //para visualizar Gizmos, apenas do objecto que tiver selecionado 
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
            
        }
    }
    
}


