using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardAlgorithmDisplay : MonoBehaviour
{
    // Assign a TextMeshPro UI component in the Inspector.
    public TMP_Text displayText;

    void Start()
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

        // Determine how many sets to form (based on the count in each type).
        int numSets = typeA.Count;
        List<string> sets = new List<string>();

        for (int i = 0; i < numSets; i++)
        {
            // Start with the base 4 cards (one of each type).
            List<string> setCards = new List<string>() { typeA[i], typeB[i], typeC[i], typeD[i] };
            
            // Choose 2 extra cards randomly from among the 4 types.
            List<int> extraIndices = new List<int>() { 0, 1, 2, 3 };
            Shuffle(extraIndices);

            // For each extra card, based on the random type chosen.
            int extraChoice1 = extraIndices[0];
            int extraChoice2 = extraIndices[1];
            // extra card depending on the chosen type index.
            if (extraChoice1 == 0) setCards.Add(typeA[i]);
            else if (extraChoice1 == 1) setCards.Add(typeB[i]);
            else if (extraChoice1 == 2) setCards.Add(typeC[i]);
            else if (extraChoice1 == 3) setCards.Add(typeD[i]);

            if (extraChoice2 == 0) setCards.Add(typeA[i]);
            else if (extraChoice2 == 1) setCards.Add(typeB[i]);
            else if (extraChoice2 == 2) setCards.Add(typeC[i]);
            else if (extraChoice2 == 3) setCards.Add(typeD[i]);

            // Now shuffle the whole 6-card set.
            Shuffle(setCards);

            // Join the shuffled cards with commas.
            string setStr = string.Join(",", setCards.ToArray());
            sets.Add(setStr);
        }

        // Shuffle the sets themselves.
        Shuffle(sets);

        // Combine sets into a final display string, separated by " , ".
        string finalOutput = string.Join(" , ", sets.ToArray());

        // Display on the TextMeshPro component.
        if (displayText != null)
        {
            displayText.text = finalOutput;
        }
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