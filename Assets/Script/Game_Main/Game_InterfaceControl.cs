using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_InterfaceControl : MonoBehaviour
{
    public static Game_InterfaceControl control;

    public Text textDebug;

    private void Awake()
    {
        control = this;
    }

    void Start()
    {
        if (!Debug.isDebugBuild) Destroy(textDebug.gameObject);
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if(Debug.isDebugBuild)
        {
            textDebug.text =
                "HEALTH = " + GameData.data.playerHealth.ToString() + "\n" +
                "ENERGY = " + GameData.data.playerEnergy.ToString() + "\n" +
                "WPN MELEE CURRENT = " + GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].stringName + " (ID " + GameData.data.playerWeaponMeleeCurrent.ToString() + ")\n" +
                "WPN RANGE CURRENT = " + GameInfo.info.itemListWeaponRange.listItemData[GameData.data.playerWeaponRangeCurrent].stringName + " (ID " + GameData.data.playerWeaponRangeCurrent.ToString() + ")\n" +
                "\n" +
                "ATTACK COMBO = " + Game_PlayerControl.control.attackCombo.ToString() + "\n" +
                "COMBO TIMER = " + Game_PlayerControl.control.attackComboTimerCurrent.ToString("f2") + " (RED = " + Game_PlayerControl.control.attackComboTimerDecreasingState + ")\n" +
                "\n";

            if (Game_GameControl.control.objectPlayerTarget != null)
            {
                textDebug.text += "TARGET NAME = " + Game_GameControl.control.objectPlayerTarget.targetName + "\n" +
                    "TARGET HITPOINTS = " + Game_GameControl.control.objectPlayerTarget.hitpointCurrent + " / " + Game_GameControl.control.objectPlayerTarget.hitpointMaximum + "\n\n";
            }
        }
#endif
    }
}