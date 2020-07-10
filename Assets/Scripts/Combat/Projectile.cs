using RPG.Core.Pooling;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour, IPoolableObject
    {
        [SerializeField] float speed = 1f;
        [SerializeField] float range = 25f;
        [SerializeField] bool followTarget;
        [SerializeField] GameObject hitEffect;

        private Health target;
        private GameObject instigator;
        private Vector3 startPos;
        private float damage = 0;

        private void Update()
        {
            if (target == null) return;

            if (followTarget && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            // TODO: also destroy if outside of camera projection volumen
            if (Vector3.Distance(transform.position, startPos) < range)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void SetTarget(GameObject instigator, Health target, float damage)
        {
            this.instigator = instigator;
            this.target = target;
            this.damage = damage;
            transform.LookAt(GetAimLocation());
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target.IsDead()) return;
            if (other.GetComponent<Health>() == target)
            {
                target.TakeDamage(instigator, damage);
                if (hitEffect != null)
                {
                    // TODO: pool & make particle effect
                    Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                }
                gameObject.SetActive(false);
            }
        }

        public void OnObjectActivation()
        {
            startPos = transform.position;
        }
    }
}

