using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "ProgressionSO", menuName = "Stat/ProgressionSO", order = 0)]
    public class ProgressionSO : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] progressionCharacterClasses;
        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();

            float statLevel = 0;
            var levels = lookupTable[characterClass][stat];

            statLevel = levels[Mathf.Clamp(level - 1, 0, levels.Length - 1)];
            
            return statLevel;
        }

        public float[] GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookUp();
            float[] levels = lookupTable[characterClass][stat];
            return levels;
        }

        private void BuildLookUp()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            for (int i = 0; i < progressionCharacterClasses.Length; i++)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();
                ProgressionCharacterClass progressionCharacterClass = progressionCharacterClasses[i];
                foreach (ProgressionStat ps in progressionCharacterClass.progressionStats)
                    statLookUpTable[ps.stat] = ps.levels;

                lookupTable[progressionCharacterClass.characterClass] = statLookUpTable;
            }

        }
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] progressionStats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}

