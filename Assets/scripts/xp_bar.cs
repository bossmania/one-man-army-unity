using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class xp_bar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image bar;
    public TMP_Text xp_text;
    public TMP_Text level_text;
    public TMP_Text upgradePoints_text;
    public TMP_Text hp_bonus_text;
    public TMP_Text freeze_bonus_text;
    public TMP_Text damage_bonus_text;
    public TMP_Text dual_wield_text;
    public player player_scr;


    void Update() {
        player_scr = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        slider.maxValue = player_scr.max_xp;
        slider.value = player_scr.xp;

        xp_text.text = "XP: " + player_scr.xp.ToString() + "/" + player_scr.max_xp.ToString();
        level_text.text = "Level " + player_scr.level.ToString();
        upgradePoints_text.text = "Spendable Upgrades: " + player_scr.upgradePoints.ToString();
        hp_bonus_text.text = "x" + player_scr.hp_bonus.ToString();
        freeze_bonus_text.text = "x" + player_scr.freeze_bonus.ToString();
        damage_bonus_text.text = "x" + player_scr.damage_bonus.ToString();
        if (player_scr.dual_wield == false) {
            dual_wield_text.text = "Upgrade Cost: 3";
        } else {
            dual_wield_text.text = "Purchased";
        }

        bar.color = gradient.Evaluate(slider.normalizedValue);
    }

}
