using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class Projectile : EnemyDamage,ISlowMotionObject
    {
        [SerializeField] private BoxCollider2D triggerCollider;
        [SerializeField] private float speed;


        private void Update() {
            float movementSpeed = speed * Time.deltaTime;
            transform.Translate(movementSpeed, 0, 0);
        }
        public void ActiveSlowMotion(float slowDownMult) {
            float currentSpeed = speed;
            currentSpeed *= slowDownMult;
            speed = currentSpeed;
        }

        public void StopSlowMotion(float slowDownMult) {
            float currentSpeed = speed;
            currentSpeed /= slowDownMult;
            speed = currentSpeed;
        }

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        public float GetSpeed()
        {
            return speed;
        }

        private new void OnTriggerEnter2D(Collider2D collision) {
            base.OnTriggerEnter2D(collision);

            if (!collision.CompareTag("SlowMotionBubble") && collision.isTrigger && collision.CompareTag("Player")) {
                Destroy(this.gameObject);
            }
        }
    }
}

