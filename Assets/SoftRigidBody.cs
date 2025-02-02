using System.Linq;
using UnityEngine;

public class SoftRigidBody : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform[] nodes;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        nodes = GetComponentsInChildren<Transform>()
            .Where(t => t != transform) // Hoofdobject uitsluiten
            .OrderBy(t => Mathf.Atan2(t.position.y - transform.position.y, t.position.x - transform.position.x)) // Sorteren op hoek
            .ToArray();

        lineRenderer.positionCount = nodes.Length + 1; // +1 om de lus te sluiten
    }


  void Update()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            lineRenderer.SetPosition(i, nodes[i].position);
        }
        // Laatste punt gelijk maken aan eerste om de cirkel te sluiten
        lineRenderer.SetPosition(nodes.Length, nodes[0].position);
    }
}
