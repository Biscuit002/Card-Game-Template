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

        // Form sets. Each set ensures one card of each type, but the order is randomized.
        int numSets = typeA.Count;
        List<string> sets = new List<string>();

        for (int i = 0; i < numSets; i++)
        {
            // Create a temporary list with one card of each type.
            List<string> setCards = new List<string>() { typeA[i], typeB[i], typeC[i], typeD[i] };
            // Shuffle the order of cards within the set.
            Shuffle(setCards);
            // Join the shuffled cards separated by commas.
            string setStr = string.Join(",", setCards.ToArray());
            sets.Add(setStr);
        }

        // Shuffle the sets.
        Shuffle(sets);

        // Combine sets into a display string, separated by " , ".
        string finalOutput = string.Join(" , ", sets.ToArray());

        // Display on UI TextMeshPro component.
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