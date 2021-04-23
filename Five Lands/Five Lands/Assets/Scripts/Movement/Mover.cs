using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement{
    public class Mover : MonoBehaviour, IAction, ISaveable {

        [SerializeField] Transform target; // este targer e a esfera invisivel para onde vai o player !
        [SerializeField] float maxSpeed = 6f;
        NavMeshAgent navMeshAgent;

        Health health;
        float speed;
        Vector3 velocity;
        Vector3 localVelocity;
        Animator animator;

        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction){
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel(){
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            //obter a velocidade global do navMesh. covnerter a velocidade global para a local. senao fizer isto
            // a velocidade parece muito lenta e nao importa onde estiver no mundo!

            velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            speed = localVelocity.z;
            animator.SetFloat("forwardSped", speed);
        }

        public object CaptureState()
        {
            //tenho que usar a classe com o formato correcto, o serialized vector 3 que e igual mas tem no topo [System.Serializable]
            return new SerializableVector3(transform.position);
        }

        //restore state e chamado depois de awake mas antes do start
        public void RestoreState(object state)
        {
            //converter a posição do seriazable vector 3 para o transform
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
