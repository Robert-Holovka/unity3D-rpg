using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats.UI
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience experience;
        private Text experienceText;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            experienceText = GetComponent<Text>();
        }

        private void Update()
        {
            experienceText.text = experience.GetPoints().ToString();
        }
    }
}
