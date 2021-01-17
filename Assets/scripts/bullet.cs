using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public float death_timer;
    public float death_countdown;
    public Transform trans;
    public float spd = 0.25f;
    public float damage;
    public string state;
    public CircleCollider2D col;
    public Rigidbody2D rb;
    public bool applyForce = false;

    void Start() {
        death_countdown = death_timer;
        col.radius = 1 + (spd * 1.25f);
    }

    void Update() {
        if (state != "null") {
            Invoke(state, 0f);
        }
        if (death_countdown < 0) {
            Destroy(gameObject);
        }
    }

    void moveState() {
        trans.position += (trans.up*spd);
        col.enabled = true;
        death_countdown -= Time.deltaTime;
    }

    void freezeState() {
        col.enabled = false;
        rb.velocity = new Vector2(0, 0);
        applyForce = false;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag != "bullet") {
            col.gameObject.SendMessage("takeDamage", damage);
            Destroy(gameObject);
        }
    }
}
