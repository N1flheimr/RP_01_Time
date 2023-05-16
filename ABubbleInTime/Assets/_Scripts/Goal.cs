using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NifuDev
{
    public class Goal : MonoBehaviour
    {
        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            if (GameManager.Instance.GetButtonLeftToPush() == 0)
            {
                animator.SetBool("IsOpen", true);
            }
            GameManager.Instance.OnButtonPressed.AddListener(GameManager_OnButtonPressed);
            GameManager.Instance.OnButtonReleased.AddListener(GameManager_OnButtonPressed);
        }

        private void GameManager_OnButtonPressed(int buttonLeft)
        {
            if (buttonLeft <= 0)
            {
                animator.SetBool("IsOpen", true);
            }
            else
            {
                animator.SetBool("IsOpen", false);
            }
        }

        private void GameManager_OnButtonReleased(int buttonLeft)
        {
            if (buttonLeft == 0)
            {
                animator.SetBool("IsOpen", true);
            }
            else
            {
                animator.SetBool("IsOpen", false);
            }
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
