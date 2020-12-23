 ﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class xp_ui : MonoBehaviour
{
    public GameObject[] options = new GameObject[5];
    public GameObject[] bg = new GameObject[5];
    public player player_scr;
    public GameObject loadingScreen;
    int selected = 0;
    bool mouseInput = true;
    bool axis_move = false;

    void Start() {
        player_scr = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
    }

    void Update() {

        if ((Input.GetAxis("mouse x") != 0 || Input.GetAxis("mouse y") != 0)) {
            mouseInput = true;
        }
        if ((Input.GetAxis("ver") != 0 || Input.GetAxis("hor") != 0) && mouseInput == true) {
            mouseInput = false;
            selected = 100;
        }

        if (mouseInput == false) {
            if(Input.GetAxisRaw("hor") != 0) {
                if(axis_move == false && selected != -4) {
                    if (Input.GetAxisRaw("hor") > 0) {
                        selected += 1;
                    } else if (Input.GetAxisRaw("hor") < 0) {
                        selected -= 1;
                    }
                    axis_move = true;
                }
            } if (Input.GetAxisRaw("hor") == 0) {
                axis_move = false;
            }

            if (selected > options.Length-1) {
                selected = 0;
            }
            if (selected < 0 && selected != -4) {
                selected = options.Length-1;
            }

            for (int i=0; i<bg.Length; i++) {
                bg[i].SetActive(false);
            }
            if (selected != -4) {
                bg[selected].SetActive(true);
            }


            if (Input.GetButtonDown("Submit")) {
                if (selected != -4) {
                options[selected].GetComponent<Button>().onClick.Invoke();
                }
            }

            if (Input.GetButtonDown("Cancel")) {
                Destroy(gameObject);
            }
        }
    }

    public void hp() {
        if (player_scr.upgradePoints > 0) {
            player_scr.upgradePoints -= 1;
            player_scr.hp_bonus += 0.04f;
            player_scr.max_hp = player_scr.og_hp*player_scr.hp_bonus;
            player_scr.hp = player_scr.max_hp;
            player_scr.hp_bar.set_max_hp(player_scr.max_hp);
            player_scr.hp_bar.set_hp(player_scr.hp);
        }
    }

    public void freeze() {
        if (player_scr.upgradePoints > 0) {
            player_scr.upgradePoints -= 1;
            player_scr.freeze_bonus += 0.04f;
            player_scr.max_freeze = player_scr.og_freeze*player_scr.freeze_bonus;
            player_scr.freeze = player_scr.max_freeze;
            player_scr.freeze_bar.set_max_freeze(player_scr.max_freeze);
            player_scr.freeze_bar.set_freeze(player_scr.freeze);
        }
    }

    public void damage() {
        if (player_scr.upgradePoints > 0) {
            player_scr.upgradePoints -= 1;
            player_scr.damage_bonus += 0.04f;
        }
    }

    public void dual_wield() {
        if (player_scr.upgradePoints > 2 & player_scr.dual_wield == false) {
            player_scr.upgradePoints -= 3;
            player_scr.dual_wield = true;
        }
    }

    public void exit() {
        GameObject loadingScreenObj = Instantiate(loadingScreen, new Vector3(0, 0, 0), new Quaternion(0,0,0,0));
        loading loadingScreen_scr = loadingScreenObj.GetComponent<loading>();
        loadingScreen_scr.dest = 1;
    }

    public void onMouseEnter(GameObject obj) {
        obj.SetActive(true);
    }

    public void onMouseExit(GameObject obj) {
        obj.SetActive(false);
    }

}
