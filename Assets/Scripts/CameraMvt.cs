using UnityEngine;

// Ce script permet de déplacer la caméra en suivant le joueur

public class CameraMvt : MonoBehaviour
{
    private Transform player;  //Position du joueur

    private void Awake()
    {
        // On récupère l’objet "Player" dans la scène
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        //LateUpdate = s’exécute après que le joueur ait bougé
        Vector3 cameraPosition = transform.position;

        //On aligne la position X de la caméra avec celle du joueur
        cameraPosition.x = player.position.x;

        //On applique la nouvelle position à la caméra
        transform.position = cameraPosition;
    }
}

