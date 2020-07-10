using RPG.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats.UI
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Text healthPercentageText;
        private Fighter player;
        private string defaultValue = "N/A";

        private void Awake()
        {
            healthPercentageText = GetComponent<Text>();
            player = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Health target = player.GetTarget();
            if (target == null)
            {
                healthPercentageText.text = defaultValue;
            }
            else
            {
                healthPercentageText.text = string.Format("{0:0}/{1:0}", target.GetHealthPoints(), target.GetMaxHealthPoints());
            }
        }

    }
}
