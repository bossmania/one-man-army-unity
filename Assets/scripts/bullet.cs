using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public float death_timer;
    public Transform trans;
    public float damage;
    public float spd;
    public string state;
    public Collider2D col;

    void Start() {
        trans.position += new Vector3(0,0,-8);
    }

    void Update() {
        if (state != "null") {
            Invoke(state, 0f);
        }
    }

    void moveState() {
        trans.position += trans.up * Time.deltaTime * spd;
        col.enabled = true;
    }

    void freezeState() {
        col.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (state != "freezeState") {
            string tag = col.gameObject.tag;
            if (tag != "bullet") {
                col.gameObject.SendMessage("takeDamage", damage);
                Destroy(gameObject);
            }
        }
    }
}
