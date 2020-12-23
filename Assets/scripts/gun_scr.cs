using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_scr : MonoBehaviour
{
    public GameObject bullet_prefab;
    public int clip;
    public int ammo;
    public int max_clip;
    public string weapon;
    public float damage;
    public float og_damage;
    public float shoot_timer;
    public float shoot_cooldown;
    public ammo_text ammo_text;
    public float reload_timer;
    public float reload_cooldown;
    public bool reload = false;

    void Start()
    {
        max_clip = clip;
        og_damage = damage;
        shoot_cooldown = shoot_timer;
        reload_cooldown = reload_timer;
    }

    void Update()
    {
        shoot_cooldown -= Time.deltaTime;
        if (clip <= 0 & reload == false) {
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

        if (ammo_text != null) {
            ammo_text.set_ammo(clip, ammo, weapon);
        }
    }

    public void shoot(Transform barrel, bool dual) {
        if (shoot_cooldown <= 0) {
            if (clip != 0) {
                if (weapon == "shotgun") {
                    GameObject bullet_obj_2 = Instantiate(bullet_prefab, barrel.position, barrel.rotation);
                    bullet bullet_scr_2 = bullet_obj_2.GetComponent<bullet>();
                    bullet_scr_2.damage = damage;
                    Transform bullet_trans = bullet_obj_2.GetComponent<Transform>();
                    bullet_trans.Rotate(Vector3.forward * 8);

                    GameObject bullet_obj_3 = Instantiate(bullet_prefab, barrel.position, barrel.rotation);
                    bullet bullet_scr_3 = bullet_obj_3.GetComponent<bullet>();
                    bullet_scr_3.damage = damage;
                    Transform bullet_trans_2 = bullet_obj_3.GetComponent<Transform>();
                    bullet_trans_2.Rotate(Vector3.forward * -8);
                }

                GameObject bullet_obj = Instantiate(bullet_prefab, barrel.position, barrel.rotation);
                bullet bullet_scr = bullet_obj.GetComponent<bullet>();
                bullet_scr.damage = damage;

                if (dual) {
                    shoot_cooldown = shoot_timer;
                    clip -= 1;
                    if (ammo_text != null) {
                        ammo_text.set_ammo(clip, ammo, weapon);
                    }
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
