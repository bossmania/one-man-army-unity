using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
    public string type;
    public BoxCollider2D collideBox;
    public GameObject xpUI;

    void Start()
    {
        if (type == "start") {
            collideBox.enabled = false;
        }
    }

    void Update() {
        if (type == "exit") {
            int dead = 0;
            foreach (GameObject enemy_obj in GameObject.FindGameObjectsWithTag("enemy")) {
                enemy enemy_scr = enemy_obj.GetComponent<enemy>();
                if (enemy_scr == null) {
                    brawler brawler_scr = enemy_obj.GetComponent<brawler>();
                    if (brawler_scr.state == "deathState") {
                        dead++;
                    }
                } else {
                    if (enemy_scr.dead) {
                        dead++;
                    }
                }
            }
            if (dead == GameObject.FindGameObjectsWithTag("enemy").Length) {
                collideBox.enabled = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (type == "exit") {
            player player_scr = col.GetComponent<player>();
            player_scr.state = "cutsceneState";
            player_scr.xp += 30;
            foreach (GameObject enemy_obj in GameObject.FindGameObjectsWithTag("enemy")) {
                enemy enemy_scr = enemy_obj.GetComponent<enemy>();
                if (enemy_scr == null) {
                    brawler brawler_scr = enemy_obj.GetComponent<brawler>();
                    brawler_scr.state = "cutsceneState";
                } else {
                    enemy_scr.state = "cutsceneState";
                }
            }
            Instantiate(xpUI, new Vector3(0, 0, 0), new Quaternion(0,0,0,0));
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        if (type == "start") {
            collideBox.enabled = true;
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
    }
}
