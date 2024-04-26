using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataContainer : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private int playerID; public int PlayerID { get { return playerID; } }
    private Color playerColor;
    public void SetPlayerProperties(Color color, int Id)
    {
        playerColor = color;
        meshRenderer.material.color = playerColor;
        playerID = Id;
    }
}
