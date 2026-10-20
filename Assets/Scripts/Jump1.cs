using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jump1 : MonoBehaviour
{
    [Header("Saut ultra nerveux")]
    public float jumpForce = 7f;             // petite impulsion
    public float gravityScale = 6f;          // gravité élevée en montée ET descente

    [Header("Détection du sol")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        CheckGrounded();
    }

    public void JumpAction()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset vitesse Y
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("🚀 Saut sec & court !");
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = true;
        }
    }
}