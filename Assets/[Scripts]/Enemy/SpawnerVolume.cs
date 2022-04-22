using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpawnerVolume : MonoBehaviour
{
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }


    /// <summary>
    /// Get position within bounds
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPositionInBounds()
    {
        Bounds boxBounds = boxCollider.bounds;
        return new Vector3(Random.Range(boxBounds.min.x, boxBounds.max.x), transform.position.y,
            Random.Range(boxBounds.min.z, boxBounds.max.z));
    }
}
