using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();    //On récupère l’Animator attaché au joueur
        body = GetComponent<Rigidbody2D>(); //On récupère le Rigidbody2D attaché au joueur
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); //On récupère l’input horizontal (flèches, A/D)

        //On applique la vitesse horizontale au Rigidbody2D du joueur
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        //On change la direction du sprite selon le sens du déplacement
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);  //Regard vers la droite (sprite normal)
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Regard vers la gauche (sprite retourné)
        }

        //On active ou désactive l’animation de course selon que le joueur bouge ou non
        anim.SetBool("run", horizontalInput != 0);
    }

    public void Move(float direction) //Si un autre script veut faire une action qui va affecter le deplacement de mario alors on va créé une fonction de déplacement et ce script pourra acceder au deplacement de mario
    {
    
        body.linearVelocity = new Vector2(direction * speed, body.linearVelocity.y);

        if (direction > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("run", direction != 0);
    }
}
