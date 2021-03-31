using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class player : MonoBehaviour
{

    public float spd;
    public float og_spd;
    public Rigidbody2D rb;
    public LineRenderer lr;
    public float max_hp = 100f;
    public float og_hp;
    public float hp;
    public float max_freeze = 100f;
    public float og_freeze;
    public float freeze;
    public bool is_freeze = false;
    public float freeze_remove;
    public float freeze_add;
    public bool can_freeze = false;
    public float can_freeze_timer;
    public float can_freeze_cooldown;
    public freeze_bar freeze_bar;
    public Image filter;
    public health_bar hp_bar;
    public ammo_text ammo_text;
    public Transform barrel;
    public Transform dual_barrel;
    public Transform gun;
    public bool can_shoot = true;
    public bool can_knife = true;
    public bool can_tase = true;
    public gun_scr gun_scr;
    public Transform guns;
    public float weapon = 0;
    public float max_weapons = 0;
    public List<string> weapons = new List<string>();
    public knife_scr knife_scr;
    public tase_scr tase_scr;
    public gernade_scr gernade_scr;
    public Transform trans;
    public GameObject paused;
    public string state;
    public float knockback_timer;
    public float knockback_cooldown;
    public bool knockback = false;
    public Vector3 knockback_dir;
    public float shock_timer;
    public float shock_cooldown;
    public bool shock = false;
    public int xp = 0;
    public int max_xp = 30;
    public int upgradePoints = 0;
    public int level = 1;
    public int gameLevel = 1;
    public float hp_bonus = 1;
    public float freeze_bonus = 1;
    public float damage_bonus = 1;
    public bool is_slow;
    public float slow;
    public float slow_timer;
    public float slow_cooldown;
    public bool dual_wield = false;
    public saveSystem saveSystemController = new saveSystem();
    public GameObject deathScreen;
    public Animator animator;
    public bool can_aim;

    Vector2 move;
    Vector2 mousepos;
    bool dead = false;
    float deathCooldown = 5;

    void changeWeapon() {

        if (weapon > max_weapons) {
            weapon = 0;
        }
        if (weapon < 0) {
            weapon = max_weapons;
        }

        foreach (Transform child in guns) {
            if (weapons[(int)weapon] == child.name) {
                child.gameObject.SetActive(true);
                gun = child.GetComponent<Transform>();
                gun_scr = child.GetComponent<gun_scr>();
                barrel = gun.GetChild(0).GetComponent<Transform>();
                // knife_scr.gun_spr = child.GetComponent<SpriteRenderer>();
                // tase_scr.gun_spr = child.GetComponent<SpriteRenderer>();
                if (dual_wield) {
                    if (gun.GetChild(1).gameObject.activeSelf == false) {
                        gun.GetChild(1).gameObject.SetActive(true);
                        dual_barrel = gun.GetChild(1).GetComponent<Transform>();
                    }
                    dual_barrel = gun.GetChild(1).GetComponent<Transform>();
                }
                gun_scr.damage = gun_scr.og_damage*damage_bonus;
            } else {
                child.gameObject.SetActive(false);
            }
        }
        Camera.main.orthographicSize = 8.83f;
        ammo_text.set_ammo(gun_scr.clip, gun_scr.ammo, gun_scr.weapon);
    }

    // Start is called before the first frame update
    void Start()
    {
        og_spd = spd;
        hp = max_hp;
        og_hp = max_hp;
        freeze = max_freeze;
        og_freeze = max_freeze;
        knockback_cooldown = knockback_timer;
        can_freeze_timer = can_freeze_cooldown;
        slow_cooldown = slow_timer;

        foreach (Transform child in guns) {
            weapons.Add(child.name);
        }

        changeWeapon();

        saveSystemController.setPath(Path.Combine(Application.persistentDataPath, "nothing.nothing"));

        // if (saveSystemController.checkSaveFile()) {
        //     playerData data = saveSystemController.loadPlayer();
        //     max_hp = data.max_hp;
        //     hp = data.hp;
        //     max_freeze = data.max_freeze;
        //     freeze = data.freeze;
        //     xp = data.xp;
        //     max_xp = data.max_xp;
        //     upgradePoints = data.upgradePoints;
        //     level = data.level;
        //     max_weapons = data.max_weapons;
        //
        //     foreach (Transform child in guns)  {
        //         if (child.name == "pistol") {
        //             child.GetComponent<gun_scr>().ammo = data.pistol_ammo;
        //         } else if (child.name == "assault rifle") {
        //             child.GetComponent<gun_scr>().ammo = data.ak_ammo;
        //         } else if (child.name == "shotgun") {
        //             child.GetComponent<gun_scr>().ammo = data.shotgun_ammo;
        //         } else if (child.name == "sniper") {
        //             child.GetComponent<gun_scr>().ammo = data.sniper_ammo;
        //         }
        //     }
        // }

        freeze_bar.set_max_freeze(max_freeze);
        hp_bar.set_max_hp(max_hp);
    }

    // Update is called once per frame
    void Update() {
        if (state != "null") {
            Invoke(state, 0f);
        }

        if (hp <= 0 && state != "cutsceneState") {
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

            foreach (GameObject enemy_obj in GameObject.FindGameObjectsWithTag("enemy")) {
                enemy enemy_scr = enemy_obj.GetComponent<enemy>();
                if (enemy_scr == null) {
                    brawler brawler_scr = enemy_obj.GetComponent<brawler>();
                    brawler_scr.state = "chaseState";
                } else {
                    enemy_scr.state = "chaseState";
                }
            }
        }

        if (dead == true) {
          if (deathCooldown > 0) {
            deathCooldown -= Time.deltaTime;
          } else {
            Instantiate(deathScreen, new Vector3(0, 0, 0), new Quaternion(0,0,0,0));
            dead = false;
          }
        }

        freeze_bar.set_freeze(freeze);
        hp_bar.set_hp(hp);

        animator.SetInteger("weapon", (int)weapon);
        animator.SetBool("is dual", dual_wield);
        animator.SetBool("is dead", dead);
        animator.SetBool("is reload", gun_scr.reload);

    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * spd * Time.fixedDeltaTime);

        if (is_slow) {
            spd = og_spd * slow;
            if (slow_timer <= 0) {
                is_slow = false;
                slow_timer = slow_cooldown;
            } else {
                slow_timer -= Time.deltaTime;
            }
        } else {
            spd = og_spd;
        }

        if (can_freeze == false) {
            freeze_bar.bar.enabled = false;
            if (can_freeze_timer <= 0) {
                can_freeze = true;
                can_freeze_timer = can_freeze_cooldown;
            } else {
                can_freeze_timer -= Time.deltaTime;
            }
        } else {
            freeze_bar.bar.enabled = true;
        }

        if (knockback) {
            if (knockback_timer >= 0) {
                trans.position += (knockback_dir*10) * Time.deltaTime;
                knockback_timer -= Time.deltaTime;
            } else {
                knockback_timer = knockback_cooldown;
                knockback = false;
            }
        }

        if (shock) {
            if (is_freeze == false) {
                if (shock_cooldown <= 0) {
                    hp -= 10;
                    shock_cooldown = shock_timer;
                } else {
                    shock_cooldown -= Time.deltaTime;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (knockback && (col.gameObject.name != "brawler enemy" && col.gameObject.name != "bullet")) {
            knockback = false;
        }
    }

    void moveState() {
        move.x = Input.GetAxisRaw("hor");
        move.y = Input.GetAxisRaw("ver");

        if (Input.GetAxis("scroll wheel") != 0) {
            weapon += Input.GetAxis("scroll wheel");
            changeWeapon();
        }

        if (Input.GetButton("pistol") || Input.GetAxisRaw("pistol") > 0) {
            weapon = 0;
            changeWeapon();
        }

        if (Input.GetButton("shotgun") || Input.GetAxisRaw("shotgun") < 0) {
            weapon = 1;
            changeWeapon();
        }

        if (Input.GetButton("assault rifle") || Input.GetAxisRaw("assault rifle") > 0) {
            weapon = 2;
            changeWeapon();
        }

        if (Input.GetButton("sniper") || Input.GetAxisRaw("sniper") < 0) {
            weapon = 3;
            changeWeapon();
        }

        if ((Input.GetAxis("mouse x") != 0 || Input.GetAxis("mouse y") != 0)) {
            mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
                if (dual_wield) {
                    gun_scr.shoot(barrel, false);
                    gun_scr.shoot(dual_barrel, true);
                } else {
                    gun_scr.shoot(barrel, true);
                }
            }
        }
        if (can_aim) {
          if (Input.GetButton("aim") || Input.GetAxisRaw("aim") < 0) {
              if (can_shoot) {
                  lr.enabled = true;
                  RaycastHit2D hit = Physics2D.Raycast(barrel.position, barrel.TransformDirection(Vector2.up), Mathf.Infinity, Physics2D.DefaultRaycastLayers, -50, 50);
                  List<Vector3> pos = new List<Vector3>();
                  pos.Add(barrel.position);
                  pos.Add(new Vector3(hit.point.x, hit.point.y, barrel.position.z));
                  lr.startColor = Color.red;
                  lr.endColor = Color.red;
                  lr.startWidth = 0.05f;
                  lr.endWidth = 0.05f;
                  lr.SetPositions(pos.ToArray());
                  lr.useWorldSpace = true;
                  if (weapons[(int)weapon] == "sniper") {
                      Camera.main.orthographicSize = 12f;
                  } else {
                      Camera.main.orthographicSize = 8.83f;
                  }
              } else {
                  lr.enabled = false;
              }
          }

          if (Input.GetButtonUp("aim") || Input.GetAxisRaw("aim") == 0) {
              lr.enabled = false;
              Camera.main.orthographicSize = 8.83f;
          }
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
            if (is_freeze == false && can_freeze) {
                is_freeze = true;
                filter.enabled = true;

                foreach (GameObject enemy_obj in GameObject.FindGameObjectsWithTag("enemy")) {
                    enemy enemy_scr = enemy_obj.GetComponent<enemy>();
                    if (enemy_scr == null) {
                        brawler brawler_scr = enemy_obj.GetComponent<brawler>();
                        brawler_scr.state = "freezeState";
                    } else {
                        enemy_scr.state = "freezeState";
                    }
                }
            } else {
                is_freeze = false;
                filter.enabled = false;

                foreach (GameObject enemy_obj in GameObject.FindGameObjectsWithTag("enemy")) {
                    enemy enemy_scr = enemy_obj.GetComponent<enemy>();
                    if (enemy_scr == null) {
                        brawler brawler_scr = enemy_obj.GetComponent<brawler>();
                        brawler_scr.state = "chaseState";
                    } else {
                        enemy_scr.state = "chaseState";
                    }
                }

                foreach (GameObject gernade in GameObject.FindGameObjectsWithTag("gernade")) {
                    gernade_obj gernade_scr = gernade.GetComponent<gernade_obj>();
                    gernade_scr.state = "unfreezeState";
                }
            }
        }

        if (Input.GetButtonDown("gernade")) {
            gernade_scr.throw_gernade(gameObject);
        }

        if (Input.GetButtonDown("pause")) {
            Instantiate(paused);
        }

        if (xp >= max_xp) {
            level++;
            upgradePoints++;
            max_xp = xp+20;
            xp = xp - max_xp;
            if (xp < 0) {
                xp = 0;
            }
        }

    }

    void deathState() {
        GetComponent<SpriteRenderer>().enabled = false;
        filter.enabled = false;
        can_aim = false;
        lr.enabled = false;
        knockback = false;
        move = new Vector2(0, 0);
        rb.velocity = new Vector2(0, 0);
        state = "cutsceneState";
        dead = true;
        rb.rotation -= 90f;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    void cutsceneState() {
        filter.enabled = false;
        lr.enabled = false;
        rb.velocity = new Vector2(0, 0);
    }

    public void takeDamage(float damage) {
        if (hp > 0) {
        hp -= damage;
        }
    }

    public void punch(Vector3 info) {
        if (hp > 0) {
        hp -= info.z;
        }
        knockback_dir = (trans.position-new Vector3(info.x, info.y, 0f));
        knockback_dir.z = 0f;
        knockback = true;
    }

    public void shockStart() {
        shock = true;
    }

    public void shockEnd() {
        shock = false;
    }
}
