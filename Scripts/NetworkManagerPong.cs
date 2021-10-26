using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerPong : NetworkManager
{
    public Transform leftRacketSpawn;
    public Transform rightRacketSpawn;

    public GameObject ballPrefab;
    public GameObject ball;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPosition = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
        GameObject player = Instantiate(playerPrefab, startPosition.position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);

        if (numPlayers == 2){
            ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(ball);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (ball != null)
        {
            NetworkServer.Destroy(ball);
        }

        base.OnServerDisconnect(conn);
    }
}
