using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knife_scr : MonoBehaviour
{
    public Transform trans;
    public SpriteRenderer spr;
    public SpriteRenderer gun_spr;
    public player player_scr;
    public float knife_timer = 1f;
    public float knife_cooldown;
    public bool can_knife = false;

    // Start is called before the first frame update
    void Start()
    {
        knife_cooldown = knife_timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (can_knife) {
            knife_cooldown -= Time.deltaTime;
            if (knife_cooldown < 0) {
                spr.enabled = false;
                gun_spr.enabled = true;
                player_scr.can_shoot = true;
                can_knife = false;
                knife_cooldown = knife_timer;
            }
        }
    }

    public void knife() {
        spr.enabled = true;
        gun_spr.enabled = false;
        player_scr.can_shoot = false;
        can_knife = true;
    }
}
