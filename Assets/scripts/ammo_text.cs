using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ammo_text : MonoBehaviour
{
    public Text Text_field;

    public void set_ammo(int clip, int ammo, string weapon) {
        Text_field.text = weapon + ": " +clip.ToString() + "/" + ammo.ToString();
    }
}
