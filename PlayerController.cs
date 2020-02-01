using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float rotateSpeed;
    public float maxEnergy;
    public float rechargeSpeed;
    public float jetForce;
    public ParticleSystem jetFireParticle;

    public float energy;
    public bool onSpace = false;
    public bool canFly = false;
    private bool noEnergy = false;
    private bool onGround = false;
    public bool canMove = true;
    private bool flying = false;

    public float outAnimationTime;

    private Rigidbody2D _rb;


    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        if (jetFireParticle)
            jetFireParticle.Stop();
        energy = maxEnergy;
    }

    private void Update() {
        checkSpace();
        jetParticleController();
        checkFly();
    }

    private void FixedUpdate() {
        if (canMove)
            Movement();
        if (canFly)
            Fly();
    }

    private void Movement() {
        float xAxis = 0;
        xAxis = Input.GetAxis("Horizontal");
        if (!onSpace) {
            //en tierra se mueve con axisX
            Vector2 movement = new Vector2(xAxis, 0);
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        } else {
            //en el espacio rota segun axisX
            Vector3 rotation = new Vector3(0, 0, xAxis);
            transform.Rotate(rotation * rotateSpeed * Time.deltaTime);
        }
    }

    private void checkFly() {
        //if noEnergy --> cant fly
        //if onGround --> increaseEnergy

        if (energy <= 0) {
            noEnergy = true;
            canFly = false;
        }
        if (!onSpace && onGround)
            increaseEnergy(rechargeSpeed);
        if (noEnergy && energy >= maxEnergy)
            noEnergy = false;
        if (!noEnergy && onGround) {
            canFly = true;
        }
    }

    private void checkSpace() {
        if (onSpace) {
            _rb.freezeRotation = false;
            _rb.gravityScale = 0;
        } else {
            _rb.freezeRotation = true;
            _rb.gravityScale = 1;
        }
    }

    private void increaseEnergy(float value) {
        if (energy < maxEnergy)
            energy += value * Time.deltaTime;
        else if (energy > maxEnergy)
            energy = maxEnergy;
    }

    private void reduceEnergy(float value) {
        if (energy > 0)
            energy -= value * Time.deltaTime;
        else if (energy < 0)
            energy = 0;
    }

    private void Fly() {
        flying = Input.GetMouseButton(0);
        if (flying) {
            if (!onSpace)
                _rb.AddForce(Vector2.up * jetForce);
            else
                _rb.AddForce(transform.rotation * Vector2.up * jetForce);
            reduceEnergy(10f);
        }
        if (onSpace && !flying)
            increaseEnergy(rechargeSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            onGround = false;
        }
    }

    private void jetParticleController() {
        if (Input.GetMouseButton(0) && !noEnergy && !jetFireParticle.isPlaying) {
            if (jetFireParticle) {
                jetFireParticle.Play();
            }
        } else if (Input.GetMouseButtonUp(0) || noEnergy) {
            if (jetFireParticle) {
                jetFireParticle.Stop();
            }
        }
    }

    internal void OnLevelEnded() {
        canMove = false;
        canFly = false;
    }
    internal void OnLevelStart() {
        canMove = true;
        canFly = true;
    }
}
