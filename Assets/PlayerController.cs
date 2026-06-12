using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 6.5f; // Fuerza del salto
    public float rayLength = 0.9f; // Longitud del rayo para detectar el suelo
    public LayerMask groundLayer; // Capa del suelo para detección

    private Rigidbody2D rb;
    private bool isGrounded;

    // Propiedades personalizadas para el movimiento
    private float localScaleX = 10.64716f;
    private float localScaleY = 8.479184f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(localScaleX, localScaleY, 1);

        else if (moveInput < 0)
            transform.localScale = new Vector3(-localScaleX, localScaleY, 1);

        transform.localRotation = Quaternion.identity;

        isGrounded = IsGrounded();

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            rayLength,
            groundLayer
        );

        UnityEngine.Debug.DrawRay(
            transform.position,
            Vector2.down * rayLength,
            Color.red
        );

        return hit.collider != null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * rayLength
        );
    }
}
