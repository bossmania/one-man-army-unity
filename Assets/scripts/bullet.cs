using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public float death_timer;
    public float death_countdown;
    public Transform trans;
    public float damage;
    public string state;
    public Collider2D col;
    public Rigidbody2D rb;
    public bool applyForce = false;

    void Start() {
        death_countdown = death_timer;
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
        if (applyForce == false) {
            rb.AddForce(trans.up*0.3f, ForceMode2D.Force);
            applyForce = true;
        }
        RaycastHit2D hit = Physics2D.Raycast(trans.position, trans.TransformDirection(Vector2.up), 8f);
        if (hit.collider != null) {
            hit.transform.gameObject.SendMessage("takeDamage", damage);
            Destroy(gameObject);
        }
        col.enabled = true;
        death_countdown -= Time.deltaTime;
    }

    void freezeState() {
        col.enabled = false;
        rb.velocity = new Vector2(0, 0);
        applyForce = false;
    }
}
