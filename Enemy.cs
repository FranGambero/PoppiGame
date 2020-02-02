using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float direction = 1;
    public float velocity = 5f;
    private GrabController grabController;

    private void Start() {
        grabController = FindObjectOfType<GrabController>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector2.right * direction * velocity * Time.deltaTime);
        if(transform.position.x > 4.5f || transform.position.x < -4.5f) {
            this.direction *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            grabController.Drop();
        }
    }

}
