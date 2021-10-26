using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Ball : NetworkBehaviour
{
    public float speed = 30;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.simulated = true;
        rb.velocity = Vector2.left * speed;
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
}
