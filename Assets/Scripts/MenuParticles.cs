using UnityEngine;
using UnityEngine.UI;

public class MenuParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlePrefab; // Changed to ParticleSystem type
    [SerializeField] private Button menuButton; // Button that triggers particles
    [SerializeField] private Transform spawnPoint; // Where particles spawn (optional)
    
    private void Start()
    {
        // Add listener to button click
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(SpawnParticles);
        }
        
        // If no spawn point set, use button position
        if (spawnPoint == null)
        {
            spawnPoint = menuButton.transform;
        }
    }

    private void SpawnParticles()
    {
        if (particlePrefab != null)
        {
            // Instantiate as ParticleSystem to maintain all settings
            ParticleSystem particles = Instantiate(particlePrefab, spawnPoint.position, particlePrefab.transform.rotation);
            
            // Get the original settings
            var mainModule = particles.main;
            
            // Destroy after duration
            Destroy(particles.gameObject, mainModule.duration);
        }
        else
        {
            Debug.LogWarning("Particle system prefab not assigned!");
        }
    }
} 