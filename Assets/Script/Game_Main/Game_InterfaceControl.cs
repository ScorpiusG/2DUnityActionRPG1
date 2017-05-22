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

    private void Update()
    {
        if(Debug.isDebugBuild)
        {
            textDebug.text =
                "HEALTH = " + GameData.data.playerHealth.ToString() + "\n" +
                "WPN MELEE CURRENT = " + GameInfo.info.itemListWeaponMelee.listItemData[GameData.data.playerWeaponMeleeCurrent].stringName + " (ID " + GameData.data.playerWeaponMeleeCurrent.ToString() + ")\n" +
                "WPN MELEE INVENTORY = " + GameData.data.playerWeaponMeleeList.ToString() + "\n" +
                "WPN RANGE CURRENT = " + GameInfo.info.itemListWeaponRange.listItemData[GameData.data.playerWeaponRangeCurrent].stringName + " (ID " + GameData.data.playerWeaponRangeCurrent.ToString() + ")\n" +
                "WPN RANGE INVENTORY = " + GameData.data.playerWeaponRangeList.ToString() + "\n";
        }
    }
}