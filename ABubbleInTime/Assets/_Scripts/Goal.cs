using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NifuDev
{
    public class Goal : MonoBehaviour
    {
        Animator animator;
        [SerializeField] private GameObject doorOpenParticleSystem;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            if (GameManager.Instance.GetButtonLeftToPush() == 0)
            {
                animator.SetBool("IsOpen", true);

                doorOpenParticleSystem.SetActive(true);
            }
            else
            {

                doorOpenParticleSystem.SetActive(false);
            }
            GameManager.Instance.OnDoorOpen.AddListener(GameManager_OnDoorOpen);
            GameManager.Instance.OnDoorClose.AddListener(GameManager_OnDoorClose);
        }

        private void GameManager_OnDoorOpen()
        {
            animator.SetBool("IsOpen", true);
            doorOpenParticleSystem.SetActive(true);
        }

        private void GameManager_OnDoorClose()
        {
            animator.SetBool("IsOpen", false);
            doorOpenParticleSystem.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance.LoadNextStage();
            }
        }
    }
}
