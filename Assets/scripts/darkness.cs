using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class darkness : MonoBehaviour {

    public Material lightMaterial;
    public GameObject humanLight;
    public GameObject bg;
    public GameObject globalLightPrefab;
    GameObject globalLight;
    List<GameObject> lights = new List<GameObject>();
    bool isGlobalLight = false;

    void Start() {

        foreach (GameObject child in GameObject.FindObjectsOfType(typeof(GameObject))) {
           if (child.activeInHierarchy) {
               try {
                   Renderer render = child.GetComponent<Renderer>();
                   render.material = lightMaterial;
               } catch {

               }
            }
        }

        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        Transform camera_trans = cam.GetComponent<Transform>();
        GameObject bg_cam = Instantiate(bg, camera_trans.position, camera_trans.rotation);
        bg_cam.GetComponent<Transform>().parent = camera_trans;
        cam.GetComponent<Camera>().backgroundColor = Color.black;

        foreach (GameObject player_obj in GameObject.FindGameObjectsWithTag("Player")) {
            Transform trans = player_obj.GetComponent<Transform>();
            GameObject light = Instantiate(humanLight, trans.position, trans.rotation);
            light.GetComponent<Transform>().parent = trans;
            light.SetActive(true);
            lights.Add(light);
        }

        foreach (GameObject enemy_obj in GameObject.FindGameObjectsWithTag("enemy")) {
            Transform trans = enemy_obj.GetComponent<Transform>();
            GameObject light = Instantiate(humanLight, trans.position, trans.rotation);
            light.GetComponent<Transform>().parent = trans;
            light.SetActive(true);
            lights.Add(light);
        }
    }

    void Update() {
        GameObject player_obj = GameObject.FindGameObjectWithTag("Player");
        Transform trans = player_obj.GetComponent<Transform>();
        player player_scr = player_obj.GetComponent<player>();
        if (player_scr.is_freeze) {
            foreach (GameObject light in lights) {
                light.SetActive(false);
            }
            if (isGlobalLight == false) {
                globalLight = Instantiate(globalLightPrefab, trans.position, trans.rotation);
                isGlobalLight = true;
            }
        } else {
            if (isGlobalLight) {
                Destroy(globalLight);
                globalLight = null;
                foreach (GameObject light in lights) {
                    light.SetActive(true);
                }
                isGlobalLight = false;
            }
        }
    }
}
