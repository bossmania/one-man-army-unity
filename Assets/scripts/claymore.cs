using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class claymore : MonoBehaviour {

    public EdgeCollider2D col;
    player player_scr;

    void Start() {
        player_scr = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (player_scr.is_freeze == false && (col.tag == "Player" || col.tag == "enemy")) {
            col.gameObject.SendMessage("takeDamage", 70);
        }
        // insert explosion here.
        Destroy(gameObject);
    }
}
