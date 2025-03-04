using Unity.VisualScripting;
using UnityEngine;

public class CombatManager1 : MonoBehaviour
{

    private float enemyCooldown;
    private Vector3 enemyPosition1;
    private Vector3 enemyPosition2;
    private Vector3 enemyPosition3;
    public GameObject enemyPrefab;
    private GameObject enemyClone;
    void Start()
    {
        enemyCooldown = 3;

        enemyPosition1 = new Vector3(3.59f, 6, 0);
        enemyPosition2 = new Vector3(5.43f, 6, 0);
        enemyPosition3 = new Vector3(7.38f, 6, 0);
    }
    void Update()
    {
        enemyPrefab = GameObject.Find("Enemy");
        if (enemyCooldown > 0)
        {
            enemyCooldown -= Time.deltaTime;
        }
        else
        {
            int randomValue = Random.Range(1, 4);
            if (randomValue == 1)
            {
                enemyClone = Instantiate(enemyPrefab, enemyPosition1, Quaternion.identity);
            }
            if (randomValue == 2)
            {
                enemyClone = Instantiate(enemyPrefab, enemyPosition2, Quaternion.identity);
            }
            if (randomValue == 3)
            {
                enemyClone = Instantiate(enemyPrefab, enemyPosition3, Quaternion.identity);
            }
            enemyCooldown = 3;
        }
        if (enemyClone != null)
        {
            enemyClone.transform.position = new Vector3(enemyClone.transform.position.x, enemyClone.transform.position.y - Time.deltaTime, enemyClone.transform.position.z);
        }
    }
}
