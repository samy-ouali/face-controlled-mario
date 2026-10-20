using UnityEngine;

public class Flag : MonoBehaviour
{
    public Animator flagAnimator;          //L’Animator qui gère l’animation du drapeau qui descend
    public Transform mario;                //La position de Mario
    public Transform flagStopPoint;       //Le point où Mario doit s’arrêter en descendant le drapeau
    public Transform castleEntryPoint;    //Le point d’entrée dans le château (fin de niveau)
    private bool reachedFlag = false;     //Booléen pour savoir si Mario a déjà touché le drapeau


    private void OnTriggerEnter2D(Collider2D other)
    {
        //Quand Mario entre en contact avec le drapeau
        if (other.CompareTag("Player") && !reachedFlag)
        {
            reachedFlag = true;                    // On marque que Mario a touché le drapeau
            flagAnimator.SetTrigger("LowerFlag"); // On lance l’animation de descente du drapeau
            StartCoroutine(MarioSlideDown());     // On lance la coroutine qui gère Mario qui descend le drapeau
        }
    }

    private System.Collections.IEnumerator MarioSlideDown()
    {
        //On récupère le script de mouvement de Mario pour le désactiver
        PlayerMovement marioMovement = mario.GetComponent<PlayerMovement>();
        marioMovement.enabled = false; // On désactive le mouvement de Mario

        Rigidbody2D rb = mario.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero; //On stoppe toute vélocité
        rb.gravityScale = 0;              //On désactive la gravité pour pas que Mario tombe

        //Tant que Mario n’a pas atteint le point où il doit s’arrêter sur le drapeau
        while (mario.position.y > flagStopPoint.position.y)
        {
            mario.position += Vector3.down * 2f * Time.deltaTime; //On descend Mario
            yield return null; //On attend la prochaine frame
        }

        yield return new WaitForSeconds(0.5f); //Petite pause après la descente

        StartCoroutine(MarioWalkToCastle()); //Ensuite, on lance la marche vers le château
    }

    private System.Collections.IEnumerator MarioWalkToCastle()
    {
        //On fait face au château (Mario regarde vers la droite)
        mario.localScale = new Vector3(1, 1, 1);

        //Mario avance jusqu’au point d’entrée du château
        while (mario.position.x < castleEntryPoint.position.x)
        {
            mario.position += Vector3.right * 3f * Time.deltaTime; //Mario avance vers le château
            yield return null; //On attend la prochaine frame
        }

        //Une fois dans le château Mario disparaît
        mario.gameObject.SetActive(false);

        //On récupère le GameManager et on signale la fin du niveau
        GameManager gm = FindFirstObjectByType<GameManager>();
        gm.LevelComplete();  //On appel fonction fin de niveau
    }
    
    
}
