using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator_arrow : MonoBehaviour
{
    public Transform trans;
    public Rigidbody2D rb;
    public SpriteRenderer arrow_spr;
    public Transform player_trans;
    public Transform elevator_trans;

    void Start()
    {
        player_trans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        foreach (GameObject elevators in GameObject.FindGameObjectsWithTag("elevator")) {
          elevator elevator_scr = elevators.GetComponent<elevator>();
          if (elevator_scr.type == "exit") {
            elevator_trans = elevators.GetComponent<Transform>();
          }
        }
    }

    // Update is called once per frame
    void Update()
    {
        trans.position = new Vector3(player_trans.position.x, player_trans.position.y, -52f);
        Vector2 lookdir = elevator_trans.position - trans.position;
        rb.rotation = (Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg) - 90;

        if (Vector2.Distance(trans.position, elevator_trans.position) < 10) {
          arrow_spr.enabled = false;
        } else {
          arrow_spr.enabled = true;
        }
    }
}
