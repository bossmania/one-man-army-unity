using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class playerData
{
    public float max_hp;
    public float hp;
    public float max_freeze;
    public float freeze;
    public int xp;
    public int max_xp;
    public int upgradePoints;
    public int level;
    public int gameLevel;
    public float hp_bonus;
    public float freeze_bonus;
    public float damage_bonus;
    public bool dual_wield;

    public int pistol_ammo;
    public int ak_ammo;
    public int shotgun_ammo;
    public int sniper_ammo;

    public playerData(player player_scr) {
        max_hp = player_scr.max_hp;
        hp = player_scr.hp;
        max_freeze = player_scr.max_freeze;
        freeze = player_scr.freeze;

        xp = player_scr.xp;
        max_xp = player_scr.max_xp;
        upgradePoints = player_scr.upgradePoints;
        level = player_scr.level;
        gameLevel = player_scr.gameLevel;
        hp_bonus = player_scr.hp_bonus;
        freeze_bonus = player_scr.freeze_bonus;
        damage_bonus = player_scr.damage_bonus;
        dual_wield = player_scr.dual_wield;

        foreach (Transform child in player_scr.guns)  {
            if (child.name == "pistol") {
                pistol_ammo = child.GetComponent<gun_scr>().ammo;
            } else if (child.name == "assault rifle") {
                ak_ammo = child.GetComponent<gun_scr>().ammo;
            } else if (child.name == "shotgun") {
                shotgun_ammo = child.GetComponent<gun_scr>().ammo;
            } else if (child.name == "sniper") {
                sniper_ammo = child.GetComponent<gun_scr>().ammo;
            }
        }
    }
}
