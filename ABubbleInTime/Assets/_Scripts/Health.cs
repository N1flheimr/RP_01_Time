using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class Health : MonoBehaviour
    {
        public void TakeDamage() {
            Death();
        }

        public void Death() {
            GameManager.Instance.RestartScene();
        }
    }
}

