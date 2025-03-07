using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class CombatManager1 : MonoBehaviour
{
    private float enemyCooldown;
    private Vector3 enemyPosition1;
    private Vector3 enemyPosition2;
    private Vector3 enemyPosition3;
    private Vector3 spawnPosition;
    private int randomValue;
    
    public GameObject enemyPrefab;
    public GameManager gameManager;
    public CardPickup cardPickup;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    
    void Start()
    {
        enemyCooldown = 3;

        enemyPosition1 = new Vector3(3.59f, 6, 0);
        enemyPosition2 = new Vector3(5.43f, 6, 0);
        enemyPosition3 = new Vector3(7.38f, 6, 0);
    }
    
    void Update()
    {
        cardPickup = FindObjectOfType<CardPickup>();
        if (enemyCooldown > 0)
        {
            enemyCooldown -= Time.deltaTime;
        }
        if (enemyCooldown <= 0)
        {
            randomValue = Random.Range(1, 4);
            if (randomValue == 1)
            {
                spawnPosition = enemyPosition1;
            }
            if (randomValue == 2) 
            {
                spawnPosition = enemyPosition2;
            }
            if (randomValue == 3)
            {
                spawnPosition = enemyPosition3;
            }
                
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(newEnemy);
            enemyCooldown = 3;

            print(cardPickup.closestTarget);

        }
        
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = spawnedEnemies[i];
            GameObject currentTarget = cardPickup.closestTarget;
            enemy.transform.position += Vector3.down * Time.deltaTime;
            if (enemy.transform.position.y < -1 && randomValue == 1)
            {
                print(cardPickup.GetSnapTargetPower(currentTarget));
                if (cardPickup.GetSnapTargetPower(currentTarget) >= 0)
                {
                    Destroy(enemy);
                    spawnedEnemies.RemoveAt(i);
                }
            }
            if (enemy.transform.position.y < -1 && randomValue == 1)
            {
                //print("get card value collumn 2");
            }
            if (enemy.transform.position.y < -1 && randomValue == 1)
            {
               // print("get card value collumn 3");
            }
            if (enemy.transform.position.y < -6)
            {
                Destroy(enemy);
                spawnedEnemies.RemoveAt(i);
            }
            if (spawnedEnemies.Count > 3)
            {
                Destroy(enemy);
                spawnedEnemies.RemoveAt(i);
            }
        }
    }
}