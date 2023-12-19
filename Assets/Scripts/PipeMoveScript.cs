using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float moveSpeed;
    public float despawnPosition = -10;

    void Update()
    {
        transform.position += (Vector3.left * moveSpeed) * Time.deltaTime;

        // despawn pipes if out of bounds
        if (transform.position.x <= despawnPosition)
        {
            Destroy(gameObject);
        }
    }
}
