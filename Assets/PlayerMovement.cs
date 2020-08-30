// its on line 69

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
    private bool isDecel = true;
    private Rigidbody2D rb2d;
    private BoxCollider2D boxCollider2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
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
        }
        else if (Input.GetKey(moveRight))
        {
            vel.x = speed;
        }
        else if (isDecel)
        {
            //Debug.Log("decellerate");
            vel.x = vel.x * decelVar;
        }

        rb2d.velocity = vel;

        //if (rb2d.velocity.y > 0.001 && rb2d.velocity.y < -0.001)
        //{
        //    inAir = true;
        //}

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
        else if(dashCooldown >= 0)
        {
            dashCooldown -= Time.deltaTime;
            //Debug.Log(dashCooldown);
            inDash = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.GetContact(0).point.y + " contact point 0");
        Debug.Log(collision.GetContact(1).point.y + " contact point 1");
        Debug.Log(collision.GetContact(2).point.y + " contact point 2");
        Debug.Log(rb2d.position.y + " player centrepoint");
        if (collision.gameObject.tag == "Platform" && collision.GetContact(0).point.y < rb2d.position.y) 
        {
            //Debug.Log("On Platform");
            inAir = false;
            hasDoubleJump = true;
            isDecel = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            Debug.Log("exit platform collision");
            inAir = true;
            isDecel = false;
        }
    }
}