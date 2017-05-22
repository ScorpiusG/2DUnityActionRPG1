using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    /// <summary>
    /// Name of item.
    /// </summary>
    public string stringName = "ItemName";

    /// <summary>
    /// Description of item.
    /// </summary>
    public string stringDescription = "ItemDesc";

    /// <summary>
    /// Functionality ID number of item. Zero (0) makes it do nothing.
    /// </summary>
    public int intFunctionID = 0;

    /// <summary>
    /// The item's upgrade cost in one currency. The amount of times upgraded is determined by this list's size.
    /// </summary>
    public int[] intUpgradeCost;

    /// <summary>
    /// As an equipment, this value is its base attack.
    /// </summary>
    public int intAttackBase = 0;

    /// <summary>
    /// As an equipment, this value increases the base attack multiplied by the item's level.
    /// </summary>
    public int intAttackPerUpgrade = 0;

    /// <summary>
    /// The pause between each consecutive attack.
    /// </summary>
    public float floatAttackCooldown = 0f;

    /// <summary>
    /// The value of health (or hitpoints) the user has to pay to use/attack with this item.
    /// </summary>
    public int intCostHealth = 0;

    /// <summary>
    /// The value of resource (or hitpoints) the user has to pay to use/attack with this item.
    /// </summary>
    public int intCostEnergy = 0;

    /// <summary>
    /// The item's AoE distance from the user.
    /// </summary>
    public float floatAreaRadius = 0;

    /// <summary>
    /// The item's AoE angle from where the user is facing (up to 360, in degrees).
    /// </summary>
    public float floatAreaAngleArc = 0;

    /// <summary>
    /// The item affects the position change of the user/target.
    /// </summary>
    public float floatKnockbackDistance = 0f;
}