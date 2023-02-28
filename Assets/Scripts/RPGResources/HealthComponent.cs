using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.RPGResources
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private float regenerationVal = 70;
        [SerializeField] private TakeDamageEvent takeDamage;
        [SerializeField] private UnityEvent OnDie;
        public LazyValue<float> health;
        private float maxHealth;

        private bool alreadyDied;
        public bool AlreadyDied { get { return alreadyDied; } }
        public float Health { get { return health.value; } }
        public float MaxHealth { get { return maxHealth; } }
        private BaseStat baseStat;
        public System.Action UpdateHealthBar;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }
        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
            maxHealth = health.value;
            baseStat = GetComponent<BaseStat>();
        }

        private void Start()
        {
            health.ForceInit();
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStat>().GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            baseStat.OnLevelUp += SetHealth;
        }

        private void OnDisable()
        {
            baseStat.OnLevelUp -= SetHealth;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health.value = Mathf.Max((health.value - damage), 0);
            takeDamage.Invoke(damage);
            UpdateHealthBar?.Invoke();
            if (health.value == 0 && !alreadyDied)
            {
                OnDie?.Invoke();
                Die();
                SetExperiencePoint(instigator);
            }
        }

        private void SetHealth()
        {
            maxHealth = GetComponent<BaseStat>().GetStat(Stat.Health);
            float regenHealth = maxHealth * (regenerationVal / 100);
            health.value = Mathf.Max(health.value, regenerationVal);
        }

        public float GetHealthPercentage()
        {
            return GetFraction() * 100;
        }

        public float GetFraction()
        {
            return (health.value / maxHealth);
        }

        private void Die()
        {
            alreadyDied = true;

            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void SetExperiencePoint(GameObject instigator)
        {
            if (instigator)
            {
                BaseStat baseStat = GetComponent<BaseStat>();
                Experience experience = instigator.GetComponent<Experience>();

                if (experience && baseStat)
                    experience.GainExperience(baseStat.GetStat(Stat.ExperienceReward));
            }
        }

        public void Heal(float healthRestore)
        {
            health.value = Mathf.Min(health.value + healthRestore, maxHealth);
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;
            TakeDamage(null, 0);
        }
    }
}