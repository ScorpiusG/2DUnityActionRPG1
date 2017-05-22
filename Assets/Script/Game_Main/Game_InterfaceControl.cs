using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_InterfaceControl : MonoBehaviour
{
    public static Game_InterfaceControl control;

    public Text textDebug;

    void Start()
    {
        control = this;

        if (!Debug.isDebugBuild) Destroy(textDebug.gameObject);
    }

    private void FixedUpdate()
    {
        if(Debug.isDebugBuild)
        {
            textDebug.text =
                "HEALTH = " + GameData.data.playerHealth.ToString() + "\n" +
                "WPN MELEE CURRENT = " + GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].stringName + " (ID " + GameData.data.playerWeaponMeleeCurrent.ToString() + ")\n" +
                "WPN RANGE CURRENT = " + GameInfo.info.itemListWeaponRange.listItemData[GameData.data.playerWeaponRangeCurrent].stringName + " (ID " + GameData.data.playerWeaponRangeCurrent.ToString() + ")\n\n";

            if (Game_GameControl.control.objectPlayerTarget != null)
            {
                textDebug.text += "TARGET NAME = " + Game_GameControl.control.objectPlayerTarget.targetName + "\n" +
                    "TARGET HITPOINTS = " + Game_GameControl.control.objectPlayerTarget.hitpointCurrent + " / " + Game_GameControl.control.objectPlayerTarget.hitpointMaximum + "\n\n";
            }
        }
    }
}