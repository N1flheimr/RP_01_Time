using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class SpikeTrap : EnemyDamage
    {
        private new void OnTriggerEnter2D(Collider2D collision) {
            base.OnTriggerEnter2D(collision);
        }
    }
}

