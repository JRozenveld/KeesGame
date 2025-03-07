using UnityEngine;

public class SoftRigidBody : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform[] nodes; // Bestaande nodes

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        nodes = GetComponentsInChildren<Transform>();

        // Verwijder het hoofdobject uit de lijst van nodes
        nodes = System.Array.FindAll(nodes, node => node != transform);

        // Stel de LineRenderer in op het aantal nodes
        lineRenderer.positionCount = nodes.Length + 1; // +1 om de lus te sluiten
    }

    void Update()
    {
        // Werk de LineRenderer bij met de posities van de nodes
        for (int i = 0; i < nodes.Length; i++)
        {
            lineRenderer.SetPosition(i, nodes[i].position);
        }

        // Sluit de cirkel door het laatste punt naar het eerste te zetten
        lineRenderer.SetPosition(nodes.Length, nodes[0].position);
    }
}
