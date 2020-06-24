using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public BallSpawner spawner;
    private Rigidbody rigidbody;
    
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void ApplyBounceForce(Vector3 otherPosition)
    {
        if (!rigidbody) return;

        Vector3 heading = otherPosition - transform.position;
        rigidbody.AddForce((heading * -1) * 500);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Killbox"))
        {
            spawner.RemoveBallInPlay();
            PhotonNetwork.Destroy(gameObject);
        }

        if (other.collider.CompareTag("Player"))
        {
            ApplyBounceForce(other.transform.position);
        }
    }

    private void OnDestroy()
    {
        spawner.RemoveBallInPlay();
    }
}
