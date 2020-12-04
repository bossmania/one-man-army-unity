using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class brawler : MonoBehaviour
{
    public Transform trans;
    public float max_hp = 100f;
    public float hp;
    public int damage;
    public health_bar hp_bar;
    public string state;
    public Rigidbody2D rb;
    public Transform target;
    public AIDestinationSetter desSetter;
    public AIPath aipath;
    public Transform player;
    public player player_scr;
    public Text weapon;
    public float chaseDis = 8f;
    public float stagger_timer = 7f;
    public float stagger_cooldown;
    public float stagger_flash_timer = 0.25f;
    public float stagger_flash_cooldown;
    public float knockback_timer;
    public float knockback_cooldown;
    public bool knockback = false;
    public Vector3 knockback_dir;


    // Start is called before the first frame update
    void Start() {
        hp = max_hp;
        hp_bar.set_max_hp(max_hp);
        knockback_cooldown = knockback_timer;
        stagger_cooldown = stagger_timer;
        player = GameObject.Find("player").GetComponent<Transform>();
        player_scr = GameObject.Find("player").GetComponent<player>();

        weapon.text = "Brawler";
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (state != "null") {
            Invoke(state, 0f);
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

        if (hp <= (max_hp*0.2) & state != "freezeState" & state != "staggerState") {
            state = "staggerState";
        }

        if (hp <= 0 & state != "freezeState") {
            hp = 0;
            state = "deathState";
        }


        hp_bar.set_hp(hp);
    }

    public void chaseState() {
        target = player;
        aipath.canMove = true;
        desSetter.target = target;
        Vector2 dir = target.position - trans.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        trans.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void freezeState() {
        aipath.canMove = false;
    }

    public void deathState() {
        desSetter.target = null;
        aipath.canMove = false;
        Debug.Log("Im Dead, lol");
        // Destroy(gameObject);
    }


    public void staggerState() {
        aipath.canMove = false;
        if (stagger_cooldown > 0) {
            if (stagger_flash_cooldown <= 0) {
                if (GetComponent<SpriteRenderer>().enabled == true){
                    GetComponent<SpriteRenderer>().enabled = false;
                } else {
                    GetComponent<SpriteRenderer>().enabled = true;
                }
                stagger_flash_cooldown = stagger_flash_timer;
            }
            stagger_cooldown -= Time.deltaTime;
            stagger_flash_cooldown -= Time.deltaTime;
        } else {
            stagger_cooldown = stagger_timer;
            GetComponent<SpriteRenderer>().enabled = true;
            hp = max_hp*0.4f;
            state = "chaseState";
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        Vector3 info = new Vector3(trans.position.x, trans.position.y, damage);
        col.gameObject.transform.gameObject.SendMessage("punch", info);
    }

    public void takeDamage(float damage) {
        if (hp > 0) {
            hp -= damage;
        }
        if (state != "freezeState" & state != "chaseState") {
            state = "chaseState";
            target = player;
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

    public void takeKnifeDamage(float damage) {
        if (hp > 0) {
            hp -= damage;
        }
        if (state == "staggerState") {
            player_scr.hp += 20f;
            state = "deathState";
        }
    }

    public void takeTaseDamage(float damage) {
        if (hp > 0) {
            hp -= damage;
        }
        if (state == "staggerState") {
            foreach (Transform child in player_scr.guns) {
                gun_scr player_gun_scr = child.GetComponent<gun_scr>();
                player_gun_scr.ammo += player_gun_scr.max_clip;
                if (child.name == player_scr.weapons[(int)player_scr.weapon]) {
                    if (player_gun_scr.ammo_text != null) {
                        player_gun_scr.ammo_text.set_ammo(player_gun_scr.clip, player_gun_scr.ammo, player_gun_scr.weapon);
                    }
                }
                if ((int)Random.Range(1, 10) == 1) {
                    player_scr.gernade_scr.gernades += 1;
                    player_scr.gernade_scr.set_gernade();
                } else {
                }
            }
            state = "deathState";
        }
    }
}
