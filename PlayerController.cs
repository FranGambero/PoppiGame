using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float maxHeat;
    public float coolingSpeed;
    public float jetForce;
    public ParticleSystem jetFireParticle;
    public float heat;

    private bool canFly = false;
    private bool overheated = false;
    private bool onGround = false;
    private bool canMove = true;
    private bool flying = false;

    private Rigidbody2D _rb;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if(jetFireParticle)
            jetFireParticle.Stop();
        heat = 0;
    }

    private void Update()
    {
        jetParticleController();
        checkFly();
    }

    private void FixedUpdate()
    {
        if(canMove)
            Movement();
        if (canFly)
            Fly();
    }

    private void Movement()
    {
        float xAxis = 0;
        xAxis = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(xAxis,0);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    private void checkFly()
    {
        if(heat >= maxHeat)
        {
            canFly = false;
            overheated = true;
        }
        if (overheated && heat > 0 && onGround)
        {
            reduceHeat(coolingSpeed);
        }else if(overheated && heat <= 0)
        {
            heat = 0;
            overheated = false;
        }

        if(!overheated && onGround)
        {
            canFly = true;
        }
    }

    private void increaseHeat(float value)
    {
        heat += value * Time.deltaTime;
    }

    private void reduceHeat(float value)
    {
        heat -= value * Time.deltaTime;
    }

    private void Fly()
    {
        flying = Input.GetKey(KeyCode.Space);
        if (flying)
        {
            _rb.AddForce(Vector2.up * jetForce);
            increaseHeat(10f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }

    private void jetParticleController()
    {
        if (Input.GetKey(KeyCode.Space) && !overheated && !jetFireParticle.isPlaying)
        {
            if (jetFireParticle)
            {
                jetFireParticle.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space) || overheated)
        {
            if (jetFireParticle)
            {
                jetFireParticle.Stop();
            }
        }
    }
}
