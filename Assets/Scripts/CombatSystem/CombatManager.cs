using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Linq;  // Add this for .Select() and .Where()

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
            // Get all card slots
            CardSlot[] attackSlots = GameObject.FindGameObjectsWithTag("AttackSlot")
                .Select(go => go.GetComponent<CardSlot>())
                .Where(slot => slot != null)
                .ToArray();
                
            CardSlot[] defenseSlots = GameObject.FindGameObjectsWithTag("DefenseSlot")
                .Select(go => go.GetComponent<CardSlot>())
                .Where(slot => slot != null)
                .ToArray();

            // Process all enemies in combat
            var enemies = GameObject.FindObjectsOfType<EnemyPower>();
            bool allEnemiesProcessed = false;

            while (!allEnemiesProcessed)
            {
                allEnemiesProcessed = true;
                
                foreach (var enemy in enemies)
                {
                    if (!enemy.IsProcessed)
                    {
                        allEnemiesProcessed = false;
                        yield return StartCoroutine(ProcessCombat(enemy, attackSlots, defenseSlots));
                        yield return new WaitForSeconds(combatDelay);
                    }
                }
            }

            // Regenerate defense card powers
            foreach (var slot in defenseSlots)
            {
                if (slot.HasCard)
                {
                    slot.RegeneratePower(powerRegenerationMultiplier);
                }
            }

            // Combat phase complete
            isCombatActive = false;
            nextWaveButton.interactable = true;
        }

        private IEnumerator ProcessCombat(EnemyPower enemy, CardSlot[] attackSlots, CardSlot[] defenseSlots)
        {
            // Find corresponding attack slot
            var attackSlot = attackSlots.FirstOrDefault(slot => 
                Mathf.Approximately(slot.transform.position.x, enemy.transform.position.x));

            if (attackSlot != null && attackSlot.HasCard)
            {
                // Process attack
                int damage = attackSlot.GetCurrentPower();
                enemy.TakeDamage(damage);
                
                if (enemy.IsDead)
                {
                    Destroy(enemy.gameObject);
                    yield break;
                }
            }

            // Process defense
            var defenseSlot = defenseSlots.FirstOrDefault(slot => slot.HasCard);
            if (defenseSlot != null)
            {
                int remainingDamage = enemy.DealDamage(defenseSlot.GetCurrentPower());
                if (remainingDamage > 0)
                {
                    TakeDamage(remainingDamage);
                }
            }
            else
            {
                // No defense card, direct damage to player
                TakeDamage(enemy.GetPower());
            }

            enemy.MarkAsProcessed();
            Destroy(enemy.gameObject);
        }

        public void TakeDamage(int damage)
        {
            currentPlayerHealth -= damage;
            UpdateHealthDisplay();
            
            if (currentPlayerHealth <= 0)
            {
                // Implement game over logic here
                Debug.Log("Game Over!");
            }
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