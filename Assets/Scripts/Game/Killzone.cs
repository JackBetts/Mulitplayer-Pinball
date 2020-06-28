using System;
using System.Collections;
using System.Collections.Generic;
using MultiplayerPinball.Networking;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public int playerIndex;
    public PlayerController connectedPlayer;

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
        if (playerIndex + 1 > players.Count) return;
        connectedPlayer = players[playerIndex]; 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ball"))
        {
            PhotonNetwork.Destroy(other.gameObject);
            

            if (!connectedPlayer) return;
            NetworkManager.Instance.TakePlayerHealth(connectedPlayer.playerIndex);
        }
    }
}
