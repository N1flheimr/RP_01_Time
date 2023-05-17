using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class Projectile : MonoBehaviour, ISlowMotionObject
    {
        [SerializeField] private float speed;
        private bool isMovingHorizontally;
        private bool isFacingRight;
        private bool isFacingUp;

        private void Update()
        {
            float movementSpeed = speed * Time.deltaTime;

            if (isMovingHorizontally)
            {
                if (isFacingRight)
                {
                    transform.Translate(movementSpeed, 0, 0);
                }
                else
                {
                    transform.Translate(-movementSpeed, 0, 0);
                }
            }
            else
            {
                if (isFacingUp)
                {
                    transform.Translate(0, movementSpeed, 0);
                }
                else
                {
                    transform.Translate(0, -movementSpeed, 0);
                }
            }
        }

        public void ActiveSlowMotion(float slowDownMult)
        {
            float currentSpeed = speed;
            currentSpeed *= slowDownMult;
            speed = currentSpeed;
        }

        public void StopSlowMotion(float slowDownMult)
        {
            float currentSpeed = speed;
            currentSpeed /= slowDownMult;
            speed = currentSpeed;
        }

        public void SetMoveDirection(bool isFacingRight, bool isFacingUp, bool isMovingHorizontally)
        {
            this.isFacingRight = isFacingRight;
            this.isFacingUp = isFacingUp;
            this.isMovingHorizontally = isMovingHorizontally;
        }

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        public float GetSpeed()
        {
            return speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !isMovingHorizontally)
            {
                collision.GetComponent<Transform>().parent = this.transform;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!isMovingHorizontally && collision.CompareTag("Player"))
            {
                collision.GetComponent<Transform>().parent = null;
            }
        }
    }
}

