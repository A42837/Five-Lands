using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using RPG.Companion;

namespace RPG.Combat{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider{

        
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeapon = null;

        private void Start()
        {
            if (currentWeapon == null) {
                EquipWeapon(defaultWeapon);
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeapon = weapon; //o current weapon e o weapon que eu quero equipar!
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void Update(){
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (target != null && !GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                //este IF e para excluir o caso do companion!
                if(GetComponent<Companion_Behavior>() == null)
                {
                    GetComponent<Mover>().Cancel(); //para o movimento
                    AttackBehaviour();
                }
            }
        }

        private void AttackBehaviour(){
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                //vai chamar o evento da animacao! Hit()
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event, quando dou o soco!
        void Hit(){
            //print("target spider--->" + target.name);
            if(target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.HasProjectile())
            {
                //print("spider project!");
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }
        //outro envento de animacao, o nome mesmo da animacao e diferente dai ter que ter isto extra
        void Shoot()
        {
            Hit();
        }

        //evento de animacao da spider
        public void SpitVenom()
        {
            //print("spider called!");
            target = GetComponent<Companion_Behavior>().GetTarget();
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

        public bool CanAttack(GameObject combatTarget){
            if(combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();

        }

        public void Attack(GameObject combatTarget){
            //print("Attacked !");
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel(); //para o movimento
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            //se o meu stat for o de damage:
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {


            //se o meu stat for o de damage:
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageBonus();
            }
        }

        public Health GetTarget()
        {
            return target;
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

    }
}
