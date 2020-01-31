using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{

    public float moveSpeed;

    private Rigidbody2D _rb;
    private ObjectController objScript;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        // fuel = maxFuel;
    }

    private void FixedUpdate()
    {
        if(true)
            Movement();
        // if (canFly)
        //     Fly();
    }

    private void Movement()
    {
        float xAxis = 0;
        float yAxis = 0;
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(xAxis, yAxis);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Caja") {
            Debug.Log("Entras collision a " + collision.gameObject.name);
            collision.gameObject.GetComponent<ObjectController>().isPicked = true;
        }

        if (collision.gameObject.tag == "Zona") {
            collision.gameObject.GetComponent<ObjectController>().inZone = true;
        }
    }

}