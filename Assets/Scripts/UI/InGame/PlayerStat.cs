using System;
using System.Collections;
using System.Collections.Generic;
using MultiplayerPinball.Networking;
using UnityEngine;
using TMPro;

public class PlayerStat : MonoBehaviour
{
    public TMP_Text healthText;
    private PlayerController connectedPlayer;

    public void Init(PlayerController player) => connectedPlayer = player;

    private void Update()
    {
        if (!connectedPlayer) return;
        NetworkManager manager = NetworkManager.Instance;
        SetHealthText(manager.playersHealthValues[connectedPlayer.playerIndex]);
    }

    public void SetHealthText(int amount)
    {
        healthText.text = amount.ToString(); 
    }
}
