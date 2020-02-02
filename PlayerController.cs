using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float rotateSpeed;
    public float rechargeSpeed;
    public float jetForce;
    public ParticleSystem jetFireParticle;
    public float maxEnergy;
    private float energy;
    public TrailRenderer trailSystem;
    private GameController gameController;

    public Image heatImage;

    public bool onSpace = false;
    public bool canFly = false;
    private bool noEnergy = false;
    private bool onGround = false;
    public bool canMove = true;
    private bool flying = false;

    public float outAnimationTime;
    public float firstStartAnimationTime;
    public float startAnimationTime;

    private Rigidbody2D _rb;
    private AudioManager audioManagerScript;
    private bool lowFuelFlag = false, jetPackPlaying = false;
    private bool _facingRight = true;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        if (jetFireParticle)
            jetFireParticle.Stop();
        energy = maxEnergy;
        gameController = FindObjectOfType<GameController>();
        audioManagerScript = FindObjectOfType<AudioManager>();
    }

    private void Update() {
        checkSpace();
        //jetParticleController();
        checkFly();
        trailController();
        if (heatImage)
            heatImage.fillAmount = energy / maxEnergy;

        float fuelLeft = energy / maxEnergy; ;
        heatImage.fillAmount = fuelLeft;
        if (fuelLeft < 0.3f && !lowFuelFlag) {
            lowFuelFlag = true;
            audioManagerScript.playDanger();
        } else if (energy == maxEnergy && lowFuelFlag) {
            lowFuelFlag = false;
            audioManagerScript.stopDanger();
        }
    }

    private void FixedUpdate() {
        if (canMove)
            Movement();
        if (canFly)
            Fly();
    }

    //private void Movement() {
    //    float xAxis = 0;
    //    xAxis = Input.GetAxis("Horizontal");
    //    if (!onSpace) {
    //        //en tierra se mueve con axisX
    //        Vector2 movement = new Vector2(xAxis, 0);
    //        transform.Translate(movement * moveSpeed * Time.deltaTime);
    //    } else {
    //        //en el espacio rota segun axisX
    //        Vector3 rotation = new Vector3(0, 0, xAxis);
    //        transform.Rotate(rotation * rotateSpeed * Time.deltaTime);
    //    }
    //}
    private void Movement() {
        float xAxis = 0;
        xAxis = Input.GetAxis("Horizontal");
        if (!onSpace) {
            if (xAxis > 0 && !_facingRight) {
                Flip();
            }
            if (xAxis < 0 && _facingRight) {
                Flip();
            }
            //en tierra se mueve con axisX
            Vector2 movement = new Vector2(Mathf.Abs(xAxis), 0);
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        } else {
            //en el espacio rota segun axisX
            Vector3 rotation = new Vector3(0, 0, xAxis);
            transform.Rotate(rotation * rotateSpeed * Time.deltaTime);
        }
    }

    private void Flip() {
        _facingRight = !_facingRight;
        transform.Rotate(new Vector3(0, 180, 0), Space.World);
    }
    private void checkFly() {
        //if noEnergy --> cant fly
        //if onGround --> increaseEnergy

        if (energy <= 0) {
            noEnergy = true;
            flying = false;
            canFly = false;
        }
        if (!onSpace && onGround)
            increaseEnergy(rechargeSpeed);
        if (noEnergy && energy >= maxEnergy)
            noEnergy = false;
        if (energy > 0 && onGround) {
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

    internal void LaunchAnimDie() {
        GetComponent<Animator>().Play("Die");
    }
    internal void LaunchAnimStart(bool firstLevel) {
        if (firstLevel) {
            GetComponent<Animator>().Play("StartFirstScene");
        } else {
            Debug.Log("Concha, entro");
            GetComponent<Animator>().Play("StartScene");
        }
    }

    private void reduceEnergy(float value) {
        if (energy > 0)
            energy -= value * Time.deltaTime;
        else if (energy < 0)
            energy = 0;
    }

    private void Fly() {
        flying = Input.GetKey(KeyCode.Space);
        if (flying) {
            if (!onSpace) {
                if (!jetPackPlaying) {
                    audioManagerScript.playJetPack();
                    jetPackPlaying = true;
                }
                _rb.AddForce(Vector2.up * jetForce);
                reduceEnergy(10f);
            } else {
                _rb.AddForce(transform.rotation * Vector2.up * jetForce);
            }
        }
        if (onSpace && !flying) {
            increaseEnergy(rechargeSpeed);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            onGround = true;
        }
    }
    //private void OnCollisionStay2D(Collision2D collision) {
    //    if (collision.gameObject.tag == "Caja") {
    //        if (collision.gameObject.GetComponent<ObjectController>().GetNotMovedRecently()) {
    //            onGround = true;
    //        }
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ojete") {
            this.transform.position = collision.gameObject.transform.position;
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            gameController.GameEnded();
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            onGround = false;
        }
    }

    private void jetParticleController() {
        if (Input.GetKey(KeyCode.Space) && !noEnergy && !jetFireParticle.isPlaying) {
            if (jetFireParticle) {
                jetFireParticle.Play();
            }
        } else if (Input.GetKeyUp(KeyCode.Space) || noEnergy) {
            if (jetFireParticle) {
                jetFireParticle.Stop();
                jetPackPlaying = false;
                audioManagerScript.stopJetPack();
            }
        }
    }

    internal void OnLevelEnded() {
        canMove = false;
        canFly = false;
        GetComponent<Rigidbody2D>().simulated = false;
        // GetComponent<Collider2D>().enabled = false;
    }
    internal void OnLevelStart() {
        canMove = true;
        canFly = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
        LaunchAnimStart(FindObjectOfType<GameController>().level == 1);
    }
    private void trailController() {
        if (flying) {
            trailSystem.emitting = true;
        }
        if (!flying) {
            trailSystem.emitting = false;
            jetPackPlaying = false;
            audioManagerScript.stopJetPack();
        }
    }
}
