using UnityEngine;

public class EnemyDrift : MonoBehaviour
{
    // The base position around which the enemy will drift.
    private Vector3 basePosition;
    // Random phases for translation and rotation.
    private float randomPhase;
    private float randomRotationPhase;
    
    // How far the enemy can drift (in units).
    public float driftAmplitude = 5f;
    // Speed of the drift movement.
    public float driftSpeed = 1f;
    
    // Maximum degrees for the enemy's rotation oscillation.
    public float rotationAmplitude = 2f;
    // Speed of the rotation oscillation.
    public float rotationSpeed = 1f;

    void Start()
    {
        // Store the initial local position as the base.
        basePosition = transform.localPosition;
        // Set each enemy's drift phase randomly so they don't sync.
        randomPhase = Random.Range(0f, Mathf.PI * 2f);
        randomRotationPhase = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        // Calculate drift offsets using sine and cosine.
        float offsetX = Mathf.Cos(Time.time * driftSpeed + randomPhase) * driftAmplitude;
        float offsetY = Mathf.Sin(Time.time * driftSpeed + randomPhase) * driftAmplitude;
        transform.localPosition = basePosition + new Vector3(offsetX, offsetY, 0);
        
        // Calculate a small oscillating rotation.
        float rot = Mathf.Sin(Time.time * rotationSpeed + randomRotationPhase) * rotationAmplitude;
        transform.localRotation = Quaternion.Euler(0, 0, rot);
    }

    // When the enemy is repositioned (e.g. during a wave advance), update its base position.
    public void SetBasePosition(Vector3 newBase)
    {
        basePosition = newBase;
    }
}