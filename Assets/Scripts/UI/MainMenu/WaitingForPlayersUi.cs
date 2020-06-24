using System;
using System.Collections;
using System.Collections.Generic;
using MultiplayerPinball.UI;
using Photon.Realtime;
using UnityEngine;

public class WaitingForPlayersUi : MonoBehaviour
{
    public GameObject playerEntryPrefab;
    public Transform playerEntryParent;

    public void AddPlayerEntry(Player player)
    {
        GameObject i = Instantiate(playerEntryPrefab, playerEntryParent); 
        i.GetComponent<PlayerEntry>()?.Init(player);
    }

    private void RemoveAllEntries()
    {
        for (int i = playerEntryParent.childCount; i >= 0; i--)
        {
            Destroy(playerEntryParent.GetChild(i));
        }
    }
}
