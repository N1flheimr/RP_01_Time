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

        public UnityEvent OnDoorOpen;
        public UnityEvent OnDoorClose;

        public static GameManager Instance;
        [SerializeField] private string sceneName;

        [SerializeField] private List<Button> buttonList;

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
            GameObject[] buttonGameObjectArray = GameObject.FindGameObjectsWithTag("Button");

            for (int i = 0; i < buttonGameObjectArray.Length; i++)
            {
                buttonList.Add(buttonGameObjectArray[i].GetComponent<Button>());
            }

            foreach (Button button in buttonList)
            {
                button.OnButtonPressed.AddListener(Button_OnButtonPressed);
                button.OnButtonReleased.AddListener(Button_OnButtonReleased);
            }

            buttonLeftToPush = buttonList.Count;
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

        private void Button_OnButtonPressed()
        {
            buttonLeftToPush--;
            if (buttonLeftToPush <= 0)
            {
                OnDoorOpen?.Invoke();
            }
        }

        private void Button_OnButtonReleased()
        {
            buttonLeftToPush++;
            buttonLeftToPush = Mathf.Min(buttonLeftToPush, totalButton);
            if (buttonLeftToPush > 0)
            {
                OnDoorClose?.Invoke();
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
            OnDoorOpen?.Invoke();
        }

        public void ButtonReleased()
        {
            buttonLeftToPush++;
            buttonLeftToPush = Mathf.Min(buttonLeftToPush, totalButton);
            OnDoorClose?.Invoke();
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