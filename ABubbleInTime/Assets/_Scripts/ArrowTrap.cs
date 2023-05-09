using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class ArrowTrap : MonoBehaviour
    {
        [SerializeField] private float attackCooldown;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform arrowPrefab;
        private float cooldownTimer;

        private void Attack() {
            cooldownTimer = 0;

            Transform projectileTransform = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        }

        private void Update() {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer > attackCooldown) {
                Attack();
            }
        }
    }
}

