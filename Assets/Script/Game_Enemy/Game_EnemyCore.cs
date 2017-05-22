using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_EnemyCore : MonoBehaviour
{
    public int hitpointInitial = -1;
    public int hitpointMaximum = 1000;
    [HideInInspector] public int hitpointCurrent = 0;

    public bool knockbackOnDamageEnable = true;

    void Awake()
    {
        if (hitpointInitial >= 0) hitpointCurrent = hitpointMaximum;
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
        hitpointCurrent -= damage;

        if (hitpointCurrent <= 0)
        {
            gameObject.SetActive(false);
        }
        if (knockbackOnDamageEnable)
        {
            transform.position += knockback;
        }
    }
}
