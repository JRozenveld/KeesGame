using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float startPosY;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPosY = transform.position.y;
    }

    void Update()
    {
        float dist = (cam.transform.position.y * parallaxEffect);
        transform.position = new Vector3(transform.position.x, startPosY + dist, transform.position.z);
    }
}