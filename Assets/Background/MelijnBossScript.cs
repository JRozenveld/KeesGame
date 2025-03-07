using System.Collections;
using UnityEngine;

public class FallingItems : MonoBehaviour
{
    public GameObject[] fallingItems;  // Meerdere objecten die kunnen vallen (bijv. afbeeldingen, objecten, etc.)
    public Transform dropPoint;        // Waar de objecten vandaan vallen
    public float timeBetweenDrops = 1f; // Tijd tussen elk object dat valt
    public float dropCycleInterval = 3f; // Tijd tussen volledige cycli van vallen
    public float dropSpeed = 3f;        // Snelheid waarmee de objecten naar beneden vallen
    public float rotationSpeed = 360f;  // Hoe snel het object ronddraait (graden per seconde)
    public float vanishHeight = -5f;    // Hoogte waarna de objecten verdwijnen
    public float maxDistance = 10f;     // Maximaal aantal objecten dat wordt gedropt

    private Vector3[] startPositions; // Opslaan van startposities voor de objecten

    void Start()
    {
        // Sla de beginposities van de objecten op
        startPositions = new Vector3[fallingItems.Length];
        for (int i = 0; i < fallingItems.Length; i++)
        {
            startPositions[i] = fallingItems[i].transform.position;
            fallingItems[i].SetActive(false);  // Zorg ervoor dat objecten inactief zijn bij start
        }

        // Start de drop-cyclus
        StartCoroutine(DropLoop());
    }

    IEnumerator DropLoop()
    {
        while (true)
        {
            // Laat voor elk object vallen
            for (int i = 0; i < fallingItems.Length; i++)
            {
                DropItem(fallingItems[i], startPositions[i]);
                yield return new WaitForSeconds(timeBetweenDrops); // Wacht tussen het vallen van objecten
            }

            yield return new WaitForSeconds(dropCycleInterval); // Wacht na een volledige cyclus
        }
    }

    void DropItem(GameObject item, Vector3 startPos)
    {
        item.SetActive(true);
        item.transform.position = dropPoint.position;  // Zet het object op de dropPoint

        // Start de coroutine voor het laten vallen en roteren van het object
        StartCoroutine(FallAndRotateItem(item, startPos));
    }

    IEnumerator FallAndRotateItem(GameObject item, Vector3 startPos)
    {
        float traveledDistance = 0f;

        while (item.transform.position.y > vanishHeight && traveledDistance < maxDistance)
        {
            // Laat het object vallen met een snelheid
            float moveStep = dropSpeed * Time.deltaTime;

            // Laat het object roteren (ronddraaien)
            float rotateStep = rotationSpeed * Time.deltaTime;

            item.transform.Translate(Vector3.down * moveStep, Space.World);  // Beweeg naar beneden
            item.transform.Rotate(Vector3.forward, -rotateStep);  // Draai het object

            traveledDistance += moveStep;

            yield return null;  // Wacht op de volgende frame
        }

        // Zet het object terug naar de beginpositie en zet het uit
        item.transform.position = startPos;
        item.SetActive(false);
    }
}