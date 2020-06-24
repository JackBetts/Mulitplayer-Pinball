using System;
using System.Collections;
using System.Collections.Generic;
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
        
        SetHealthText(connectedPlayer.playerHealth);
    }

    public void SetHealthText(int amount)
    {
        healthText.text = amount.ToString(); 
    }
}
