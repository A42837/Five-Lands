using UnityEngine;
using RPG.Resources;

namespace RPG.Companion
{

    public class Companion_Behavior : MonoBehaviour
    {

        [SerializeField] Transform player;
        bool isAttacking = false, isChasing = false, isTargetAlive;
        float rotSpeed = 3.0f, moveSpeed = 4.0f;
        GameObject[] enemies;
        GameObject target;
        float targetHealth;


        void Start()
        {
            enemies = GameObject.FindGameObjectsWithTag("ENEMY");
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
        }

        private void Follow()
        {
            //LookAt
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotSpeed * Time.deltaTime);
            //Move
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
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
            GetComponent<Animator>().SetTrigger("Attack");
        }

    }
}
