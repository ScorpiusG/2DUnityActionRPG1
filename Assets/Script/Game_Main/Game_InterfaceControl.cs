using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_InterfaceControl : MonoBehaviour
{
    public static Game_InterfaceControl control;

    public GameObject groupPlayer;
    public Text textPlayerHealth;
    public string stringPlayerHealthPrefix = "+";
    public string stringPlayerHealthSuffix = "";
    public Text textEnergyValue;
    public string stringEnergyValuePrefix = "=";
    public string stringEnergyValueSuffix = " / 100";
    public Image imageEnergyGauge;

    public GameObject groupTarget;
    public Text textTargetName;
    public Image imageTargetHitpointGauge;
    public Text textTargetHitpointValue;

    public GameObject groupCombo;
    public Text textComboValue;
    public string stringComboValuePrefix = "COMBO ";
    public string stringComboValueSuffix = "";
    public float floatComboValueScaleMaximum = 3f;
    public float floatComboValueScaleThresholdMinimum = 0.9f;
    public Color colorComboValueRed = Color.red;
    public float floatComboValueRedLerpRate = 16f;
    private Color colorComboValueInit = Color.white;
    public Image imageComboGauge;

    public Text textDebug;

    private void Awake()
    {
        control = this;
    }

    void Start()
    {
        groupTarget.SetActive(false);
        groupCombo.SetActive(false);

        colorComboValueInit = textComboValue.color;

        if (!Debug.isDebugBuild) Destroy(textDebug.gameObject);
    }

    private void Update()
    {
        // Player info
        if (GameData.data.playerHealthCurrent > 0)
        {
            if (!groupPlayer.activeSelf) groupPlayer.SetActive(true);
            textPlayerHealth.text = stringPlayerHealthPrefix + GameData.data.playerHealthCurrent.ToString() + stringPlayerHealthSuffix;
            textEnergyValue.text = stringEnergyValuePrefix + GameData.data.playerEnergy.ToString("f0") + stringEnergyValueSuffix;
            imageEnergyGauge.fillAmount = GameData.data.playerEnergy / 100f;
        }
        else
        {
            if (groupPlayer.activeSelf) groupPlayer.SetActive(false);
        }

        // Target info
        if (Game_GameControl.control.objectPlayerTarget != null)
        {
            if (!groupTarget.activeSelf) groupTarget.SetActive(true);

            textTargetName.text = Game_GameControl.control.objectPlayerTarget.targetName;
            textTargetHitpointValue.text = Game_GameControl.control.objectPlayerTarget.hitpointCurrent.ToString();
            imageTargetHitpointGauge.fillAmount = 1f * Game_GameControl.control.objectPlayerTarget.hitpointCurrent / Game_GameControl.control.objectPlayerTarget.hitpointMaximum;
        }
        else
        {
            if (groupTarget.activeSelf) groupTarget.SetActive(false);
        }

        // Attack combo
        if (Game_PlayerControl.control.attackCombo > 0)
        {
            if (!groupCombo.activeSelf) groupCombo.SetActive(true);

            // Combo value
            textComboValue.text = stringComboValuePrefix + Game_PlayerControl.control.attackCombo + stringComboValueSuffix;

            // Normal
            if (!Game_PlayerControl.control.attackComboTimerDecreasingState)
            {
                float finalScale = 1f +
                    ((Game_PlayerControl.control.attackComboTimerCurrent / Game_PlayerControl.control.attackComboTimerOnHit) - floatComboValueScaleThresholdMinimum) * floatComboValueScaleMaximum;
                finalScale = Mathf.Clamp(finalScale, 1f, floatComboValueScaleMaximum);
                textComboValue.rectTransform.localScale = Vector3.one * finalScale;
                textComboValue.color = colorComboValueInit;
                imageComboGauge.fillAmount = Game_PlayerControl.control.attackComboTimerCurrent / Game_PlayerControl.control.attackComboTimerOnHit;
            }
            // In red (decreasing)
            else
            {
                textComboValue.rectTransform.localScale = Vector3.one;
                textComboValue.color = Color.Lerp(textComboValue.color, colorComboValueRed, floatComboValueRedLerpRate * Time.unscaledDeltaTime);
                imageComboGauge.fillAmount = 0f;
            }
        }
        else
        {
            if (groupCombo.activeSelf) groupCombo.SetActive(false);
        }

#if UNITY_EDITOR
        if(Debug.isDebugBuild)
        {
            textDebug.text =
                "### DEBUG ###\n" +
                "\n" +
                "HEALTH = " + GameData.data.playerHealthCurrent.ToString() + " (MAX " + GameData.data.playerHealthMaximum.ToString() + ")\n" +
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