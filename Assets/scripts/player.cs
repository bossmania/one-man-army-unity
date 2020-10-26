using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{

    public float spd;
    public Rigidbody2D rb;
    public Camera cam;
    public LineRenderer lr;

    public float max_hp = 100f;
    public float hp;
    public float max_freeze = 100f;
    public float freeze;
    public bool is_freeze = false;
    public float freeze_remove;
    public float freeze_add;
    public freeze_bar freeze_bar;
    public Image filter;
    public health_bar hp_bar;
    public ammo_text ammo_text;
    public Transform barrel;
    public Transform gun;
    public bool can_shoot = true;
    public gun_scr gun_scr;
    public knife_scr knife_scr;

    public string state;

    Vector2 move;
    Vector2 mousepos;

    // Start is called before the first frame update
    void Start()
    {
        hp = max_hp;
        freeze = max_freeze;
        freeze_bar.set_max_freeze(max_freeze);
        hp_bar.set_max_hp(max_hp);
        ammo_text.set_ammo(gun_scr.clip, gun_scr.ammo, gun_scr.weapon);
    }

    // Update is called once per frame
    void Update() {
        if (state != "null") {
            Invoke(state, 0f);
        }

        if (hp <= 0) {
            hp = 0;
            state = "deathState";
        }

        if (is_freeze) {
            freeze -= freeze_remove * Time.deltaTime;

            foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("bullet")) {
                bullet bullet_scr = bullet.GetComponent<bullet>();
                bullet_scr.state = "freezeState";
            }
        } else {
            foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("bullet")) {
                bullet bullet_scr = bullet.GetComponent<bullet>();
                bullet_scr.state = "moveState";
            }

            if (freeze < 100) {
                freeze += freeze_add * Time.deltaTime;
            } else {
                freeze = 100;
            }
        }

        if (freeze <= 0) {
            freeze = 0;
            is_freeze = false;
            filter.enabled = false;

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy")) {
                enemy enemy_scr = enemy.GetComponent<enemy>();
                enemy_scr.state = "lastState";
            }
        }

        freeze_bar.set_freeze(freeze);
        hp_bar.set_hp(hp);
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * spd * Time.fixedDeltaTime);
    }

    void moveState() {
        move.x = Input.GetAxisRaw("hor");
        move.y = Input.GetAxisRaw("ver");

        mousepos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 gun_pos = new Vector2(gun.position.x, gun.position.y);
        Vector2 lookdir = mousepos - gun_pos;
        rb.rotation = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
        gun_scr.shoot_cooldown -= Time.deltaTime;
        if (Input.GetButton("shoot")) {
            if (can_shoot) {
                gun_scr.shoot();
            }
        }

        if (Input.GetButton("aim")) {
            if (can_shoot) {
                lr.enabled = true;
                RaycastHit2D hit = Physics2D.Raycast(barrel.position, barrel.TransformDirection(Vector2.up), Mathf.Infinity, Physics2D.DefaultRaycastLayers, -5, 5);
                List<Vector3> pos = new List<Vector3>();
                pos.Add(barrel.position);
                if (hit.collider != null) {
                    pos.Add(hit.point);
                } else {
                    pos.Add(mousepos);
                }
                lr.startColor = Color.red;
                lr.endColor = Color.red;
                lr.startWidth = 0.05f;
                lr.endWidth = 0.05f;
                lr.SetPositions(pos.ToArray());
                lr.useWorldSpace = true;
            } else {
                lr.enabled = false;
            }
        }
        if (Input.GetButtonUp("aim")) {
            lr.enabled = false;
        }

        if (Input.GetButtonDown("knife")) {
            knife_scr.knife();
        }

        if (Input.GetButtonDown("reload")) {
            gun_scr.reload_handle();
        }

        if (Input.GetButtonDown("freeze")) {
            if (is_freeze == false) {
                is_freeze = true;
                filter.enabled = true;

                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy")) {
                    enemy enemy_scr = enemy.GetComponent<enemy>();
                    enemy_scr.state = "freezeState";
                }
            } else {
                is_freeze = false;
                filter.enabled = false;

                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy")) {
                    enemy enemy_scr = enemy.GetComponent<enemy>();
                    enemy_scr.state = "lastState";
                }
            }
        }
    }

    void deathState() {
        Destroy(gameObject);
        filter.enabled = false;
    }

    public void takeDamage(float damage) {
        if (hp > 0) {
        hp -= damage;
        }
    }

}
