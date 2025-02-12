using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardAlgorithmDisplay : MonoBehaviour
{
    // Assign a TextMeshPro UI component in the Inspector if you want to display the algorithm text.
    public TMP_Text displayText;

    // References to the four different color card prefabs.
    public GameObject cardAPrefab;
    public GameObject cardBPrefab;
    public GameObject cardCPrefab;
    public GameObject cardDPrefab;
    // Parent transform for spawned cards.
    public Transform cardParent;

    // Call this method from your Draw button.
    public void DrawNextSet()
    {
        // Create card lists for each type.
        List<string> typeA = new List<string>() { "a1", "a2", "a3", "a4" };
        List<string> typeB = new List<string>() { "b1", "b2", "b3", "b4" };
        List<string> typeC = new List<string>() { "c1", "c2", "c3", "c4" };
        List<string> typeD = new List<string>() { "d1", "d2", "d3", "d4" };

        // Shuffle each type independently.
        Shuffle(typeA);
        Shuffle(typeB);
        Shuffle(typeC);
        Shuffle(typeD);

        // For this draw, pick the first card of each type.
        List<string> setCards = new List<string>() { typeA[0], typeB[0], typeC[0], typeD[0] };

        // Choose 2 extra cards randomly from among the 4 types.
        List<int> extraIndices = new List<int>() { 0, 1, 2, 3 };
        Shuffle(extraIndices);

        int extraChoice1 = extraIndices[0];
        int extraChoice2 = extraIndices[1];

        if (extraChoice1 == 0) setCards.Add(typeA[0]);
        else if (extraChoice1 == 1) setCards.Add(typeB[0]);
        else if (extraChoice1 == 2) setCards.Add(typeC[0]);
        else if (extraChoice1 == 3) setCards.Add(typeD[0]);

        if (extraChoice2 == 0) setCards.Add(typeA[0]);
        else if (extraChoice2 == 1) setCards.Add(typeB[0]);
        else if (extraChoice2 == 2) setCards.Add(typeC[0]);
        else if (extraChoice2 == 3) setCards.Add(typeD[0]);

        // Now shuffle the whole 6-card set.
        Shuffle(setCards);

        // Convert the labels before display.
        List<string> displayLabels = new List<string>();
        foreach (string label in setCards)
        {
            displayLabels.Add(ConvertLabel(label.Trim()));
        }
        string setStr = string.Join(", ", displayLabels.ToArray());
        if (displayText != null)
        {
            displayText.text = setStr;
        }

        // Clear any existing cards first.
        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }
        
        // Spawn each card prefab according to its original letter, positioning them with a horizontal offset.
        for (int i = 0; i < setCards.Count; i++)
        {
            string label = setCards[i].Trim();
            GameObject prefabToSpawn = null;

            // Determine which prefab to spawn based on the first letter.
            if (label.StartsWith("a"))
                prefabToSpawn = cardAPrefab;
            else if (label.StartsWith("b"))
                prefabToSpawn = cardBPrefab;
            else if (label.StartsWith("c"))
                prefabToSpawn = cardCPrefab;
            else if (label.StartsWith("d"))
                prefabToSpawn = cardDPrefab;

            if (prefabToSpawn != null)
            {
                // Instantiate as a child with local positioning.
                GameObject newCard = Instantiate(prefabToSpawn, cardParent, false);
                newCard.transform.localPosition = new Vector3(i * 110, 0, 0);
            }
        }
    }

    // Convert the card label letter to a word.
    string ConvertLabel(string label)
    {
        if (label.StartsWith("a"))
            return label.Replace("a", "Attack");
        else if (label.StartsWith("b"))
            return label.Replace("b", "Passive");
        else if (label.StartsWith("c"))
            return label.Replace("c", "Defense");
        else if (label.StartsWith("d"))
            return label.Replace("d", "Item");
        return label;
    }

    // Generic Fisher-Yates shuffle.
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}