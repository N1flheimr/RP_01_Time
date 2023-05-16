using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class ArrowTrap : MonoBehaviour
    {
        [SerializeField] private float attackCooldown;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform arrowPrefab;

        [SerializeField] private bool isHorizontal;

        [SerializeField] private bool isFacingRight;
        [SerializeField] private bool isFacingUp;
        private float cooldownTimer;

        [SerializeField] private int arrowTrapAmmo;
        [SerializeField] private bool hasInfiniteAmmo;


        private void Update() {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer > attackCooldown) {
                Attack();
            }
        }
        private void Attack() {
            cooldownTimer = 0;

            if (hasInfiniteAmmo || arrowTrapAmmo > 0) {
                Transform projectileTransform = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

                Projectile arrowProjectile = projectileTransform.GetComponent<Projectile>();
                arrowProjectile.SetMoveDirection(isFacingRight, isFacingUp, isHorizontal);

                arrowTrapAmmo--;
            }
            else {
                //No ammo left
            }
        }
    }
}

