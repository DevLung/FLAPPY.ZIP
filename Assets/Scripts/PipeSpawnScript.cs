using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;
    public float spawnInterval;
    private float timer;
    public float maxHeightOffset;

    void Start()
    {
        // spawn first pipe immediately
        timer = spawnInterval;
    }

    void Update()
    {
        if (timer < spawnInterval)
        {
            timer += Time.deltaTime;
        } else
        {
            SpawnPipe();
            timer = 0;
        }
    }

    void SpawnPipe()
    {
        float lowestPoint = transform.position.y - maxHeightOffset;
        float highestPoint = transform.position.y + maxHeightOffset;

        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), transform.position.z), transform.rotation);
    }
}
