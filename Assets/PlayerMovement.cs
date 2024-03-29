﻿// its on line 69

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;

    public float speed = 10.0f;
    public float jumpHeight = 10.0f;
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
    private Rigidbody2D rb2d;
    private BoxCollider2D boxCollider2d;
    private EdgeCollider2D edgeCollider2d;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        edgeCollider2d = GetComponent<EdgeCollider2D>();
        _anim = GetComponent<Animator>();
        dashTime = fixedDashTime;

        dashCooldown = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        var vel = rb2d.velocity;
        if (Input.GetKey(moveLeft))
        {
            vel.x = -speed;
            transform.localScale = new Vector3(-0.09506838f, 0.1882557f, 1);
        }
        else if (Input.GetKey(moveRight))
        {
            vel.x = speed;
            transform.localScale = new Vector3(0.09506838f, 0.1882557f, 1);
        }
        else
        {
            vel.x = 0;
        }

        animator.SetFloat("Speed", Mathf.Abs(vel.x));

        rb2d.velocity = vel;

        if (Input.GetKeyDown(moveUp) && !inAir) 
        {
            //Debug.Log("jump");
            //Debug.Log(inAir);
            rb2d.AddForce(Vector2.up * jumpHeight);
            inAir = true;
        }
        else if (Input.GetKeyDown(moveUp) && inAir && hasDoubleJump)
        {
            //Debug.Log("double jump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(new Vector2(0, jumpHeight));
            hasDoubleJump = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            inDash = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("Dash");
            Vector2 Direction = new Vector2(rb2d.position.x - mousePos.x, rb2d.position.y - mousePos.y);
            dashAngle = Mathf.Atan2(Direction.y, Direction.x) + Mathf.PI;
        }

        if (inDash && dashCooldown <= 0) 
        { 
            if(dashTime >= 0.0f)
            {
                dashTime -= Time.deltaTime;
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
        else if(dashCooldown >= 0)
        {
            dashCooldown -= Time.deltaTime;
            //Debug.Log(dashCooldown);
            inDash = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inAir = false;
        hasDoubleJump = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit trigger collision");
        inAir = true;
    }

}