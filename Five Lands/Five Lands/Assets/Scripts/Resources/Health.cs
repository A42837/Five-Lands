using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;

        public float healthPoints = -1f;

        bool isDead = false;

        public bool IsDead(){
            return isDead;
        }

        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;

            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }

        public void TakeDamage(GameObject instigator, float damage){
            print(gameObject.name + " took damage: " + damage);

            healthPoints = Mathf.Max(healthPoints - damage, 0);
            //print(healthPoints);
            if(healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        

        public float GetPercentage()
        {
            return 100 * healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die(){
            if(isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }


        public object CaptureState()
        {
            return healthPoints;
        }


        public void RestoreState(object state)
        {
            //aqui o state vai ser um float, logo posso fazer este cast
            healthPoints = (float)state;
            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}
