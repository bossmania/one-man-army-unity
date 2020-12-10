using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
    public string type;
    public BoxCollider2D collideBox;
    public GameObject loadingScreen;

    void Start()
    {
        if (type == "start") {
            collideBox.enabled = false;
        }
        // foreach (Component child in GetComponents<Component>()) {
        //     Debug.Log(child);
        // }
        // Debug.Log("------------------------------------------------------------");
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
                // Debug.Log("you can leave");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (type == "exit") {
            player player_scr = col.GetComponent<player>();
            player_scr.state = "cutsceneState";
            player_scr.xp += 30;
            Debug.Log("trigger the loading screen");
            GameObject loadingScreenObj = Instantiate(loadingScreen, new Vector3(0, 0, 0), new Quaternion(0,0,0,0));
            loading loadingScreen_scr = loadingScreenObj.GetComponent<loading>();
            loadingScreen_scr.dest = 1;
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
