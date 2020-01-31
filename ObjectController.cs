using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public Transform playerT;
    public bool isPicked, inZone, placed;
 
     private void Awake()
     {
        isPicked = inZone = placed = false;
     }

    public void Update(){
        if (Input.GetMouseButtonDown(1) && this.isPicked == true){
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

        Debug.Log("Colocao" + this.inZone);

        // Llama en GameController a ObjectPlaced
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Caja triggera" + collision.gameObject);
        if (collision.gameObject.tag == "Zona") {
            this.inZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Zona") {
            this.inZone = false;
        }
    }

}
