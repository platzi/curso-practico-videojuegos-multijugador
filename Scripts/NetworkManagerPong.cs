using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerPong : NetworkManager
{
    public Transform leftRacketSpawn;
    public Transform rightRacketSpawn;

    public GameObject ballPrefab;
    public Ball ball;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPosition = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
        GameObject player = Instantiate(playerPrefab, startPosition.position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);

        if (numPlayers == 2){
            var tempBall = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(tempBall);
            ball = tempBall.GetComponent<Ball>();
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (ball != null)
        {
            ball.textScore.text = "0 - 0";
            NetworkServer.Destroy(ball.gameObject);
        }

        base.OnServerDisconnect(conn);
    }
}
