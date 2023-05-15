using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class ArrowTrap : MonoBehaviour
    {
        [SerializeField] private float attackCooldown;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform arrowPrefab;

        [SerializeField] private bool isFacingRight;
        private float cooldownTimer;

        private void Attack()
        {
            cooldownTimer = 0;

            Transform projectileTransform = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

            if (!isFacingRight)
            {
                Projectile arrowProjectile = projectileTransform.GetComponent<Projectile>();
                arrowProjectile.SetSpeed(-arrowProjectile.GetSpeed());
            }
        }

        private void Update()
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer > attackCooldown)
            {
                Attack();
            }
        }
    }
}

