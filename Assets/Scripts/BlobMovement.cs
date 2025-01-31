using UnityEngine;

public class BlobMovement : MonoBehaviour
{
    public float maxForce = 10f; // Maximale kracht die de blob kan opbouwen
    public float chargeRate = 20f; // Hoe snel de kracht oploopt
    private Rigidbody2D rb; // Referentie naar de Rigidbody2D van de blob
    private Vector2 targetPosition; // Doelpositie waar de muis zich bevindt
    private float currentForce = 0f; // Huidige kracht die wordt opgebouwd

    void Start()
    {
        // Haal de Rigidbody2D-component op
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Converteer de muispositie van scherm- naar wereldcoördinaten
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition = new Vector2(mousePosition.x, mousePosition.y);

        // Controleer of de linker muisknop wordt ingedrukt
        if (Input.GetMouseButton(0))
        {
            // Bouw de kracht op
            currentForce += chargeRate * Time.deltaTime;
            currentForce = Mathf.Clamp(currentForce, 0f, maxForce); // Begrens de kracht
        }

        // Controleer of de linker muisknop is losgelaten
        if (Input.GetMouseButtonUp(0))
        {
            // Bereken de richting naar het doel
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

            // Pas de kracht toe in de richting van het doel
            rb.AddForce(direction * currentForce, ForceMode2D.Impulse);

            // Reset de kracht
            currentForce = 0f;
        }
    }

    void OnDrawGizmos()
    {
        // Teken een lijn naar de muiscursor voor debugging
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}
