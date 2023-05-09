using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class WaypointFollower : MonoBehaviour,ISlowMotionObject
    {
        [SerializeField] private Transform[] waypointsTransform;
        private int currentWayPointIndex;

        [SerializeField] private float speed;

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

        private void Update() {
            if (Vector2.Distance(waypointsTransform[currentWayPointIndex].position, transform.position) < 0.1f) {
                currentWayPointIndex++;
                if (currentWayPointIndex > waypointsTransform.Length - 1) {
                    currentWayPointIndex = 0;
                }
            }
            else {
                transform.position = Vector2.MoveTowards(transform.position, waypointsTransform[currentWayPointIndex].position, speed * Time.deltaTime);
            }
        }
    }
}

