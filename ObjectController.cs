using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public Transform playerT;
    public GameObject objective;
    public bool isPicked, inZone, placed;
    private Rigidbody2D myRb;
    private Quaternion originalRotation;

    private void Awake()
     {
        isPicked = inZone = placed = false;
        this.myRb = this.gameObject.GetComponent<Rigidbody2D>();
        Debug.Log("Tengo: " + this.myRb);
        this.originalRotation = this.transform.rotation;

        this.letsPlay();
    }

    public void Update(){
        if (Input.GetMouseButtonDown(1) && this.isPicked){
            this.PickObject();
        } else if (this.inZone) {
            Debug.Log("Lo has puesto");
            this.PlaceObject();
        } else if (Input.GetMouseButtonUp(1)) {
            this.sueltaObjecto();
        }
    }

    public void PickObject(){
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        this.transform.position = playerT.position;
        this.transform.parent = GameObject.Find("Hand").transform;
    }

    public void sueltaObjecto(){
        this.transform.parent = null;
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<BoxCollider2D>().enabled = true;

        this.isPicked = false;
    }


    public void PlaceObject() {
        this.transform.parent = null;
        GetComponent<Rigidbody2D>().simulated = false;
        transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * 180f);        //180 f es rotationspeed
        //this.placed = true;

        Debug.Log("Colocao" + this.inZone);

    }

    public void letsPlay() {
        this.myRb.AddTorque(50f);
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        this.myRb.AddForce(direction * 250f);

        Debug.Log("Er vector " + direction + "de " + this.gameObject);
    }

}
