 ﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class pause : MonoBehaviour
{
    List<GameObject> objs = new List<GameObject>();
    // public EventSystem system;
    public GameObject[] options = new GameObject[5];
    public GameObject[] bg = new GameObject[5];
    int selected = 0;
    bool mouseInput = true;
    bool axis_move = false;
    GameObject slider_hover;

    void Start()
    {
        foreach (GameObject child in GameObject.FindObjectsOfType(typeof(GameObject))) {
            objs.Add(child);
            if (child != gameObject && child != GameObject.Find("Main Camera")) {
                bool is_child = false;
                foreach (Transform child2 in gameObject.GetComponent<Transform>()) {
                  if (child == child2.gameObject) {
                      is_child = true;
                  }
                  foreach (Transform child3 in child2) {
                    if (child == child3.gameObject) {
                        is_child = true;
                    }
                    foreach (Transform child4 in child3) {
                      if (child == child4.gameObject) {
                          is_child = true;
                      }
                      foreach (Transform child5 in child4) {
                        if (child == child5.gameObject) {
                            is_child = true;
                        }
                        foreach (Transform child6 in child5) {
                          if (child == child6.gameObject) {
                              is_child = true;
                          }
                        }
                      }
                    }
                  }
                }
                if (is_child == false) {
                    child.SetActive(false);
                }
            }
        }
    }

    public void resume() {
        Destroy(gameObject);
    }

    public void mainMenu() {
        Debug.Log("this would take you to the main menu, but it dosent existest right now, so add it in right now.");
    }

    public void exit() {
        Debug.Log("this would exit the game, but it dosent work in the unity editor");
        Application.Quit();
    }

    void Update() {
        // if (Input.GetButtonDown("pause")) {
        //     Destroy(gameObject);
        // }

        if ((Input.GetAxis("mouse x") != 0 || Input.GetAxis("mouse y") != 0)) {
            mouseInput = true;
        }
        if ((Input.GetAxis("ver") != 0 || Input.GetAxis("hor") != 0) && mouseInput == true) {
            mouseInput = false;
            selected = -1;
        }

        if (mouseInput == false) {
            if(Input.GetAxisRaw("ver") != 0) {
                if(axis_move == false && selected != -4) {
                    if (Input.GetAxisRaw("ver") > 0) {
                        selected -= 1;
                    } else if (Input.GetAxisRaw("ver") < 0) {
                        selected += 1;
                    }
                    axis_move = true;
                }
            } if (Input.GetAxisRaw("ver") == 0) {
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

            if (Input.GetAxis("hor") != 0 && selected == -4) {
                if (Input.GetAxis("hor") < 0) {
                    slider_hover.transform.parent.GetComponentInChildren<Slider>().value += 0.1f;
                } else {
                    slider_hover.transform.parent.GetComponentInChildren<Slider>().value -= 0.1f;
                }
            }

            if (Input.GetButtonDown("Submit")) {
                if (selected != -4) {
                options[selected].GetComponent<Button>().onClick.Invoke();
                }
            }

            if (Input.GetButtonDown("Cancel")) {
                if (selected == -4) {
                    selected = int.Parse(slider_hover.name);
                    slider_hover.SetActive(false);
                } else {
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnDestroy() {
        foreach (GameObject child in objs) {
            child.SetActive(true);
        }
    }

    public void onMouseEnter(GameObject obj) {
        obj.SetActive(true);
    }

    public void onMouseExit(GameObject obj) {
        obj.SetActive(false);
    }

    public void onSliderButtonDown(GameObject obj) {
        selected = -4;
        obj.SetActive(true);
        slider_hover = obj;
    }
}
