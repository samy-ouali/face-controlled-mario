using UnityEngine;

public class MarioDeath : MonoBehaviour
{
    private GameManager gameManager;   //Référence au GameManager pour gérer la fin de jeu
    public float fallThreshold = -10f; //Hauteur limite sous laquelle Mario meurt (tombe dans le vide)

    void Start()
    {
        //On récupère le GameManager dans la scène au démarrage
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        //Si Mario tombe en dessous de la limite définie, il meurt
        if (transform.position.y < fallThreshold)
        {
            Die();
        }
    }

    public void Die()
    {
        //On appelle la fonction GameOver dans le GameManager qui gère l'affichage de l'écran quand Mario meurt
        gameManager.GameOver();
        //On fait disparaître Mario
        gameObject.SetActive(false);
    }
}
