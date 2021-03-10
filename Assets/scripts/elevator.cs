using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
    public string type;
    public BoxCollider2D collideBox;
    public GameObject xpUI;
    public GameObject arrow;
    public bool arrow_spawn = false;

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
                    if (brawler_scr.dead) {
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
                if (arrow_spawn == false) {
                  Instantiate(arrow, new Vector3(0, 0, 0), new Quaternion(0,0,0,0));
                  arrow_spawn = true;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (type == "exit" && col.gameObject.tag == "Player") {
            player player_scr = col.GetComponent<player>();
            player_scr.state = "cutsceneState";
            player_scr.xp += 30;
            player_scr.gameLevel++;

            foreach (GameObject enemy_obj in GameObject.FindGameObjectsWithTag("enemy")) {
                enemy enemy_scr = enemy_obj.GetComponent<enemy>();
                if (enemy_scr == null) {
                    brawler brawler_scr = enemy_obj.GetComponent<brawler>();
                    brawler_scr.state = "cutsceneState";
                } else {
                    enemy_scr.state = "cutsceneState";
                }
            }

            // saveSystem saveSystemController = new saveSystem();
            // saveSystemController.setPath(Path.Combine(Application.persistentDataPath, "nothing.nothing"));
            // saveSystemController.savePlayer(player_scr);

            GameObject xp = Instantiate(xpUI, new Vector3(0, 0, 0), new Quaternion(0,0,0,0));
            xp_ui xp_scr = xp.GetComponent<xp_ui>();
            xp_scr.mode = "in";
            if (player_scr.gameLevel == 5) {
                xp_scr.dest = 2;
                player_scr.max_weapons = 1;
            } else if (player_scr.gameLevel == 10) {
                xp_scr.dest = 3;
                player_scr.max_weapons = 2;
            } else if (player_scr.gameLevel == 15) {
                xp_scr.dest = 4;
                player_scr.max_weapons = 3;
            } else {
                xp_scr.dest = 1;
            }
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        if (type == "start" && col.gameObject.tag == "Player") {
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
