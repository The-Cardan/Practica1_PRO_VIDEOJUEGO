using UnityEngine;

public class Enemy : MonoBehaviour

{
    //Variables
    public Transform Avatar;
    public float detectionRange = 5f;
    public float speed = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Avatar.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector2 direction = (Avatar.position - transform.position).normalized;
            movement = direction * speed;
        }
        else
        {
            movement = Vector2.zero;
        }

        rb.MovePosition(rb.position + movement *speed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
