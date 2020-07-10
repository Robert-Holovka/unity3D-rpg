using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats.UI
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats baseStats;
        private Text levelText;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            levelText = GetComponent<Text>();
        }

        private void Update()
        {
            levelText.text = baseStats.GetLevel().ToString();
        }
    }
}
