using UnityEngine;

public class RotationDrift : MonoBehaviour
{
    // The base z-rotation (in degrees) at start.
    private float baseZRotation;
    // Random phase offset so that drift is unsynchronized.
    private float randomRotationPhase;
    
    // Maximum additional rotation (in degrees) applied.
    public float rotationAmplitude = 2f;
    // Speed of rotation oscillation.
    public float rotationSpeed = 1f;

    void Start()
    {
        // Store the initial local z rotation.
        baseZRotation = transform.localEulerAngles.z;
        // Set a random phase so the rotation drift is not synchronized.
        randomRotationPhase = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        // Calculate small oscillating rotation offset.
        float offset = Mathf.Sin(Time.time * rotationSpeed + randomRotationPhase) * rotationAmplitude;
        // Apply only rotation drift (z-axis) while keeping other rotation components unchanged.
        transform.localRotation = Quaternion.Euler(0, 0, baseZRotation + offset);
    }
}