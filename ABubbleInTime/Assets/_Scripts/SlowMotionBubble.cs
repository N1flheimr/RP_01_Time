using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class SlowMotionBubble : MonoBehaviour
    {
        [SerializeField] private float slowMotionMult;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.TryGetComponent<ISlowMotionObject>(out ISlowMotionObject slowMotionObject)) {
                slowMotionObject.ActiveSlowMotion(slowMotionMult);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.TryGetComponent<ISlowMotionObject>(out ISlowMotionObject slowMotionObject)) {
                slowMotionObject.StopSlowMotion(slowMotionMult);
            }
        }
    }
}

