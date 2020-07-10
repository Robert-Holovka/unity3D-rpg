using RPG.Core.Pooling;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        private enum WeaponType
        {
            LEFT_HANDED,
            RIGHT_HANDED
        }

        [SerializeField] AnimatorOverrideController attackAnimatorOverride;
        [SerializeField] GameObject weaponPrefab;
        [Header("Weapon Stats")]
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] WeaponType weaponType;
        [SerializeField] Projectile projectilePrefab;

        const string weaponName = "Weapon";
        private ObjectPooler objectPooler;
        private GameObject instigator;

        public void Spawn(GameObject instigator, Transform rightHand, Transform leftHand, Animator animator)
        {
            this.instigator = instigator;
            DestroyOldWeapon(rightHand, leftHand);
            if (weaponPrefab != null)
            {
                GameObject weapon = Instantiate(weaponPrefab, GetCorrectHandTransform(rightHand, leftHand));
                weapon.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (attackAnimatorOverride != null)
            {
                animator.runtimeAnimatorController = attackAnimatorOverride;
            }
            else if (overrideController != null)
            {
                // Reset to default controller
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName) ?? leftHand.Find(weaponName);
            if (oldWeapon != null)
            {
                oldWeapon.name = "DESTROYING";
                Destroy(oldWeapon.gameObject);
            }
        }

        private Transform GetCorrectHandTransform(Transform rightHand, Transform leftHand)
        {
            return (weaponType == WeaponType.RIGHT_HANDED) ? rightHand : leftHand;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, float calculatedDamage)
        {
            if (objectPooler == null)
            {
                objectPooler = FindObjectOfType<ObjectPooler>();
            }
            Projectile projectileInstance = objectPooler.SpawnObject(projectilePrefab, GetCorrectHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(instigator, target, calculatedDamage);
        }

        public bool HasProjectile()
        {
            return projectilePrefab != null;
        }
    }
}
