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
    public bool can_knife = true;
    public bool can_tase = true;
    public gun_scr gun_scr;
    public Transform guns;
    public float weapon = 0;
    [System.NonSerialized] public List<string> weapons = new List<string>();
    public knife_scr knife_scr;
    public tase_scr tase_scr;
    public gernade_scr gernade_scr;
    public Transform trans;

    public string state;

    Vector2 move;
    Vector2 mousepos;

    void changeWeapon() {
        foreach (Transform child in guns) {
            if (weapons[(int)weapon] == child.name) {
                gun = child.GetComponent<Transform>();
                gun_scr = child.GetComponent<gun_scr>();
                barrel = gun.GetChild(0).GetComponent<Transform>();
            }
        }
        cam.orthographicSize = 8.83f;
        ammo_text.set_ammo(gun_scr.clip, gun_scr.ammo, gun_scr.weapon);
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = max_hp;
        freeze = max_freeze;
        freeze_bar.set_max_freeze(max_freeze);
        hp_bar.set_max_hp(max_hp);

        foreach (Transform child in guns) {
            weapons.Add(child.name);
        }

        changeWeapon();
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
            foreach (GameObject gernade in GameObject.FindGameObjectsWithTag("gernade")) {
                gernade_obj gernade_scr = gernade.GetComponent<gernade_obj>();
                gernade_scr.state = "freezeState";
            }
        } else {
            foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("bullet")) {
                bullet bullet_scr = bullet.GetComponent<bullet>();
                bullet_scr.state = "moveState";
            }

            if (freeze < max_freeze) {
                freeze += freeze_add * Time.deltaTime;
            } else {
                freeze = max_freeze;
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

        if (Input.GetAxis("scroll wheel") != 0) {
            weapon += Input.GetAxis("scroll wheel");
            if (weapon > weapons.Count-1) {
                weapon = 0;
            }
            if (weapon < 0) {
                weapon = weapons.Count-1;
            }
            changeWeapon();
        }

        if (Input.GetButton("pistol") || Input.GetAxisRaw("pistol") > 0) {
            weapon = 0;
            changeWeapon();
        }

        if (Input.GetButton("assault rifle") || Input.GetAxisRaw("assault rifle") < 0) {
            weapon = 1;
            changeWeapon();
        }

        if (Input.GetButton("shotgun") || Input.GetAxisRaw("shotgun") > 0) {
            weapon = 2;
            changeWeapon();
        }

        if (Input.GetButton("sniper") || Input.GetAxisRaw("sniper") < 0) {
            weapon = 3;
            changeWeapon();
        }

        if ((Input.GetAxis("mouse x") != 0 || Input.GetAxis("mouse y") != 0)) {
            mousepos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gun_pos = new Vector2(gun.position.x, gun.position.y);
            Vector2 lookdir = mousepos - gun_pos;
            rb.rotation = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
        }

        if ((Input.GetAxis("right x") != 0 || Input.GetAxis("right y") != 0)) {
            Vector2 lookdir = new Vector2(Input.GetAxis("right x"), Input.GetAxis("right y"));
            rb.rotation = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
        }

        gun_scr.shoot_cooldown -= Time.deltaTime;

        if (Input.GetButton("shoot") || Input.GetAxisRaw("shoot") > 0) {
            if (can_shoot) {
                gun_scr.shoot();
            }
        }

        if (Input.GetButton("aim") || Input.GetAxisRaw("aim") < 0) {
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
                if (weapons[(int)weapon] == "sniper") {
                    cam.orthographicSize = 12f;
                } else {
                    cam.orthographicSize = 8.83f;
                }
            } else {
                lr.enabled = false;
            }
        }

        if (Input.GetButtonUp("aim") || Input.GetAxisRaw("aim") == 0) {
            lr.enabled = false;
            cam.orthographicSize = 8.83f;
        }

        if (Input.GetButtonDown("knife")) {
            knife_scr.knife();
        }

        if (Input.GetButtonDown("tase")) {
            tase_scr.tase();
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

                foreach (GameObject gernade in GameObject.FindGameObjectsWithTag("gernade")) {
                    gernade_obj gernade_scr = gernade.GetComponent<gernade_obj>();
                    gernade_scr.state = "unfreezeState";
                }
            }
        }

        if (Input.GetButtonDown("gernade")) {
            gernade_scr.throw_gernade();
        }

        if (Input.GetButtonDown("pause")) {
            Debug.Log("paused");
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
