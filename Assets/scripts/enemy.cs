using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class enemy : MonoBehaviour
{
    public Transform trans;
    public float max_hp = 100f;
    public float hp;
    public health_bar hp_bar;
    public string state;
    public Rigidbody2D rb;
    public Transform gun;
    public gun_scr gun_scr;
    public Transform target;
    public AIDestinationSetter desSetter;
    public AIPath aipath;
    public Transform follow_me;
    public Transform player;
    public player player_scr;
    public float chaseDis = 8f;


    // Start is called before the first frame update
    void Start()
    {
        hp = max_hp;
        hp_bar.set_max_hp(max_hp);

        if (state == "wanderState") {
            float xx = Random.Range(-16.74f, 14.38f);
            float yy = Random.Range(-9.16f, 6.57f);
            follow_me.position = new Vector2(xx, yy);
            target = follow_me;
            aipath.endReachedDistance = 1f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (state != "null") {
            Invoke(state, 0f);
        }

        if (hp <= 0) {
            hp = 0;
            state = "deathState";
        }

        // if (state != "deathState" & state != "freezeState") {
        //     if(player_scr.is_freeze) {
        //         state = "freezeState";
        //     }
        // }


        hp_bar.set_hp(hp);
    }

    public void wanderState() {
        desSetter.target = target;
        Vector2 dir = aipath.steeringTarget - trans.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        trans.rotation = Quaternion.Euler(0, 0, angle);

        if ((trans.position - player.position).magnitude < chaseDis) {
            state = "chaseState";
            target = player;
            aipath.endReachedDistance = 5;
        }

        if (aipath.reachedEndOfPath) {
            float xx = Random.Range(-16.74f, 14.38f);
            float yy = Random.Range(-9.16f, 6.57f);
            follow_me.position = new Vector2(xx, yy);
            target = follow_me;
            aipath.endReachedDistance = 1f;
        }

    }

    public void chaseState() {
        desSetter.target = target;
        Vector2 dir = target.position - trans.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        trans.rotation = Quaternion.Euler(0, 0, angle);
        gun_scr.shoot();
    }

    public void freezeState() {
        aipath.canMove = false;
    }

    public void lastState() {
        aipath.canMove = true;
        if (target == follow_me) {
            state = "wanderState";
        } else if (target == player) {
            state = "chaseState";
        }
    }

    public void deathState() {
        desSetter.target = null;
        Debug.Log("Im Dead, lol");
        Destroy(gameObject);
    }

    public void takeDamage(float damage) {
        if (hp > 0) {
            hp -= damage;
        }
    }
}
