using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health health;
        private Text healthPercentageText;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthPercentageText = GetComponent<Text>();
        }

        private void Update()
        {

            healthPercentageText.text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}
