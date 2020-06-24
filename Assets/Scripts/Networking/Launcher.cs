using System;
using System.Collections;
using MultiplayerPinball.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace MultiplayerPinball.Networking
{
	public class Launcher : MonoBehaviourPunCallbacks
	{
		public GameObject connectingUi;
		public GameObject notConnectedUi; 
		public byte maxPlayersPerRoom;

		public WaitingForPlayersUi lobbyUi;

		private const string _gameVersion = "1";
		private bool _isConnecting; 
		
		private void Awake()
		{
			//Make sure we can use PhotonNetwork.LoadLevel() on master client
			PhotonNetwork.AutomaticallySyncScene = true; 
		}

		#region Public Methods

		public void ConnectToGame()
		{
			_isConnecting = true;
			
			MainMenuUiManager.Instance.SetGameSearchUi(true);
			
			if (PhotonNetwork.IsConnected)
			{
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				PhotonNetwork.GameVersion = _gameVersion;
				PhotonNetwork.ConnectUsingSettings(); 
			}
		}

		public void LoadGame()
		{
			Debug.Log("Load Game :: Current room  : " + PhotonNetwork.CurrentRoom.Name);
			Debug.Log("Load Game :: Current players in lobby : " + PhotonNetwork.CurrentRoom.PlayerCount);
			Debug.Log("Load Game :: Player at 1  : " + PhotonNetwork.CurrentRoom.GetPlayer(1).NickName);
			
			//Loop through the players and add custom property to identify them in game
			for (int i = 1; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
			{
				Player player = PhotonNetwork.CurrentRoom.GetPlayer(i);
				Hashtable playerHash = player.CustomProperties;
				playerHash.Add("playerIndex", i);
				player.SetCustomProperties(playerHash);
			}
			
			PhotonNetwork.LoadLevel("RoomFor1");
		}

		#endregion

		#region PunCallbacks

		public override void OnConnectedToMaster()
		{
			if (_isConnecting)
			{
				PhotonNetwork.JoinRandomRoom(); 
			}
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, new RoomOptions {MaxPlayers = maxPlayersPerRoom});
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			_isConnecting = false; 
		}

		public override void OnJoinedRoom()
		{
			PhotonNetwork.CurrentRoom.AddPlayer(PhotonNetwork.LocalPlayer);
			lobbyUi.AddPlayerEntry(PhotonNetwork.LocalPlayer);
			if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
			{
				StartCoroutine(CoWaitThenLoadGame(2));
			}
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			PhotonNetwork.CurrentRoom.AddPlayer(newPlayer);
			lobbyUi.AddPlayerEntry(newPlayer);
			if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
			{
				StartCoroutine(CoWaitThenLoadGame(2));
			}
		}

		#endregion

		#region Coroutines

		private IEnumerator CoWaitThenLoadGame(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			LoadGame();
		}

		#endregion
	}
}