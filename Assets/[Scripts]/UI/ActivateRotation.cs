using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRotation : MonoBehaviour
{
    public float rotationFactor = 0.1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward,rotationFactor);
    }
}
