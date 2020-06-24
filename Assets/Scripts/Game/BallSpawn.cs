using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BallSpawn : MonoBehaviour
{
    public GameObject ballPrefab;
    public float ballStartForce = 10000;
    
    private Transform cachedTransform;

    public Ball Spawn()
    {
        if (!cachedTransform)
        {
            cachedTransform = transform; 
        }

        GameObject newBall = PhotonNetwork.Instantiate(ballPrefab.name, cachedTransform.position, cachedTransform.rotation);
        Rigidbody ballRb = newBall.GetComponent<Rigidbody>(); 
        ballRb.AddForce(cachedTransform.forward * ballStartForce);

        return newBall.GetComponent<Ball>(); 
    }
}
