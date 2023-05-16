using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NifuDev
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        public UnityEvent<int> OnButtonPressed;
        public UnityEvent<int> OnButtonReleased;

        public static GameManager Instance;
        [SerializeField] private string sceneName;

        [SerializeField] private GameObject[] buttonArray;
        [SerializeField] private int buttonLeftToPush;
        private int totalButton;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("Having 2 GameManager");
            }
        }

        private void Start()
        {
            buttonArray = GameObject.FindGameObjectsWithTag("Button");
            buttonLeftToPush = buttonArray.Length;
            totalButton = buttonLeftToPush;
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartScene();
            }
            if (player.transform.position.y < -10f)
            {
                RestartScene();
            }
        }

        public void LoadNextStage()
        {
            if (buttonLeftToPush <= 0)
                SceneManager.LoadScene(sceneName);
        }

        public void ButtonPressed()
        {
            buttonLeftToPush--;
            OnButtonPressed?.Invoke(buttonLeftToPush);
        }

        public void ButtonReleased()
        {
            buttonLeftToPush++;
            buttonLeftToPush = Mathf.Min(buttonLeftToPush, totalButton);
            OnButtonReleased?.Invoke(buttonLeftToPush);
        }

        public int GetButtonLeftToPush()
        {
            return buttonLeftToPush;
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}