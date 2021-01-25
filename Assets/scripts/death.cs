using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class death : MonoBehaviour {


    public TMP_Text survived;
    public GameObject[] options = new GameObject[3];
    public GameObject[] bg = new GameObject[3];
    public GameObject loadingScreen;
    public saveSystem saveSystemController = new saveSystem();
    int selected = 0;
    bool mouseInput = true;
    bool axis_move = false;
    player player_scr;

    void Start() {
        saveSystemController.setPath(Path.Combine(Application.persistentDataPath, "nothing.nothing"));
        saveSystemController.deleteSaveFile();
        player_scr = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        survived.text = "You survive " + (player_scr.gameLevel - 1).ToString() + " floors of enemies.";

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
        }
    }

    public void retry() {
        GameObject loadingScreenObj = Instantiate(loadingScreen, new Vector3(0, 0, 0), new Quaternion(0,0,0,0));
        loading loadingScreen_scr = loadingScreenObj.GetComponent<loading>();
        loadingScreen_scr.dest = 1;
    }

    public void main_menu() {
        Debug.Log("this would take you to the main menu, but it dosent exist right now, so add it in right now.");
    }

    public void exit() {
        Debug.Log("this would exit the game, but it dosent work in the unity editor");
        Application.Quit();
    }

    public void onMouseEnter(GameObject obj) {
        obj.SetActive(true);
    }

    public void onMouseExit(GameObject obj) {
        obj.SetActive(false);
    }
}
