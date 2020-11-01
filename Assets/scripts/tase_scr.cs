using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tase_scr : MonoBehaviour
{
    public Transform trans;
    public SpriteRenderer spr;
    public SpriteRenderer gun_spr;
    public player player_scr;
    public float tase_timer;
    public float tase_cooldown;
    public bool can_tase = false;
    public Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        tase_cooldown = tase_timer;
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (can_tase) {
            tase_cooldown -= Time.deltaTime;
            if (tase_cooldown < 0) {
                spr.enabled = false;
                gun_spr.enabled = true;
                player_scr.can_shoot = true;
                player_scr.can_knife = true;
                player_scr.can_tase = true;
                can_tase = false;
                tase_cooldown = tase_timer;
                col.enabled = false;
            }
        }
    }

    public void tase() {
        spr.enabled = true;
        gun_spr.enabled = false;
        player_scr.can_shoot = false;
        player_scr.can_knife = false;
        player_scr.can_tase = true;
        can_tase = true;
        col.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D col) {
        col.gameObject.SendMessage("takeTaseDamage", 10);
    }
}
