using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   [SerializeField] private GameObject pauseMenuUI;

   public void Pause()
   {
      pauseMenuUI.SetActive(true);
      GameManager.Instance.pauseGame();
   }

   public void Resume()
   {
      pauseMenuUI.SetActive(false);
      GameManager.Instance.resumeGame();
   }

   public void LoadMenu()
   {
      GameManager.Instance.resumeGame();
      GameManager.Instance.GoToMainMenu();
   }
}
