using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ammo_text : MonoBehaviour
{
    public Text Text_field;
    public Transform trans;
    public Transform player;

    void Start() {
        trans = GetComponent<Transform>();
        player = trans.parent.parent;
        trans.position = new Vector3(player.position.x, trans.position.y, trans.position.z);
    }

    public void set_ammo(int clip, int ammo, string weapon) {
        Text_field.text = weapon + ": " +clip.ToString() + "/" + ammo.ToString();
        trans.position = new Vector3(player.position.x, trans.position.y, trans.position.z);
    }
}
