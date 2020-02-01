using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float maxHeat;
    public float coolingSpeed;
    public float jetForce;
    public ParticleSystem jetFireParticle;
    public float heat;
    public float outAnimationTime;

    private bool canFly = false;
    private bool overheated = false;
    private bool onGround = false;
    private bool canMove = true;
    private bool flying = false;

    public Collider2D[] objsTotal;

    private Rigidbody2D _rb;


    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        if (jetFireParticle)
            jetFireParticle.Stop();
        heat = 0;
    }

    private void Update() {
        jetParticleController();
        checkFly();

        this.Boom();
        if (this.objsTotal.Length >= 2) {
            Debug.Log("Obks> " + this.objsTotal[1]);

        }

    }

    private void FixedUpdate() {
        if (canMove)
            Movement();
        if (canFly)
            Fly();
    }

    internal void OnLevelEnded() {
        canMove = false;
        canFly = false;
    }
    internal void OnLevelStart() {
        canMove = true;
        canFly = true;
    }

    private void Movement() {
        float xAxis = 0;
        xAxis = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(xAxis, 0);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    private void checkFly() {
        if (heat >= maxHeat) {
            canFly = false;
            overheated = true;
        }
        if (overheated && heat > 0 && onGround) {
            reduceHeat(coolingSpeed);
        } else if (overheated && heat <= 0) {
            heat = 0;
            overheated = false;
        }

        if (!overheated && onGround) {
            canFly = true;
        }
    }

    private void increaseHeat(float value) {
        heat += value * Time.deltaTime;
    }

    private void reduceHeat(float value) {
        heat -= value * Time.deltaTime;
    }

    private void Fly() {
        flying = Input.GetKey(KeyCode.Space);
        if (flying) {
            _rb.AddForce(Vector2.up * jetForce);
            increaseHeat(10f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            onGround = true;
        }

        if (this.objsTotal.Length > 1) {
            if (collision.gameObject.tag == "Caja") {
                //Debug.Log("Entras collision a " + collision.gameObject.name);
                //ObjectController result = objsTotal.Find(x => x.GetId() == "xy");
                List<Collider2D> listAux = new List<Collider2D>();
                listAux.AddRange(objsTotal);
                GameObject cubo = listAux.Find(o => o.GetComponent<ObjectController>() != null).gameObject;
                //GameObject cubo = this.objsTotal[0].gameObject;
                cubo.GetComponent<ObjectController>().isPicked = true;
                //collision.gameObject.GetComponent<ObjectController>().isPicked = true;

                //this.isGrabbing() = true;
            }

            if (collision.gameObject.tag == "Zona") {
                Debug.Log("ESTOY EN ZONA");
                collision.gameObject.GetComponent<ObjectController>().inZone = true;

                //this.isGrabbing() = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            onGround = false;
        }
    }

    public void Boom() {
        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, .75f);
        this.objsTotal = objs;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, .75f);
    }

    private void jetParticleController() {
        if (Input.GetKey(KeyCode.Space) && !overheated && !jetFireParticle.isPlaying) {
            if (jetFireParticle) {
                jetFireParticle.Play();
            }
        } else if (Input.GetKeyUp(KeyCode.Space) || overheated) {
            if (jetFireParticle) {
                jetFireParticle.Stop();
            }
        }
    }
}
