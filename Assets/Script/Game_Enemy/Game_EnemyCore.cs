using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_EnemyCore : MonoBehaviour
{
    public string targetName = "";

    public int hitpointInitial = -1;
    public int hitpointMaximum = 1000;
    [HideInInspector] public int hitpointCurrent = 0;
    public Vector2 hitboxDamage = new Vector2(0.5f, 0.5f);
    public bool cannotTakeDamage = false;
    public bool cannotBeDestroyed = false;
    public Vector3 positionMoveTo = Vector3.zero;

    public bool knockbackOnDamageEnable = true;

    public void Initialize()
    {
        if (hitpointInitial <= 0) hitpointCurrent = hitpointMaximum;
        else hitpointCurrent = hitpointInitial;
    }

    /// <summary>
    /// The enemy will take the specified amount of damage. A knockback Vector3 is usually specified, but optional (will not take effect).
    /// </summary>
    /// <param name="damage">Amount of damage taken. Hitpoints will be reduced by this.</param>
    public void TakeDamage(int damage)
    {
        TakeDamage(damage, Vector3.zero);
    }
    /// <summary>
    /// The enemy will take the specified amount of damage. Knockback affects where the enemy is repositioned on hit.
    /// </summary>
    /// <param name="damage">Amount of damage taken. Hitpoints will be reduced by this.</param>
    /// <param name="knockback">Repositioning of enemy using this Vector3.</param>
    public void TakeDamage(int damage, Vector3 knockback)
    {
        if (cannotTakeDamage) return;

        int totalDamage = damage + Mathf.FloorToInt(Game_PlayerControl.control.attackCombo * damage / 80);
        totalDamage = Mathf.Clamp(totalDamage, damage, damage * 2);

        hitpointCurrent -= totalDamage;
        Game_PlayerControl.control.AddCombo();

        if (hitpointCurrent <= 0)
        {
            if (cannotBeDestroyed)
            {
                hitpointCurrent = 1;
            }
            else
            {
                hitpointCurrent = 0;
                gameObject.SetActive(false);
            }
        }
        if (knockbackOnDamageEnable)
        {
            transform.position += knockback;
            positionMoveTo += knockback;
        }
    }
}
