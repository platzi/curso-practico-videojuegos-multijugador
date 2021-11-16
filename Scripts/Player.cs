using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    [SyncVar(hook = nameof(SetColor))]
    Color playerColor = Color.white;

    Dictionary<KeyCode, Color> colors = new Dictionary<KeyCode, Color>(){
        {KeyCode.Alpha1, Color.white},
        {KeyCode.Alpha2, Color.red},
        {KeyCode.Alpha3, Color.green},
        {KeyCode.Alpha4, Color.gray},
        {KeyCode.Alpha5, Color.blue},
        {KeyCode.Alpha6, Color.yellow}
    };

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        foreach (var keycode in colors.Keys)
        {
            if (Input.GetKeyDown(keycode))
            {
                CmdChangeColor(colors[keycode]);
            }
        }

    }

    [Command]
    public void CmdChangeColor(Color newColor)
    {
        playerColor = newColor;
    }

    void SetColor(Color oldColor, Color newColor)
    {
        sr.color = newColor;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        rb.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
    }
}
