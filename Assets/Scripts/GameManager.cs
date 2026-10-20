using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI; //Interface à afficher quand le joueur perd
    public GameObject levelCompleteUI;  //Interface à afficher quand le niveau est terminé

    private void Start()
    {
        gameOverUI.SetActive(false); //On cache l'UI de Game Over au début
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false); //Pareil pour l'UI de fin de niveau

        Time.timeScale = 1f; //On remet le temps à la normale (pas de pause)
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true); //On affiche l'écran de Game Over
        Time.timeScale = 0f; //On met le jeu en pause 
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; //On remet le temps à la normale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //On recharge la scène actuelle
    }

    public void LevelComplete()
    {
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(true);  //On affiche l'écran de fin de niveau
        Time.timeScale = 0f; //On met le jeu en pause 
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f; //On remet le temps à la normale
        int currentIndex = SceneManager.GetActiveScene().buildIndex; //On récupère l'index de la scène actuelle
        if (currentIndex + 1 < SceneManager.sceneCountInBuildSettings)  
        {
            SceneManager.LoadScene(currentIndex + 1); //On charge la scène suivante si elle existe
        }
        else
        {
            Debug.Log("Dernier niveau atteint !");  //Sinon on prévient que c'est la fin
        }
    }
}
