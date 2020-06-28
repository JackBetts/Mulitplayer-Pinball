using System;
using System.Collections;
using System.Collections.Generic;
using MultiplayerPinball.Networking;
using Photon.Realtime;
using UnityEngine;

public class PlayerStatsUi : MonoBehaviour
{
    public PlayerStat[] playerStats;
    public List<PlayerController> playerControllers;

    private void OnEnable()
    {
        NetworkManager.OnPlayerListFilled += NetworkManagerOnOnPlayerListFilled;
    }

    private void OnDisable()
    {
        NetworkManager.OnPlayerListFilled -= NetworkManagerOnOnPlayerListFilled;
    }

    private void NetworkManagerOnOnPlayerListFilled(List<PlayerController> players)
    {
        playerControllers = new List<PlayerController>(players);
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (i + 1 > playerControllers.Count) return;
            if (playerControllers[i])
            {
                playerStats[i].Init(playerControllers[i]);   
            }
        }
    }
}
