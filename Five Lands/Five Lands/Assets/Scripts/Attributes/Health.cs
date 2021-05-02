using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using UnityEngine.Events;
using GameDevTV.Utils;

namespace RPG.Attributes{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        //este lazy value garante-me que as variaveis são chamadas depois do awake, mas antes do start, ou seja, antes da sua primeira utilização!
        LazyValue<float> healthPoints;

        bool isDead = false;

        public bool IsDead(){
            return isDead;
        }

        private void Awake()
        {
            //os healthpoints so vao ser definidos pela funcao getInityialHealth, quando for possivel chamar essa mesma funcao
            healthPoints = new LazyValue<float>(GetInitialHealth);

        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints() );
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            //este force init e para caso quando o start e chamado o healthpooints ainda não ter sido chamado, vou inicializa-lo agora!
            healthPoints.ForceInit();
            
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        //instigator e o tropa que esta a atacar !
        public void TakeDamage(GameObject instigator, float damage){
            //print(gameObject.name + " took damage: " + damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            takeDamage.Invoke(damage);        //usar os UnityEvents! estão definidos no editor !
            if (healthPoints.value == 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            /*
            else
            {
                takeDamage.Invoke(damage);        //usar os UnityEvents! estão definidos no editor !
            }
            */
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die(){
            if(isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            print("Award XP called!");
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }


        public object CaptureState()
        {
            return healthPoints.value;
        }


        public void RestoreState(object state)
        {
            //aqui o state vai ser um float, logo posso fazer este cast
            healthPoints.value = (float)state;
            if (healthPoints.value == 0)
            {
                Die();
            }
        }
    }
}
