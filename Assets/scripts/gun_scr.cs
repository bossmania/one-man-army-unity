using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_scr : MonoBehaviour
{
    public Transform barrel;
    public GameObject bullet_prefab;
    public float bullet_spd;
    public int clip;
    public int ammo;
    public int max_clip;
    public string weapon;
    public float damage;
    public float shoot_timer;
    public float shoot_cooldown;
    public ammo_text ammo_text;
    public float reload_timer;
    public float reload_cooldown;
    public bool reload = false;

    void Start()
    {
        max_clip = clip;
        shoot_cooldown = shoot_timer;
        reload_cooldown = reload_timer;
    }

    void Update()
    {
        shoot_cooldown -= Time.deltaTime;
        if (clip <= 0) {
            reload_handle();
        }
        if (reload) {
            reload_cooldown -= Time.deltaTime;
            if (reload_cooldown <= 0) {
                reload = false;
                reload_cooldown = reload_timer;
                clip = max_clip;
                ammo -= clip;
                if (ammo_text != null) {
                    ammo_text.set_ammo(clip, ammo, weapon);
                }
            }
        }
    }

    public void shoot() {
        if (shoot_cooldown <= 0) {
            if (clip != 0) {
                GameObject bullet = Instantiate(bullet_prefab, barrel.position, barrel.rotation);
                bullet bullet_scr = bullet.GetComponent<bullet>();
                bullet_scr.damage = damage;
                bullet_scr.spd = bullet_spd;
                // Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
                // bullet_rb.AddForce(barrel.up*bullet_spd, ForceMode2D.Impulse);
                shoot_cooldown = shoot_timer;
                clip -= 1;
                if (ammo_text != null) {
                    ammo_text.set_ammo(clip, ammo, weapon);
                }
            }
        }
    }

    public void reload_handle() {
        if (ammo_text != null) {
            ammo_text.Text_field.text =  weapon + ": Reloading...";
        }
        reload = true;
    }

}
