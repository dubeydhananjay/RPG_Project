using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using System.Collections;
using RPG.Saving;
using RPG.RPGResources;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IModifierProvider
    {
        private Mover mover;

        [SerializeField] private float timeBetweenAttack = 1;
        [SerializeField] private HealthComponent target;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private WeaponConfig defaultWeapon;
        private BaseStat baseStat;

        private WeaponConfig currentWeaponConfig;
       public LazyValue<Weapon> currentWeapon;

        private float timeSinceLastAttack = Mathf.Infinity;
        private Animator animator;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            baseStat = GetComponent<BaseStat>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetDefaultWeapon);

        }

        private void Start()
        {
            AttachWeapon(currentWeaponConfig);
            currentWeapon.ForceInit();
        }

        private Weapon SetDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);

        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target)
            {
                if (target.AlreadyDied) return;
                if (IsInRange(target.transform))
                {
                    mover.CancelAction();
                    AttackBehaviour();
                }
                else
                    mover.MoveTo(target.transform.position);

            }
        }

        private bool IsInRange(Transform targetTransform)
        {
            return (transform.position - targetTransform.position).sqrMagnitude <= Mathf.Pow(currentWeaponConfig.WeaponRange, 2);
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<HealthComponent>();
        }
        public void CancelAction()
        {
            TriggerAnimation("stopAttack", "attack");
            target = null;
            mover.CancelAction();
        }


        //Animation Event
        private void Hit()
        {
            if (target)
            {
                float damage = baseStat.GetStat(Stat.Damage);
                if (currentWeaponConfig.HasProjectile()) currentWeaponConfig.SpawnProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
                else target.TakeDamage(gameObject, damage);
            }
        }

        private void Shoot()
        {
            Hit();
        }

        //Need to uncheck "Has exit time" from locomotive to Attack transition. Then it will not wait the whole locomotive animation.
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttack)
            {
                TriggerAnimation("attack", "stopAttack");
                timeSinceLastAttack = 0;
            }
        }


        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.PercentageBonus;
            }
        }

        public void TriggerAnimation(string triggerName, string resetTriggerName = null)
        {
            if (resetTriggerName != null) animator.ResetTrigger(resetTriggerName);
            animator.SetTrigger(triggerName);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (!mover.CanMoveTo(combatTarget.transform.position) &&
                !IsInRange(combatTarget.transform))
            {
                return false;
            }
            HealthComponent testTarget = combatTarget.GetComponent<HealthComponent>();
            return (testTarget && !testTarget.AlreadyDied);
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(currentWeaponConfig);
        }

        public Weapon AttachWeapon(WeaponConfig weapon)
        {
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public HealthComponent GetTarget()
        {
            return target;
        }
        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

    }
}