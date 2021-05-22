using UnityEngine;
using RPG.Attributes;
using RPG.Combat;
using RPG.Stats;
using UnityEngine.AI;

namespace RPG.Companion
{

    public class Companion_Behavior : MonoBehaviour
    {

        [SerializeField] Transform player;
        [SerializeField] Projectile projectile;
        [SerializeField] Transform mouth;

        bool isAttacking = false, isChasing = false, isTargetAlive;
        float rotSpeed = 3.0f, moveSpeed = 4.0f;

        Health targetHealth;

        GameObject[] enemies;
        GameObject target;
        NavMeshAgent navMeshAgent;


        void Start()
        {
            enemies = GameObject.FindGameObjectsWithTag("ENEMY");
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (!isAttacking)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) <= 15)
                    {
                        if (enemy.GetComponent<Health>().GetHealthPoints() > 0)
                        {
                            isTargetAlive = true;
                            isAttacking = true;
                            target = enemy;
                            break;
                        }
                    }
                }

                if (Vector3.Distance(transform.position, player.position) > 2) Follow();

                else
                {
                    GetComponent<Animator>().SetFloat("Speed", 0);
                    navMeshAgent.isStopped = true;

                }
            }

            else if (isTargetAlive)
            {
                targetHealth = target.GetComponent<Health>();
                if (targetHealth.GetHealthPoints() <= 0)
                {
                    isTargetAlive = false;
                    isAttacking = false;
                }
                else if (Vector3.Distance(target.transform.position, transform.position) > 10) Chase();
                else Attack();
            }
        }


        public Health GetTarget()
        {
            return targetHealth;
        }

        /*void Update()
        {
            if (!isAttacking)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) <= 15)
                    {
                        if (enemy.GetComponent<Health>().GetHealthPoints() > 0)
                        {
                            isAttacking = true;
                            target = enemy;
                            isTargetAlive = true;
                            break;
                        }
                    }
                }

                if (Vector3.Distance(transform.position, player.position) > 2) Follow();

                else GetComponent<Animator>().SetFloat("Speed", 0);
            }

            else if (isTargetAlive)
            {
                if (target.GetComponent<Health>().GetHealthPoints() <= 0)
                {
                    isTargetAlive = false;
                    isAttacking = false;
                }
                else if (Vector3.Distance(target.transform.position, transform.position) > 2) Chase();
                else Attack();
            }
        }*/

        /*
        private void Follow()
        {
            //LookAt
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotSpeed * Time.deltaTime);
            //Move
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            GetComponent<Animator>().SetFloat("Speed", moveSpeed);
        }
        */
        private void Follow()
        {
            //LookAt
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotSpeed * Time.deltaTime);
            //Move
            navMeshAgent.destination = player.position;
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.isStopped = false;
            GetComponent<Animator>().SetFloat("Speed", moveSpeed);
        }


        private void Chase()
        {
            //LookAt
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), rotSpeed * Time.deltaTime);
            //Move
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            GetComponent<Animator>().SetFloat("Speed", moveSpeed);
        }

        private void Attack()
        {
            GetComponent<Animator>().SetFloat("Speed", 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), rotSpeed * Time.deltaTime);
            GetComponent<Animator>().SetTrigger("attack");
        }

        /*
         * para a aranha conseguir ganhar experiencia é preciso usar o fighter e por isso tive que la este metodo. acrescentei um getter para por o target
        public void SpitVenom()
        {
            Projectile projectileInstance = Instantiate(projectile, mouth.position, Quaternion.identity);
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            projectileInstance.setTarget(targetHealth, target, damage);
        }
        */

    }
}
