using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 6.5f;
    public float rayLength = 0.7f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    public bool isGrounded;
    private Animator animator;
    private bool facingRight = true;

    public TextMeshProUGUI objectCounterText;

    private Dictionary<string, int> collectedObjects = new Dictionary<string, int>()
    {
        { "Cake", 0 },
        { "Chicken", 0 },
        { "Coffee", 0 },
        { "Jam", 0 },
        { "Cookie", 0 },
        { "Gem", 0 }
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateObjectCounterUI();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        isGrounded = IsGrounded();

        animator.SetBool("IsJumping", !isGrounded);
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();

        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
        transform.localRotation = Quaternion.identity;

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);
        UnityEngine.Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.red);

        return hit.collider != null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectible"))
        {
            string objectName = collision.gameObject.name;

            if (collectedObjects.ContainsKey(objectName))
            {
                collectedObjects[objectName]++;
                UpdateObjectCounterUI();
            }
            Destroy(collision.gameObject);
        }
    }

    void UpdateObjectCounterUI()
    {
        objectCounterText.text = $"Cake: {collectedObjects["Cake"]} | " +
                                 $"Chicken: {collectedObjects["Chicken"]} | " +
                                 $"Coffee: {collectedObjects["Coffee"]} | " +
                                 $"Jam: {collectedObjects["Jam"]} | " +
                                 $"Cookie: {collectedObjects["Cookie"]} | " +
                                 $"Gem: {collectedObjects["Gem"]}";

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("¡Has muerto!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDrawGizmos()
    { 
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    }

}
