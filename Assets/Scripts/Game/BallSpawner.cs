using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public BallSpawn[] spawnPoints;
    public int targetBallsOnArena = 1;
    public float targetTimeBetweenBalls = 5f;
    
    private int amountBallsInPlay = 0;
    private float lastSpawnedTime;
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return; //Only the host should be spawning balls
        if (!DoesNeedMoreBalls()) return;

        if (Time.time >= (lastSpawnedTime + targetTimeBetweenBalls))
        {
            lastSpawnedTime = Time.time; 
            SpawnBall();   
        }
    }

    private void SpawnBall()
    {
        Ball ball = GetRandomSpawnPoint().Spawn();
        ball.spawner = this;
        amountBallsInPlay++;
    }

    public void RemoveBallInPlay()
    {
        amountBallsInPlay--; 
    }

    private bool DoesNeedMoreBalls() => amountBallsInPlay < targetBallsOnArena;

    private BallSpawn GetRandomSpawnPoint()
    {
        int randNum = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randNum]; 
    }
}
