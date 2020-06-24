using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerPinball.UI
{
    public class MainMenuUiManager : MonoBehaviour
    {
        public static MainMenuUiManager Instance; 
        
        public GameObject usernameUi;
        public TMP_InputField usernameInput;

        public GameObject waitingForPlayersUi; 

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
            bool firstLaunch = PlayerPrefs.GetInt(GameGlobals.kFirstLaunchKey) == 0;

            if (firstLaunch)
            {
                usernameUi.SetActive(true);
                PlayerPrefs.SetInt(GameGlobals.kFirstLaunchKey, 1); 
            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString(GameGlobals.kPlayerUsernameKey); 
            }
        }

        public void OnEnteredUsername()
        {
            if (string.IsNullOrEmpty(usernameInput.text)) return;

            string newUsername = usernameInput.text;
            PhotonNetwork.NickName = newUsername; 
            PlayerPrefs.SetString(GameGlobals.kPlayerUsernameKey, newUsername);

            usernameUi.GetComponent<DisableTweenedUiElement>()?.DisableElement();
        }

        public void SetGameSearchUi(bool active)
        {
            waitingForPlayersUi.SetActive(active);
        }
    }
}
