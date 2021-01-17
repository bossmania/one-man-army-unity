using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour {

    public Transform trans;
    public Transform barrel;
    public LineRenderer lr;
    public string type = null;
    public GameObject bullet_prefab;
    public int damage;
    public float shoot_cooldown;
    public float shoot_timer;
    public player player_Scr;

    void Start() {
        player_Scr = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        shoot_timer = shoot_cooldown;

        // Vector3 dir = new Vector3(0, 0, 0);
        //
        // switch (Random.Range(0, 5)) {
        //     case 0:
        //         dir = new Vector3(0, -1, 0);
        //     break;
        //     case 1:
        //         dir = new Vector3(1, 0, 0);
        //     break;
        //     case 2:
        //         dir = new Vector3(0, 1, 0);
        //     break;
        //     case 3:
        //         dir = new Vector3(-1, 0, 0);
        //     break;
        // }
        //
        //
        // RaycastHit2D hit = Physics2D.Raycast(barrel.position, dir, Mathf.Infinity, Physics2D.DefaultRaycastLayers, -50, 50);
        // if (hit.collider != null) {
        //     float x = Mathf.Round(hit.point.x);
        //     float y = Mathf.Round(hit.point.y);
        //
        //     if (dir == new Vector3(0, -1, 0)) {
        //         trans.rotation = Quaternion.Euler(0, 0, 180);
        //         y += 1f;
        //     } else if (dir == new Vector3(1, 0, 0)) {
        //         trans.rotation = Quaternion.Euler(0, 0, 270);
        //         x -= 2f;
        //     } else if (dir == new Vector3(0, 1, 0)) {
        //         trans.rotation = Quaternion.Euler(0, 0, 0);
        //         y -= 1f;
        //     } else if (dir == new Vector3(-1, 0, 0)) {
        //         trans.rotation = Quaternion.Euler(0, 0, 90);
        //         x += 2f;
        //     }
        //
        //     trans.position = new Vector3(x, y, trans.position.z);
        // }


        // switch (Random.Range(0, 3)) {
        //     case 0:
        //         type = "shoot";
        //     break;
        //     case 1:
        //         type = "freeze";
        //     break;
        //     case 2:
        //         type = "slow";
        //     break;
        // }
        type = "shoot";
    }


    void Update() {
        if (player_Scr.is_freeze == false) {
            shoot_cooldown -= Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(barrel.position, barrel.TransformDirection(Vector2.up), Mathf.Infinity, Physics2D.DefaultRaycastLayers, -50, 50);
            List<Vector3> pos = new List<Vector3>();
            pos.Add(barrel.position);
            if (hit.collider != null) {
                pos.Add(hit.point);
                if (hit.collider.name != "wall") {
                    switch (type) {
                        case "shoot":
                            if (shoot_cooldown < 0) {
                                GameObject bullet_obj = Instantiate(bullet_prefab, barrel.position, barrel.rotation);
                                bullet bullet_scr = bullet_obj.GetComponent<bullet>();
                                bullet_scr.damage = damage;
                                shoot_cooldown = shoot_timer;
                            }
                        break;
                        case "freeze":
                            if (hit.collider.tag == "Player") {
                                player_Scr.can_freeze = false;
                            }
                        break;
                        case "slow":
                            player player_scr = hit.collider.gameObject.GetComponent<player>();
                            if (player_scr != null) {
                                player_scr.is_slow = true;
                            } else {
                                enemy enemy_scr = hit.collider.gameObject.GetComponent<enemy>();
                                if (enemy_scr != null) {
                                    enemy_scr.is_slow = true;
                                } else {
                                    brawler brawler_scr = hit.collider.gameObject.GetComponent<brawler>();
                                    if (brawler_scr != null) {
                                        brawler_scr.is_slow = true;
                                    }
                                }
                            }
                        break;
                    }
                }
            } else {
                pos.Add(barrel.TransformDirection(Vector2.up).normalized * 1000);
            }
            switch (type) {
                case "shoot":
                    lr.startColor = new Color(0.8862745098f, 0.8117647059f, 0.1960784314f);
                    lr.endColor = new Color(0.8862745098f, 0.8117647059f, 0.1960784314f);
                break;
                case "freeze":
                lr.startColor = new Color(0, 0.862745098f, 1f);
                lr.endColor = new Color(0, 0.862745098f, 1f);
                break;
                case "slow":
                lr.startColor = new Color(0.5960784314f, 0f, 0.9529411765f);
                lr.endColor = new Color(0.5960784314f, 0f, 0.9529411765f);
                break;
            }
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.SetPositions(pos.ToArray());
            lr.useWorldSpace = true;
        }
    }
}
