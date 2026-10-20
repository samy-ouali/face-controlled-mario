using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D body;
    private Animator anim;

    private Jump1 jumpComponent;
    public OpenFaceReader faceReader;

    private bool hasJumpedSinceLastLanding = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        jumpComponent = GetComponent<Jump1>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("run", horizontalInput != 0);

        bool isGrounded = jumpComponent.IsGrounded();

        // Réinitialise l’autorisation de sauter une fois Mario au sol
        if (isGrounded && hasJumpedSinceLastLanding)
        {
            Debug.Log("🟢 Mario a atterri. Nouveau saut possible.");
            hasJumpedSinceLastLanding = false;
        }

        if (faceReader != null)
        {
            Debug.Log($"👄 mouthJustOpened: {faceReader.mouthJustOpened}, grounded: {isGrounded}, hasJumped: {hasJumpedSinceLastLanding}");

            if (faceReader.mouthJustOpened && isGrounded && !hasJumpedSinceLastLanding)
            {
                Debug.Log("🚀 Mario saute !");
                jumpComponent.JumpAction();
                hasJumpedSinceLastLanding = true;
            }
        }
        else
        {
            Debug.LogWarning("⚠️ OpenFaceReader n’est pas assigné !");
        }
    }

    public void Move(float direction)
    {
        body.linearVelocity = new Vector2(direction * speed, body.linearVelocity.y);

        if (direction > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("run", direction != 0);
    }
}
