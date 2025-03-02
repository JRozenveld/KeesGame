using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNode : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isStuck = false;
    private Transform stickySurface;

    // Dit is een statische teller per stickySurface-object
    public static int maxNodes = 2; // Maximaal aantal nodes dat aan een surface kan vastplakken
    private static Dictionary<Transform, int> stickySurfaceCounters = new Dictionary<Transform, int>();

    // Cooldown voor het opnieuw vastplakken
    public float stickCooldown = 1f; // Stel de cooldown in (bijv. 1 seconde)
    private float stickCooldownTimer = 0f;

    public static Dictionary<GameObject, Vector3> originalRelativePositions = new Dictionary<GameObject, Vector3>();
    private static Vector3 originalCenter;
    private static bool initialized = false;

    public float checkInterval = 1f; // Hoe vaak de vorm gecontroleerd wordt (in seconden)
    public float maxDeviation = 0.5f; // Maximale toegestane afwijking
    public float resetDuration = 1f; // Duur van de reset (optioneel, kan worden gebruikt voor meer controle)


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Bereken en sla de originele vorm op (maar doe dit maar één keer)
        if (!initialized)
        {
            initialized = true;
            StoreOriginalShape();
        }

        // Start de periodieke controle
        InvokeRepeating(nameof(CheckShapeIntegrity), 1f, checkInterval);
    }

    void StoreOriginalShape()
    {
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("Node");

        // Bereken het originele middelpunt van de cirkel
        Vector3 sum = Vector3.zero;
        foreach (GameObject node in allNodes)
        {
            sum += node.transform.position;
        }
        originalCenter = sum / allNodes.Length;

        // Sla de relatieve posities ten opzichte van het originele middelpunt op
        foreach (GameObject node in allNodes)
        {
            originalRelativePositions[node] = node.transform.position - originalCenter;
        }
    }


    void CheckShapeIntegrity()
    {
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("Node");

        // Controleer of minstens één node nog vastzit aan iets
        foreach (GameObject node in allNodes)
        {
            StickyNode stickyNode = node.GetComponent<StickyNode>();
            if (stickyNode.isStuck)
            {
                return; // Stop hier! De vorm wordt NIET hersteld als een node vastzit.
            }
        }

        // Als geen enkele node vastzit, check of de cirkel vervormd is
        Vector3 currentCenter = Vector3.zero;
        foreach (GameObject node in allNodes)
        {
            currentCenter += node.transform.position;
        }
        currentCenter /= allNodes.Length;

        foreach (GameObject node in allNodes)
        {
            Vector3 expectedPosition = currentCenter + originalRelativePositions[node];
            float distance = Vector3.Distance(node.transform.position, expectedPosition);

            if (distance > maxDeviation)
            {
                StartCoroutine(ResetShape(currentCenter));
                break;
            }
        }
    }


    public float smoothSpeed = 2f;  // Snelheid van de overgang (hoe hoger, hoe sneller de overgang)

    IEnumerator ResetShape(Vector3 currentCenter)
    {
        float elapsedTime = 0f;

        // Herstel de originele posities van de nodes, maar nu met een smooth overgang
        while (elapsedTime < resetDuration)
        {
            // Bereken de voortgang van de overgang, maar gebruik smoothSpeed voor een meer flexibele snelheid
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / resetDuration * smoothSpeed);

            // Pas de position van elke node aan met Lerp (interpolatie)
            foreach (GameObject node in GameObject.FindGameObjectsWithTag("Node"))
            {
                Vector3 originalPosition = currentCenter + originalRelativePositions[node];
                node.transform.position = Vector3.Lerp(node.transform.position, originalPosition, t);
            }

            // Wacht op de volgende frame
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Zorg ervoor dat de uiteindelijke positie exact de originele is
        foreach (GameObject node in GameObject.FindGameObjectsWithTag("Node"))
        {
            Vector3 originalPosition = currentCenter + originalRelativePositions[node];
            node.transform.position = originalPosition;
        }
    }


    void Update()
    {
        // Tel de cooldown af
        if (stickCooldownTimer > 0f)
        {
            stickCooldownTimer -= Time.deltaTime;
        }

        // Als de linker muisknop wordt ingedrukt en de node vastzit, maak dan los
        if (Input.GetMouseButtonDown(0) && isStuck)
        {
            Unstick();
        }
    }

    // Dit wordt aangeroepen wanneer er een andere collider wordt geraakt
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kijk of het object met een kinematic rigidbody is
        Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb != null && otherRb.isKinematic && stickCooldownTimer <= 0f)
        {
            // Kijk of het oppervlak al 2 nodes heeft
            if (CanStickToSurface(collision.transform))
            {
                StickToSurface(collision.transform);
            }
        }
    }

    // Controleer of er al 2 nodes aan het surface object vastzitten
    bool CanStickToSurface(Transform surface)
    {
        if (!stickySurfaceCounters.ContainsKey(surface))
        {
            stickySurfaceCounters[surface] = 0; // Als het surface object nog geen nodes heeft, stel de counter in op 0
        }

        // Als het aantal nodes kleiner is dan maxNodes, mag de node zich vasthechten
        return stickySurfaceCounters[surface] < maxNodes;
    }

    // Maak de node vast aan het surface
    public void StickToSurface(Transform surface)
    {
        stickySurface = surface;

        // Verhoog de counter voor het surface object
        stickySurfaceCounters[surface]++;

        transform.SetParent(surface); // Zet de node als child van het stickySurface object
        rb.isKinematic = true; // Zet Rigidbody2D op Kinematic zodat hij niet beweegt
        isStuck = true;

        // Bewaar de huidige wereldpositie voordat we de node als child zetten
        Vector3 worldPosition = transform.position;

        // Zet de node als child, maar behoud de wereldpositie
        transform.SetParent(surface);

        // Zet de lokale positie terug naar de vorige wereldpositie, zodat de node op dezelfde plek blijft
        transform.position = worldPosition;
    }

    // Dit kan je aanroepen om de node los te maken
    public void Unstick()
    {
        if (stickySurface != null)
        {
            // Verlaag de counter voor het surface object
            stickySurfaceCounters[stickySurface]--;

            transform.SetParent(null); // Haal de node van de stickySurface af
            rb.isKinematic = false; // Zet de Rigidbody2D weer terug naar normaal
            isStuck = false;

            // Reset de cooldown timer zodat we na een bepaalde tijd weer kunnen vastplakken
            stickCooldownTimer = stickCooldown;
        }
    }
}
