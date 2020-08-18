﻿// its on line 69

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;

    public float speed = 10.0f;
    public float jumpHeight = 10.0f;
    public float decelVar = 0.98f;
    public float dashMagnitude = 10.0f;
    public float fixedDashTime = 1.0f;
    public float fixedDashCooldown = 1.0f;
    private float dashCooldown;
    private float dashTime;
    private bool inDash = false;
    private float dashAngle;
    private Vector3 mousePos;
    private bool inAir = false;
    private bool hasDoubleJump = true;
    private bool onPlatform = false;
    private bool isDecel = true;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        dashTime = fixedDashTime;
        dashCooldown = fixedDashCooldown;
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
        else if (isDecel)
        {
            Debug.Log("decellerate");
            vel.x = vel.x * decelVar;
        }

        rb2d.velocity = vel;


        if (Input.GetKeyDown(moveUp) && !inAir) // both conditions can be in the same branch
        {
            Debug.Log("jump");
            Debug.Log(onPlatform);
            rb2d.AddForce(Vector2.up * jumpHeight);
            inAir = true;
        }
        else if (Input.GetKeyDown(moveUp) && onPlatform && hasDoubleJump)
        {
            Debug.Log("double jump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(new Vector2(0, jumpHeight));
            hasDoubleJump = false;
        }

        if (rb2d.velocity.y > 0.001 && rb2d.velocity.y < -0.001)
        {
            inAir = true;
            onPlatform = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            inDash = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Dash");
            Vector2 Direction = new Vector2(rb2d.position.x - mousePos.x, rb2d.position.y - mousePos.y);
            dashAngle = Mathf.Atan2(Direction.y, Direction.x) + Mathf.PI;
        }

        if (inDash && dashCooldown <= 0) 
        { 
            if(dashTime >= 0.0f)
            {
                dashTime -= Time.deltaTime;
                isDecel = false;
                rb2d.velocity = new Vector2(dashMagnitude * Mathf.Cos(dashAngle), dashMagnitude * Mathf.Sin(dashAngle));
            }
            else
            {
                dashTime = fixedDashTime;
                dashCooldown = fixedDashCooldown;
                inDash = false;
            }

            //Debug.Log(Direction);        
        }
        else
        {
            dashCooldown -= Time.deltaTime;
            inDash = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Platform") // GameObject is a type, gameObject is the property
        {
            inAir = false;
            hasDoubleJump = true;
            onPlatform = true;
            isDecel = true;
        }

    }
}