using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NifuDev
{
    public class Button : MonoBehaviour
    {
        public UnityEvent OnButtonPressed;
        public UnityEvent OnButtonReleased;

        [SerializeField] private bool isPressed;
        [SerializeField] private int buttonPressedCount = 0;

        public void ButtonActivated()
        {
            if (isPressed == false)
            {
                OnButtonPressed?.Invoke();
            }

            isPressed = true;
            buttonPressedCount++;
        }

        public void ButtonReleased()
        {
            buttonPressedCount--;

            if (buttonPressedCount == 0)
            {
                isPressed = false;
                OnButtonReleased?.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((collision.CompareTag("Player") && collision.isTrigger) || collision.CompareTag("Box"))
            {
                ButtonActivated();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if ((collision.CompareTag("Player") && collision.isTrigger) || collision.CompareTag("Box"))
            {
                if (buttonPressedCount > 0)
                {
                    ButtonReleased();
                }
            }
        }
    }
}
