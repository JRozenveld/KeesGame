using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float speed = 2f;  // Speed of movement
    public float maxDistance = 5f; // How far it moves before flipping
    public float waveHeight = 1f; // How high the wave motion goes
    public float waveSpeed = 2f;  // Speed of the wave motion

    private Vector3 startPosition;
    private int direction = -1; // Start by moving LEFT
    private SpriteRenderer spriteRenderer;
    private float timeCounter = 0f; // Tracks time for wave motion

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Do NOT force a flip at the start. Keep the original sprite orientation.
    }

    void Update()
    {
        timeCounter += Time.deltaTime * waveSpeed;

        // Move left and right
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        // Add sine wave motion (up and down)
        float verticalMovement = startPosition.y + Mathf.Sin(timeCounter) * waveHeight;
        transform.position = new Vector3(transform.position.x, verticalMovement, transform.position.z);

        // Check if we need to flip direction
        if (Mathf.Abs(transform.position.x - startPosition.x) >= maxDistance)
        {
            direction *= -1; // Reverse direction
            Flip();
        }
    }

    void Flip()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX; // Flip the sprite
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}