using RPG.Core;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Health : MonoBehaviour, ISaveable
    {
        private float health = -1f;
        private bool isDead = false;
        private GameObject instigator;

        private void Start()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            this.instigator = instigator;
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return health;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void RegenerateHealth()
        {
            health = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.Experience));
        }

        public float GetPercentage()
        {
            return 100 * health / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if (health <= 0)
            {
                Die();
            }
        }

        public bool IsDead()
        {
            return isDead;
        }
    }

}