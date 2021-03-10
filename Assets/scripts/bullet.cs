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
    public float rad;

    void Start() {
        death_countdown = death_timer;
        rad = spd + 1.3f;
    }

    void Update() {
        if (state != "null") {
            Invoke(state, 0f);
        }
        if (death_countdown < 0) {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos() {
      Gizmos.DrawWireSphere(transform.position, rad);
    }

    void moveState() {
        trans.position += (trans.up*spd);
        col.enabled = true;
        death_countdown -= Time.deltaTime;
    }

    void freezeState() {
        col.enabled = false;
        rb.velocity = new Vector2(0, 0);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "bullet") {
            other.gameObject.SendMessage("takeDamage", damage);
            Destroy(gameObject);
        }
    }
}
