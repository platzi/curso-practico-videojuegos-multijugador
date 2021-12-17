using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class Ball : NetworkBehaviour
{
    public float speed = 30;
    public Rigidbody2D rb;

    IEnumerator StartBall(float delay)
    {
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        yield return new WaitForSeconds(delay);
        RpcDisableWinState();
        yield return new WaitForSeconds(2);
        rb.simulated = true;
        float direction = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        rb.velocity = Vector2.right * speed * direction;
    }

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        StartCoroutine(StartBall(0));
        networkManager = FindObjectOfType<NetworkManagerPong>();
    }

    void Start()
    {
        textScore = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        winnerScore = GameObject.Find("WinnerPlayerText").GetComponent<TMP_Text>();

        winnerScore.gameObject.SetActive(false);
    }

    float HitFactor(Vector2 ballPosition, Vector2 racketPosition, float racketHeight)
    {
        return (ballPosition.y - racketPosition.y) / racketHeight;
    }

    // Update is called once per frame
    [ServerCallback]
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.GetComponent<Player>())
        {
            rb.velocity = Vector2.Reflect(rb.velocity, col.contacts[0].normal).normalized * speed;
            float y = HitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
            float x = col.relativeVelocity.x > 0 ? 1 : -1;

            Vector2 dir = new Vector2(x, y).normalized;
            rb.velocity = dir * speed;
        }
    }

    NetworkManagerPong networkManager;
    public int leftScore;
    public int rightScore;
    public TMP_Text textScore;
    public TMP_Text winnerScore;

    [ServerCallback]
    void Update()
    {
        int delay = 0;
        if (transform.position.x > networkManager.rightRacketSpawn.position.x)
        {
            leftScore++;
            print(leftScore);
            if (leftScore >= 2)
            {
                RpcWinState("Izquierdo");
                delay = 5;
                leftScore = 0;
                rightScore = 0;
            }
            StartCoroutine(StartBall(delay));
            RpcUpdateTextScore(leftScore, rightScore);
        }

        if (transform.position.x < networkManager.leftRacketSpawn.position.x)
        {
            rightScore++;
            print(rightScore);
            if (rightScore >= 2)
            {
                RpcWinState("Derecho");
                delay = 5;
                leftScore = 0;
                rightScore = 0;
            }
            StartCoroutine(StartBall(delay));
            RpcUpdateTextScore(leftScore, rightScore);
        }
    }

    [ClientRpc]
    public void RpcWinState(string winner)
    {
        winnerScore.gameObject.SetActive(true);
        winnerScore.text = $"Ha ganado el jugador del lado <b>{winner}</b>";
    }

    [ClientRpc]
    public void RpcDisableWinState()
    {
        winnerScore.gameObject.SetActive(false);
    }

    [ClientRpc]
    public void RpcUpdateTextScore(int leftScore, int rightScore)
    {
        textScore.text = $"{leftScore} - {rightScore}";
    }
}
