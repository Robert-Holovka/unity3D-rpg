using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            if (lookupTable == null)
            {
                BuildLookupTable();
            }

            float[] levels = lookupTable[characterClass][stat];
            if (levels.Length < level)
            {
                return 0;
            }
            return levels[level - 1];
        }

        public int GetMaxLevel(Stat stat, CharacterClass characterClass)
        {
            BuildLookupTable();

            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookupTable()
        {
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                Dictionary<Stat, float[]> statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionClass.characterClass] = statLookupTable;

            }
        }
    }
}