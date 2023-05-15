using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NifuDev
{
    public class Button : MonoBehaviour
    {

        public void ButtonActivated()
        {
            GameManager.Instance.ButtonPressed();
        }

        public void ButtonReleased()
        {
            GameManager.Instance.ButtonReleased();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Box"))
            {
                ButtonActivated();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Box"))
            {
                ButtonReleased();
            }
        }
    }
}
