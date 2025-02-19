using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Grid step for each wave (vertical distance for one row).
    public float gridStep = 100f;
    // Base number of enemies to spawn each wave.
    public int baseEnemyCount = 3;
    // Multiplier applied per wave (so higher wave spawns more enemies).
    public float spawnMultiplier = 1f;
    // Enemy prefab to spawn.
    public GameObject enemyPrefab;
    // Parent transform under which all enemy instances reside.
    // This should remain fixed.
    public Transform enemyParent;
    
    // Track current wave number.
    public int currentWave = 1;
    
    // NextWave is called from a UI button.
    // It moves each existing enemy down one grid slot and spawns new enemies at the top.
    public void NextWave()
    {
        // 1. Move each existing enemy down by gridStep.
        foreach (Transform enemy in enemyParent)
        {
            enemy.localPosition += Vector3.down * gridStep;
        }
        /
        // 2. Determine how many new enemies to spawn for this wave.
        int spawnCount = Mathf.RoundToInt(baseEnemyCount * (currentWave * spawnMultiplier));
        
        // 3. Calculate horizontal positioning for a centered row.
        float spacing = gridStep; // Use gridStep as horizontal spacing.
        float totalWidth = (spawnCount - 1) * spacing;
        float startX = -totalWidth / 2;
        // New enemies should spawn at the top row, so use a fixed local y coordinate.
        float spawnY = 0f;
        
        // 4. Spawn the new enemies at the top row (local positions relative to enemyParent).
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = new Vector3(startX + i * spacing, spawnY, 0);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity, enemyParent);
        }
        
        // 5. Increment the current wave.
        currentWave++;
    }
}