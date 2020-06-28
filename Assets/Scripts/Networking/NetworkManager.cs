using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiplayerPinball.Networking
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public delegate void PlayerListFilled(List<PlayerController> players);
        public static event PlayerListFilled OnPlayerListFilled;
        
        public static NetworkManager Instance;
        
        public GameObject playerPrefab;
        public List<PlayerController> players = new List<PlayerController>();
        public Transform[] playerSpawnPositions;
        public Camera[] playerCameras;

        [Header("Player Stats")] 
        public List<int> playersHealthValues = new List<int>();

        
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("MainMenu"); 
                return;
            }
    
            if (playerPrefab == null)
            {
                Debug.LogError("No Player Prefab assigned");
            }
            else
            {
                Player localPlayer = PhotonNetwork.LocalPlayer;
                int playerIndex = Convert.ToInt32(localPlayer.CustomProperties["playerIndex"]);
                Debug.Log("Spawn Player :: Players index is " + playerIndex);
                
                ActivatePlayerCamera(playerIndex);
                
                PlayerController newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, GetSpawnPosition(playerIndex), Quaternion.identity).GetComponent<PlayerController>();
                newPlayer.playerIndex = playerIndex; 
                Invoke(nameof(LookForPlayers), 2f);
            }
        }
        
        private void LookForPlayers()
        {
            players = FindObjectsOfType<PlayerController>().ToList();
            players = players.OrderBy(x => x.playerIndex).ToList();
            OnPlayerListFilled?.Invoke(players);
            AddPlayerHealthValues();
        }

        private void AddPlayerHealthValues()
        {
            foreach (PlayerController player in players)
            {
                playersHealthValues.Add(player.playerHealth);
            }
        }

        public void TakePlayerHealth(int playerIndex)
        {
            playersHealthValues[playerIndex] -= 1;

            if (playersHealthValues[playerIndex] <= 0)
            {
                string msg = $"Player {playerIndex} lost!";
                Debug.Log(msg); 
            }
        }
    
        #region Photon Callbacks
    
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("Network Manager :: Player left room : " + otherPlayer.NickName);
    
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount <= 1)
            {
                PhotonNetwork.LeaveRoom(); 
            }
        }
    
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu"); 
        }
    
        #endregion

        private void ActivatePlayerCamera(int playerId)
        {
            foreach (Camera cam in playerCameras)
            {
                cam.enabled = false; 
            }

            playerCameras[playerId].enabled = true; 
        }
        
        private Vector3 GetSpawnPosition(int playerId)
        {
            InputManager.Instance.CalculateMoveAxis(playerId);
            return playerSpawnPositions[playerId].position;
        }
    }
}
