using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Stage 1");
    }
}
