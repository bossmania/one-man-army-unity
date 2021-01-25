using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gernade_obj : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform trans;
    public float death_timer;
    public float death_countdown;
    public string state;
    public Vector2 velocity;
    public GameObject owner;
    void Awake()
    {
        death_countdown = death_timer;
        rb.AddForce(trans.right*100, ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update()
    {
        if (state != "null") {
            Invoke(state, 0f);
        }
    }

    void moveState() {
        death_countdown -= Time.deltaTime;
        velocity = rb.velocity;
        if (death_countdown < 0) {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy")) {
                if (enemy != owner) {
                    Transform enemy_trans = enemy.GetComponent<Transform>();
                    Vector3 dis = (trans.position - enemy_trans.position);
                    dis.z = 0;
                    if (dis.magnitude < 3) {
                        enemy.SendMessage("takeDamage", 70);
                        //insert explosion here.
                    }
                }
            }
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
                if (player != owner) {
                    Transform player_trans = player.GetComponent<Transform>();
                    Vector3 dis = (trans.position - player_trans.position);
                    dis.z = 0;
                    if (dis.magnitude < 3) {
                        player.SendMessage("takeDamage", 70);
                        //insert explosion here.
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    void freezeState() {
        rb.velocity = new Vector2();
    }

    void unfreezeState() {
        if (velocity == Vector2.zero) {
            rb.AddForce(trans.right*100, ForceMode2D.Force);
        } else {
            rb.velocity = velocity;
        }
        state = "moveState";
    }
}
