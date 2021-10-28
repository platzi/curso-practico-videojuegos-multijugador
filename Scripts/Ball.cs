using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class Ball : NetworkBehaviour
{
    public float speed = 30;
    public Rigidbody2D rb;

    IEnumerator StartBall() {
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        yield return new WaitForSeconds(2);
        rb.simulated = true;
        float direction = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        rb.velocity = Vector2.right * speed * direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartBall());
        textScore = FindObjectOfType<TMP_Text>();

        networkManager = FindObjectOfType<NetworkManagerPong>();
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

    void Update(){

        if (transform.position.x > networkManager.rightRacketSpawn.position.x){
            leftScore ++;
            StartCoroutine(StartBall());
            UpdateTextScore(leftScore, rightScore);
        }
        if (transform.position.x < networkManager.leftRacketSpawn.position.x){
            rightScore ++;
            StartCoroutine(StartBall());
            UpdateTextScore(leftScore, rightScore);
        }

    }


    void UpdateTextScore(int leftScore, int rightScore){
        textScore.text = $"{leftScore} - {rightScore}";
    }
}
