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
    private bool hasFlipped = false; // Keeps track of flip status

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (Mathf.Abs(transform.position.x - startPosition.x) >= maxDistance && !hasFlipped)
        {
            direction *= -1; // Reverse direction
            Flip();
            hasFlipped = true; // Mark as flipped, so it doesn't flip again too soon
        }

        // Reset flip status if we pass the threshold (to allow flip again)
        if (Mathf.Abs(transform.position.x - startPosition.x) < maxDistance)
        {
            hasFlipped = false;
        }
    }

    void Flip()
    {
        // Flip the object by changing scale
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}