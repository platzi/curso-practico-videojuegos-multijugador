using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;

    void FixedUpdate(){
        if (!isLocalPlayer)
        {
            return;
        }

        rb.velocity = new Vector2(0,Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
    }
}
