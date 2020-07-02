using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playerIndex;
    public float moveSpeed;
    public int playerHealth;

    void Update()
    {
        if (!photonView.IsMine) return;
        
        Vector3 currentPosition = transform.position;
        Vector3 desiredPosition = InputManager.Instance.GetNewPlayerPosition(currentPosition);

        Debug.Log("Desired Position: " + desiredPosition); 
        
        transform.DOMove(desiredPosition, moveSpeed); 
    }

    public void TakeHealth(int amount)
    {
        playerHealth -= amount; 
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerHealth);
            stream.SendNext(playerIndex);
        }
        else
        {
            this.playerHealth = (int) stream.ReceiveNext();
            this.playerIndex = (int) stream.ReceiveNext();
        }
    }
}
