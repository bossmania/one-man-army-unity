using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gernade_scr : MonoBehaviour
{
    public Text Text_field;
    public Transform trans;
    public Transform player;
    public int gernades = 1;
    public GameObject gernade_prefab;

    void Start() {
        trans = GetComponent<Transform>();
        player = trans.parent.parent;
        trans.position = new Vector3(player.position.x, trans.position.y, trans.position.z);
    }

    public void set_gernade() {
        Text_field.text =  "Grenade: "+ gernades.ToString();
        trans.position = new Vector3(player.position.x, trans.position.y, trans.position.z);
    }

    public void throw_gernade() {
        if (gernades > 0) {
            Instantiate(gernade_prefab, player.position+new Vector3(0.5f, 0, 0), player.rotation);
            gernades -= 1;
            set_gernade();
        }
    }
}
