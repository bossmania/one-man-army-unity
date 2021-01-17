using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gernade_scr : MonoBehaviour
{
    public Text Text_field;
    public Transform trans;
    public Transform player_obj;
    public player player_scr;
    public int gernades = 1;
    public GameObject gernade_prefab;

    public void set_gernade() {
        Text_field.text =  "Grenade: "+ gernades.ToString();
        trans.position = new Vector3(player_obj.position.x, trans.position.y, trans.position.z);
    }

    public void throw_gernade(GameObject owner) {
        if (gernades > 0) {
            GameObject gernade = Instantiate(gernade_prefab, owner.GetComponent<enemy>().barrel.position, owner.GetComponent<Transform>().rotation);
            gernade_obj gernade_obj = gernade.GetComponent<gernade_obj>();
            gernade_obj.owner = owner;
            gernades -= 1;
            set_gernade();
        }
    }

    void Start() {
        trans = GetComponent<Transform>();
        player_obj = trans.parent.parent;
        trans.position = new Vector3(player_obj.position.x, trans.position.y, trans.position.z);
        set_gernade();
    }
}
