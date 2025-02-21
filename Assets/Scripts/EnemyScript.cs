using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public Transform enemyParent;
    
    // Horizontal spacing between lanes.
    public float laneSpacing = 200f;
    
    // Starting positions for spawning new enemies.
    public float spawnStartX = 0f;
    public float spawnStartY = 0f;
    
    // TMP_Text to display the current wave.
    public TMP_Text waveText;
    
    // Track current wave number.
    public int currentWave = 1;
    
    // NextWave is called from a UI button.
    // It moves each existing enemy down one grid slot and spawns new enemies at the top in 3 random lanes.
    public void NextWave()
    {
        // 1. Move each existing enemy down by gridStep.
        foreach (Transform enemy in enemyParent)
        {
            enemy.localPosition += Vector3.down * gridStep;
            // Also update its drift base position if it has an EnemyDrift component.
            EnemyDrift drift = enemy.GetComponent<EnemyDrift>();
            if (drift != null)
            {
                drift.SetBasePosition(enemy.localPosition);
            }
        }
        
        // 2. Determine how many new enemies to spawn for this wave.
        int spawnCount = Mathf.RoundToInt(baseEnemyCount * (currentWave * spawnMultiplier));
        
        // 3. Spawn new enemies into random lanes.
        for (int i = 0; i < spawnCount; i++)
        {
            // Randomly choose a lane (0: left, 1: center, 2: right).
            int lane = Random.Range(0, 3);
            float laneOffsetX = 0f;
            if (lane == 0)
                laneOffsetX = -laneSpacing;
            else if (lane == 1)
                laneOffsetX = 0f;
            else if (lane == 2)
                laneOffsetX = laneSpacing;
            
            // Use the adjustable spawnStartX and spawnStartY for the base spawn position.
            Vector3 spawnPos = new Vector3(spawnStartX + laneOffsetX, spawnStartY, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, enemyParent);
            
            // Optionally attach the EnemyDrift component if not already present.
            if(newEnemy.GetComponent<EnemyDrift>() == null)
                newEnemy.AddComponent<EnemyDrift>();
        }
        
        // 4. Increment the current wave.
        currentWave++;
        
        // 5. Update the wave display text.
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }
    }
}