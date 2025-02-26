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
                Debug.LogError("EnemyManager reference is missing!");
                yield break;
            }

            // Find all enemies in this lane that are at or below combat trigger
            var laneEnemies = GameObject.FindObjectsOfType<EnemyPower>()
                .Where(enemy => IsInLane(enemy.transform.position.x, laneX) && 
                               enemy.transform.position.y <= enemyManager.combatTriggerY)
                .OrderByDescending(enemy => enemy.transform.position.y)  // Process from top to bottom
                .ToList(); // Convert to list to avoid multiple enumeration

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
            return Mathf.Abs(positionX - laneX) <= laneMatchTolerance;
        }

        private IEnumerator ProcessEnemyCombat(EnemyPower enemy, float laneX)
        {
            // Find the attack card in this lane
            var attackSlot = GameObject.FindGameObjectsWithTag("AttackSlot")
                .Select(go => go.GetComponent<CardSlot>())
                .Where(slot => slot != null && IsInLane(slot.transform.position.x, laneX))
                .FirstOrDefault();

            // Find the defense card in this lane
            var defenseSlot = GameObject.FindGameObjectsWithTag("DefenseSlot")
                .Select(go => go.GetComponent<CardSlot>())
                .Where(slot => slot != null && IsInLane(slot.transform.position.x, laneX))
                .FirstOrDefault();

            // Process attack phase
            if (attackSlot != null && attackSlot.HasCard)
            {
                int attackPower = attackSlot.GetCurrentPower();
                enemy.TakeDamage(attackPower);
                
                // If enemy dies from attack, destroy it and end combat for this enemy
                if (enemy.IsDead)
                {
                    Destroy(enemy.gameObject);
                    yield break;
                }
            }

            // Process defense phase - enemy survived attack
            int enemyPower = enemy.GetPower();
            
            if (defenseSlot != null && defenseSlot.HasCard)
            {
                int defensePower = defenseSlot.GetCurrentPower();
                int remainingDamage = Mathf.Max(0, enemyPower - defensePower);
                
                // Damage the defense card
                defenseSlot.TakeDamage(enemyPower);
                
                // If defense card is destroyed, apply remaining damage to player health
                if (remainingDamage > 0)
                {
                    TakeDamage(remainingDamage);
                }
            }
            else
            {
                // No defense card - direct damage to player health
                TakeDamage(enemyPower);
            }

            // Enemy dies after combat
            Destroy(enemy.gameObject);
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
    }
} 