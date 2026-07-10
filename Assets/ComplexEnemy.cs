using UnityEngine;
using UnityEngine.SceneManagement;

public class ComplexEnemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float speed = 2f;

    private bool facingRight = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Busca automáticamente al jugador si no fue asignado
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
            else
                Debug.LogError("No se encontró un objeto con la etiqueta 'Player'.");
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector2 targetPosition = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime);

            transform.position = targetPosition;

            // Girar hacia el jugador
            if (player.position.x > transform.position.x && !facingRight)
                Flip();
            else if (player.position.x < transform.position.x && facingRight)
                Flip();

            if (animator != null)
                animator.SetBool("isWalking", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("isWalking", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("GAME OVER");

            // Reinicia la escena
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}