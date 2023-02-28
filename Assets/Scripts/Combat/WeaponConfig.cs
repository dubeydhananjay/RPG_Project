using UnityEngine;
using RPG.RPGResources;
namespace RPG.Combat
{

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private Weapon equippedWeaponPrefab;

        [SerializeField] private AnimatorOverrideController animatorOverrideController;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 10;
        [SerializeField] private float percentageBonus = 0;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile;
        const string weaponName = "weapon";

        public float WeaponRange { get { return weaponRange; } }
        public float WeaponDamage { get { return weaponDamage; } }
        public float PercentageBonus { get { return percentageBonus; } }

        public Weapon Spawn(Transform righthand, Transform lefthand, Animator animator)
        {
            DestroyOldWeapon(righthand, lefthand);
            Weapon weapon = null;
            if (equippedWeaponPrefab)
            {
                weapon = Instantiate(equippedWeaponPrefab, GetHandTransform(righthand, lefthand));
                weapon.gameObject.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverrideController)
                animator.runtimeAnimatorController = animatorOverrideController;
            else if (overrideController)
                animator.runtimeAnimatorController = overrideController;
                return weapon;
        }

        public bool HasProjectile()
        {
            return projectile;
        }

        public void SpawnProjectile(Transform righthand, Transform lefthand, HealthComponent target, GameObject instigator, float damage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(righthand, lefthand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, damage, instigator);
        }

        private Transform GetHandTransform(Transform righthand, Transform lefthand)
        {
            return isRightHanded ? righthand : lefthand;
        }

        private void DestroyOldWeapon(Transform righthand, Transform lefthand)
        {
            Transform oldWeapon = righthand.Find(weaponName);
            if (!oldWeapon) oldWeapon = lefthand.Find(weaponName);
            if (oldWeapon)
            {
                oldWeapon.name = "Destroy";
                Destroy(oldWeapon.gameObject);
            }
        }
    }
}