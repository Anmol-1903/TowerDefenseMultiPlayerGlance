using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIHandler : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component for displaying player name
    [SerializeField] private TextMeshProUGUI playerNameText;

    // Method to set the player name in the UI
    public void SetPlayerName(string name)
    {
        // Log the player name for debugging
        Debug.LogWarning("Name = " + name);
        
        // Set the player name in the UI
        playerNameText.text = name;
    }
}
