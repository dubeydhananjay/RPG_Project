using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using GameDevTV.Utils;
namespace RPG.Stats
{
    public class BaseStat : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel;

        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private ProgressionSO progressionSO;
        [SerializeField] private GameObject levelupPS;
        [SerializeField] private bool shouldUseModifiers;
         public LazyValue<int> currentLevel;

        private Experience experience;
        public event System.Action OnLevelUp;
        private void Awake()
        {
            currentLevel = new LazyValue<int>(CalculateLevel);
            experience = GetComponent<Experience>();

        }

        private void Start() {
            currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (experience) experience.onExperienceGained += UpdateLevel;
        }

        private void OnDisable()
        {
            if (experience) experience.onExperienceGained -= UpdateLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                OnLevelUp?.Invoke();
                Instantiate(levelupPS, transform);
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + (GetPercentageModifiers(stat) / 100));
        }

        private float GetBaseStat(Stat stat)
        {
            return progressionSO.GetStat(stat, characterClass, GetLevel());
        }

        private float GetPercentageModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float sum = 0;
            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();
            foreach (IModifierProvider modifierProvider in modifierProviders)
            {
                foreach (float f in modifierProvider.GetPercentageModifiers(stat))
                {
                    sum += f;
                }
            }

            return sum;
        }

        public int GetLevel()
        {
            
            return currentLevel.value;
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float sum = 0;
            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();
            foreach (IModifierProvider modifierProvider in modifierProviders)
            {
                foreach (float f in modifierProvider.GetAdditiveModifiers(stat))
                {
                    sum += f;
                }
            }

            return sum;
        }

        private int CalculateLevel()
        {
            if (!experience) return startingLevel;

            float currentXP = experience.ExperiencePoints;
            float[] penultimateLevels = progressionSO.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int i = 1; i <= penultimateLevels.Length; i++)
            {
                float xpToLevelUp = progressionSO.GetStat(Stat.ExperienceToLevelUp, characterClass, i);
                if (xpToLevelUp > currentXP)
                    return i;
            }

            return penultimateLevels.Length + 1;
        }

        public object CaptureState()
        {
            return currentLevel;
        }

        public void RestoreState(object state)
        {
            currentLevel.value = (int)state;
        }
    }
}
