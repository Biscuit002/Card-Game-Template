using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

namespace CombatSystem
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }
        
        [Header("Combat Settings")]
        [SerializeField] private float combatDelay = 0.5f;
        [SerializeField] private float powerRegenerationMultiplier = 0.2f;
        [SerializeField] private int basePlayerHealth = 100;
        
        [Header("UI References")]
        [SerializeField] private Button nextWaveButton;
        [SerializeField] private TextMeshProUGUI healthText;
        
        [Header("Lane Settings")]
        [SerializeField] private float leftLaneX = -200f;    // Match your lane spacing
        [SerializeField] private float centerLaneX = 0f;
        [SerializeField] private float rightLaneX = 200f;
        [SerializeField] private float laneMatchTolerance = 10f;  // How close something needs to be to count as in the lane

        private int currentPlayerHealth;
        private bool isCombatActive = false;
        private EnemyScript enemyManager;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            currentPlayerHealth = basePlayerHealth;
            enemyManager = FindObjectOfType<EnemyScript>();
            UpdateHealthDisplay();
        }

        public void StartCombatPhase()
        {
            if (!isCombatActive)
            {
                isCombatActive = true;
                nextWaveButton.interactable = false;
                StartCoroutine(CombatSequence());
            }
        }

        private IEnumerator CombatSequence()
        {
            // Process enemies lane by lane
            yield return StartCoroutine(ProcessLaneCombat(leftLaneX));
            yield return StartCoroutine(ProcessLaneCombat(centerLaneX));
            yield return StartCoroutine(ProcessLaneCombat(rightLaneX));

            // Regenerate defense card powers after combat
            var defenseSlots = GameObject.FindGameObjectsWithTag("DefenseSlot")
                .Select(go => go.GetComponent<CardSlot>())
                .Where(slot => slot != null && slot.HasCard);
                
            foreach (var slot in defenseSlots)
            {
                slot.RegeneratePower(powerRegenerationMultiplier);
            }

            // Combat phase complete
            isCombatActive = false;
            nextWaveButton.interactable = true;
        }

        private IEnumerator ProcessLaneCombat(float laneX)
        {
            if (enemyManager == null)
            {
                yield break;
            }

            // Find all enemies in this lane that are at or below combat trigger
            var laneEnemies = GameObject.FindObjectsOfType<EnemyPower>().Where(enemy => IsInLane(enemy.transform.position.x, laneX) && enemy.transform.position.y <= enemyManager.combatTriggerY).OrderByDescending(enemy => enemy.transform.position.y);

            foreach (var enemy in laneEnemies)
            {
                if (enemy != null) // Check if enemy still exists
                {
                    yield return StartCoroutine(ProcessEnemyCombat(enemy, laneX));
                    yield return new WaitForSeconds(combatDelay);
                }
            }
        }

        private bool IsInLane(float positionX, float laneX)
        {
            float distance = Mathf.Abs(positionX - laneX);
            return distance <= laneMatchTolerance;
        }

        private IEnumerator ProcessEnemyCombat(EnemyPower enemy, float laneX)
        {
            
            var attackSlot = GameObject.FindGameObjectsWithTag("SnapTarget")
                .Select(go => go.GetComponent<CardSlot>())
                .Where(slot => slot != null && IsInLane(slot.transform.position.x, laneX))
                .FirstOrDefault();

            var defenseSlot = GameObject.FindGameObjectsWithTag("DefenseSlot")
                .Select(go => go.GetComponent<CardSlot>())
                .Where(slot => slot != null && IsInLane(slot.transform.position.x, laneX))
                .FirstOrDefault();

            int enemyPower = enemy.GetPower();

            // Process attack phase
            if (attackSlot != null && attackSlot.HasCard)
            {
                int attackPower = attackSlot.GetCurrentPower();
                
                if (attackPower >= enemyPower)
                {
                    Destroy(enemy.gameObject);
                    yield break;
                }
                else
                {
                    enemy.TakeDamage(attackPower);
                    enemyPower = enemy.GetPower();
                }
            }

            // Process defense phase
            if (enemy != null)
            {
                if (defenseSlot != null && defenseSlot.HasCard)
                {
                    int defensePower = defenseSlot.GetCurrentPower();

                    if (defensePower >= enemyPower)
                    {
                        defenseSlot.TakeDamage(enemyPower);
                        
                        if (defenseSlot.GetCurrentPower() <= 0)
                        {
                            defenseSlot.DestroyCard();
                        }
                    }
                    else
                    {
                        int remainingDamage = enemyPower - defensePower;
                        defenseSlot.TakeDamage(defensePower);
                        TakeDamage(remainingDamage);
                        defenseSlot.DestroyCard();
                    }
                }
                else
                {
                    TakeDamage(enemyPower);
                }

                Destroy(enemy.gameObject);
            }
            
        }

        public void TakeDamage(int damage)
        {
            currentPlayerHealth -= damage;
            UpdateHealthDisplay();
            
            if (currentPlayerHealth <= 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            Debug.Log("Game Over!");
            // Implement your game over logic here
        }

        private void UpdateHealthDisplay()
        {
            if (healthText != null)
            {
                healthText.text = $"Health: {currentPlayerHealth}";
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("SnapTarget"))
            {
                print("SnapTarget entered");
            }
        }

        void OnDrawGizmos()
        {
            // Draw the lane lines in blue
            Gizmos.color = Color.blue;
            float lineLength = 2000f; // How tall to make the lines
            float topY = 1000f;      // Top of the lines
            float bottomY = -1000f;   // Bottom of the lines
            
            // Left lane
            Gizmos.DrawLine(
                new Vector3(leftLaneX, bottomY, 0),
                new Vector3(leftLaneX, topY, 0)
            );
            
            // Center lane
            Gizmos.DrawLine(
                new Vector3(centerLaneX, bottomY, 0),
                new Vector3(centerLaneX, topY, 0)
            );
            
            // Right lane
            Gizmos.DrawLine(
                new Vector3(rightLaneX, bottomY, 0),
                new Vector3(rightLaneX, topY, 0)
            );
            
            // Optionally draw lane tolerance zones
            Gizmos.color = new Color(0, 0, 1, 0.2f); // Transparent blue
            foreach (float laneX in new float[] { leftLaneX, centerLaneX, rightLaneX })
            {
                // Draw tolerance zone for each lane
                Gizmos.DrawLine(
                    new Vector3(laneX - laneMatchTolerance, bottomY, 0),
                    new Vector3(laneX + laneMatchTolerance, topY, 0)
                );
                Gizmos.DrawLine(
                    new Vector3(laneX + laneMatchTolerance, bottomY, 0),
                    new Vector3(laneX - laneMatchTolerance, topY, 0)
                );
            }
        }
    }
} 