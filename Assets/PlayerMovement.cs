using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;

    public float speed = 10.0f;
    public float jumpHeight = 10.0f;
    private bool isJumping = false;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var vel = rb2d.velocity;
        if (Input.GetKey(moveLeft))
        {
            vel.x = -speed;
        }
        else if (Input.GetKey(moveRight))
        {
            vel.x = speed;
        }
        else
        {
            vel.x = 0;
        }

        rb2d.velocity = vel;


        if (Input.GetKeyDown(moveUp) && !isJumping) // both conditions can be in the same branch
        {
            Debug.Log("jump");
            rb2d.AddForce(Vector2.up * jumpHeight); // you need a reference to the RigidBody2D component
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Platform") // GameObject is a type, gameObject is the property
        {
            isJumping = false;
        }


    }
}