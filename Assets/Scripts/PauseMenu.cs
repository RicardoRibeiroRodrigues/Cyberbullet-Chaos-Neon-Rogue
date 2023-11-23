using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   [SerializeField] private GameObject pauseMenuUI;

   public void Pause()
   {
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0f;
   }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadMenu()
    {
       Time.timeScale = 1f;
       SceneManager.LoadScene("MainMenuScene");
    }
}
