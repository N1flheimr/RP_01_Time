using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class EnemyDamage : MonoBehaviour
    {
        protected void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player") && collision.isTrigger) {
                collision.GetComponent<Health>().TakeDamage();
            }
        }
    }
}

