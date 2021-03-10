using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{

    public Transform trans;
    public Transform player;
    public int mouseRadius;
    public string state = "moveState";
    bool mouse = true;

    void Start() {
        trans.position = new Vector3(player.position.x, player.position.y, trans.position.z);
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update() {
      if (state != "null") {
          Invoke(state, 0f);
      }

      player player_scr = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
      if (player_scr.state == "cutsceneState") {
        state = "cutsceneState";
      } else {
        state = "moveState";
      }
    }

    void moveState() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        if ((Input.GetAxis("mouse x") != 0 || Input.GetAxis("mouse y") != 0)) {
            mouse = true;
        }
        if ((Input.GetAxis("right x") != 0 || Input.GetAxis("right y") != 0)) {
            mouse = false;
        }

        if (mouse) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = (mousePos - player.position).normalized * 15;
            dir.x = Mathf.Clamp(dir.x, -mouseRadius, mouseRadius);
            dir.y = Mathf.Clamp(dir.y, -mouseRadius, mouseRadius);
            Vector3 pos = player.position + dir;
            trans.position = new Vector3(pos.x, pos.y, trans.position.z);
            } else {
            Vector3 dir = new Vector3(Input.GetAxis("right x"), Input.GetAxis("right y"), 0f)*5f;
            dir.x = Mathf.Clamp(dir.x, -mouseRadius, mouseRadius);
            dir.y = Mathf.Clamp(dir.y, -mouseRadius, mouseRadius);
            Vector3 pos = player.position + dir;
            trans.position = new Vector3(pos.x, pos.y, trans.position.z);
        }
    }
    void cutsceneState() {

    }
}
