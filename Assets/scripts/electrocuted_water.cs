using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electrocuted_water : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col) {
        col.gameObject.SendMessage("shockStart");
    }

    void OnTriggerExit2D(Collider2D col) {
        col.gameObject.SendMessage("shockEnd");
    }
}
