using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    private Quaternion originalRotation;
    public GameObject originalTransform;
    public bool placed = false;
    private Rigidbody2D myRb;

    private void Awake() {
        originalRotation = transform.rotation;
        this.myRb = this.gameObject.GetComponent<Rigidbody2D>();

        this.letsPlay();
    }
    public Quaternion getOriginalRotation() {
        return originalRotation;
    }

    public Transform getOriginalTransform() {
        return originalTransform.transform;
    }

    public void letsPlay() {
        this.myRb.AddTorque(50f);
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        this.myRb.AddForce(direction * 250f);

        Debug.Log("Er vector " + direction + "de " + this.gameObject);
    }
}