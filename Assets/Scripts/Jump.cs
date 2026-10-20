using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody2D rb;                   
    public float jump;                       
    private bool grounded;                   
    public float groundCheckDistance = 0.2f;  
    public LayerMask groundLayer;   //La couche du sol qui sera utilisé pour permettre à Mario de sauter

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Récupère le Rigidbody2D attaché à Mario
        grounded = true;                   // Au début, Mario est au sol
    }

    void Update()
    {
        CheckGrounded(); //On vérifie à chaque frame si Mario est au sol

        //Si le joueur appuie sur saut ET que Mario est au sol, il saute
        if (Input.GetButtonDown("Jump") && grounded)
        {
            Jumping();
        }
    }

    private void Jumping()
    {
        //On fait sauter Mario
        rb.AddForce(new Vector2(rb.linearVelocity.x, jump), ForceMode2D.Impulse);
        grounded = false;  // Mario n’est plus au sol après le saut ( permet de ne pas sauter en l'air)
    }

    private void CheckGrounded()
    {
        //Trois raycasts vers le bas, à gauche, au centre, à droite sous Mario qui permet de mieux detecter le sol (surtout pour les plateformes étroites si Mario est au bord d'un bloc il peut sauter)
        bool isGroundedLeft = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y), Vector2.down, groundCheckDistance, groundLayer);
        bool isGroundedRight = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.down, groundCheckDistance, groundLayer);
        bool isGroundedCenter = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, groundCheckDistance, groundLayer);

        //Si un des raycasts touche le sol, Mario est au sol
        grounded = isGroundedLeft || isGroundedRight || isGroundedCenter;
    }

    public void TryJump()
    {
        if (grounded)
        {
            Jumping();
        }
    }
}
