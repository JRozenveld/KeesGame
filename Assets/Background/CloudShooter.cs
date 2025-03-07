using System.Collections;
using UnityEngine;

public class CannonShooter : MonoBehaviour
{
    public GameObject[] cannonBalls; // 3 verschillende projectielen
    public Transform firePoint;      // Waar de kogels worden afgevuurd
    public float timeBetweenShots = 1f;  // Tijd tussen elk schot binnen de cyclus
    public float shootCycleInterval = 3f; // Tijd tussen volledige cycli
    public float cannonBallSpeed = 5f;    // Hoe snel de kogel vliegt
    public float rotationSpeed = 360f;    // Hoe snel de kogel ronddraait (graden per seconde)
    public float maxDistance = 10f;       // Hoe ver de kogel mag vliegen

    private Vector3[] startPositions; // Opslaan van startposities

    void Start()
    {
        startPositions = new Vector3[cannonBalls.Length];
        for (int i = 0; i < cannonBalls.Length; i++)
        {
            startPositions[i] = cannonBalls[i].transform.position;
        }

        StartCoroutine(ShootLoop());
    }

    IEnumerator ShootLoop()
    {
        while (true)
        {
            for (int i = 0; i < cannonBalls.Length; i++)
            {
                Shoot(cannonBalls[i], startPositions[i]);
                yield return new WaitForSeconds(timeBetweenShots); // Wacht tussen elk schot
            }
            yield return new WaitForSeconds(shootCycleInterval); // Wacht na de volledige cyclus
        }
    }

    void Shoot(GameObject cannonBall, Vector3 startPos)
    {
        cannonBall.SetActive(true);
        cannonBall.transform.position = firePoint.position;
        StartCoroutine(MoveAndRotateCannonBall(cannonBall, startPos));
    }

    IEnumerator MoveAndRotateCannonBall(GameObject ball, Vector3 startPos)
    {
        float traveledDistance = 0f;

        while (traveledDistance < maxDistance)
        {
            float moveStep = cannonBallSpeed * Time.deltaTime;
            float rotateStep = rotationSpeed * Time.deltaTime;

            ball.transform.Translate(Vector3.right * moveStep, Space.World);
            ball.transform.Rotate(Vector3.forward, -rotateStep);

            traveledDistance += moveStep;
            yield return null;
        }

        ball.transform.position = startPos;
        ball.SetActive(false);
    }
}
