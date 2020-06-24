using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerPinball.UI
{
	public class PlayerEntry : MonoBehaviour
	{
		public Image avatarImage;
		public TMP_Text usernameText;

		public void Init(Player photonPlayer)
		{
			usernameText.text = photonPlayer.NickName; 
		}
	}
}