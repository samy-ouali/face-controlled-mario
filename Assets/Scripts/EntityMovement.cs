using UnityEditor;
using UnityEngine;

//On va créer un script pour gérer le mouvement des goombas

[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
    public float speed = 1f;  
    public Vector2 direction = Vector2.left;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    public float raycastDistance = 0.5f;
    public float stompBounce = 8f;
    
    private Rigidbody2D rb;
    private Vector2 velocity;
    private bool isDead = false;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enabled = false;  //Le script est inactif tant que le goomba n'est pas visible (les goombas loins ne bougeront pas ce qui sert a quand les goombas loin  sont proche du vide il ne tombe pas tant que nous ne les voyons pas)
    }
    
    private void OnBecameVisible()
    {
        //Le script s’active quand l’objet devient visible à la caméra
        #if UNITY_EDITOR
        enabled = !EditorApplication.isPaused;
        #else
        enabled = true;
        #endif
    }
    
    private void OnBecameInvisible()
    {
        enabled = false;  //On désactive quand on sort du champ de vision
    }
    
    private void OnEnable()
    {
        if (!isDead) rb.WakeUp();  //On réactive le Rigidbody si l'entité n'est pas morte
    }
    
    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.Sleep();  //On stop le mouvement du goomba et on "l'endort"
        }
    }
    
    private void FixedUpdate()
    {
        if (isDead) return;

        //Détection d'obstacle devant
        Vector2 raycastOrigin = rb.position;
        if (GetComponent<CircleCollider2D>() != null)
        {
            raycastOrigin.x += direction.x * GetComponent<CircleCollider2D>().radius;
        }

        RaycastHit2D frontHit = Physics2D.Raycast(raycastOrigin, direction, raycastDistance, obstacleLayer);
        Debug.DrawRay(raycastOrigin, direction * raycastDistance, frontHit.collider != null ? Color.red : Color.yellow);
        
        if (frontHit.collider != null)
        {
            Debug.Log("Obstacle détecté: " + frontHit.collider.name);
            direction = -direction;  // Demi-tour si obstacle détecté
        }

        //On applique la vitesse et la gravité
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        //Raycast vers le bas pour voir si on touche le sol
        RaycastHit2D downHit = Physics2D.Raycast(rb.position, Vector2.down, 0.5f, groundLayer);
        Debug.DrawRay(rb.position, Vector2.down * 0.5f, downHit.collider != null ? Color.green : Color.red);
        
        if (downHit.collider != null)
        {
            velocity.y = Mathf.Max(velocity.y, 0f);  //On empeche l'enfoncement dans le sol
        }

        //On applique le deplacement
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        //On gère la rotation du goomba
        transform.localEulerAngles = direction.x > 0f ? new Vector3(0f, 180f, 0f) : Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.collider.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                //Si le joueur arrive bien du dessus
                if (contact.normal.y < -0.5f)
                {
                    //le joueur rebondit
                    Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        Vector2 v = playerRb.linearVelocity;
                        v.y = stompBounce;
                        playerRb.linearVelocity = v;
                    }

                    Die();// Le goomba meurt
                    return;
                }
            }

            // Si le joueur ne vient pas du dessus : il meurt
            collision.collider.GetComponent<MarioDeath>()?.Die();
        }
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.Sleep();
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = Color.gray; // On change la couleur pour indiquer la mort
        Destroy(gameObject, 0.5f);  //Le goomba disparait après 0.5s
    }

    private void OnDrawGizmosSelected()
    {
        //On dessine les raycasts pour visualiser la détection d'obstacles et le sol ce qui permet de definir la portée pour laquel les goombas peuvent détecter les obstacles et le sol
        Gizmos.color = Color.red;
        Vector2 raycastOrigin = transform.position;
        if (GetComponent<CircleCollider2D>() != null)
        {
            raycastOrigin.x += direction.x * GetComponent<CircleCollider2D>().radius;
        }
        Gizmos.DrawLine(raycastOrigin, raycastOrigin + (Vector2)(direction.normalized * raycastDistance));
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.5f);
    }
}
