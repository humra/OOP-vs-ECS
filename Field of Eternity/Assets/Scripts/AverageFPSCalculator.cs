using UnityEngine;

public class AverageFPSCalculator : MonoBehaviour
{
    private float averageFPS;
    private float timestamp;

    [SerializeField]
    private float interval = 5f;

    private void Start()
    {
        timestamp = interval;
    }

    private void Update()
    {
        averageFPS = Time.frameCount / Time.time;
        timestamp -= Time.deltaTime;

        if(timestamp <= 0)
        {
            Debug.Log("Average FPS: " + averageFPS);
            timestamp = interval;
        }
    }
}
