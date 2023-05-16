using System.Collections;
using UnityEngine;

namespace NifuDev
{
    public class BoxTube : MonoBehaviour
    {
        [SerializeField] private Transform boxSpawnTransform;
        [SerializeField] private GameObject boxPrefab;

        [SerializeField] private float spawnCooldown;
        private float cooldownTimer;

        [SerializeField] private Box currentBox;

        [SerializeField] private bool hasInfiniteBox;
        [SerializeField] private int remainingBox;

        private void Update() {
            if (currentBox == null && hasInfiniteBox || remainingBox > 0) {
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer > spawnCooldown) {
                    Spawn();
                }
            }
        }

        private void Spawn() {
            cooldownTimer = 0;

            if (hasInfiniteBox || remainingBox > 0) {
                currentBox = Instantiate(boxPrefab, boxSpawnTransform.position, Quaternion.identity).GetComponent<Box>();

                if (!hasInfiniteBox) {
                    remainingBox--;
                }
            }
        }
    }
}

