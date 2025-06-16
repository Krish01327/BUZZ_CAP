using UnityEngine;

public class RotatingWheel : MonoBehaviour
{
    public float rotationSpeed = 150f; // Degrees per second

    void Update()
    {
        // Rotate around the Z axis (in place)
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}