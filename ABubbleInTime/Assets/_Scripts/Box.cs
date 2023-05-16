using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NifuDev
{
    public class Box : MonoBehaviour, ISlowMotionObject
    {

        Rigidbody2D rb;
        private Vector2 velocityBeforeSlowMotion;
        [SerializeField] private float slowMotionGravityScale;
        private float originalGravityScale;
        [SerializeField] private float slowDownTimeMult;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            originalGravityScale = rb.gravityScale;
        }

        private void Update() {
            if (transform.position.y < -25f) {
                Destroy(this.gameObject);
            }
        }

        public void ActiveSlowMotion(float slowDownMult) {
            velocityBeforeSlowMotion = rb.velocity;

            Vector2 boxVelocity = new Vector2(rb.velocity.x, rb.velocity.y);
            boxVelocity *= slowDownMult;

            rb.velocity = boxVelocity * slowDownTimeMult;
            rb.gravityScale *= slowMotionGravityScale;
        }

        public void StopSlowMotion(float slowDownMult) {
            rb.velocity = velocityBeforeSlowMotion;
            rb.gravityScale = originalGravityScale;
        }
    }
}
