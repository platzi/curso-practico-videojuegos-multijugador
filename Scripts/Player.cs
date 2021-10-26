using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;

    void FixedUpdate(){
        rb.velocity = new Vector2(0,Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
    }
}
