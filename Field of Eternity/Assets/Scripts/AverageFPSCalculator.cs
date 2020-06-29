using UnityEngine;

public class AverageFPSCalculator : MonoBehaviour
{
    private float averageFPS;
    private float timestamp = 1f;

    private void Update()
    {
        averageFPS = Time.frameCount / Time.time;
        timestamp -= Time.deltaTime;

        if(timestamp <= 0)
        {
            Debug.Log("Average FPS: " + averageFPS);
            timestamp = 5f;
        }
    }
}
