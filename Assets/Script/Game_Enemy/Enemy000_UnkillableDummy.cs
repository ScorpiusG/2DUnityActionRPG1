using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy000_UnkillableDummy : Game_EnemyCore
{
    private void Start()
    {
        StartCoroutine("Routine");
    }

    /// <summary>
    /// The enemy will take the specified amount of damage. Knockback affects where the enemy is repositioned on hit. This specific enemy cannot be killed by the player.
    /// </summary>
    /// <param name="damage">Amount of damage taken. Hitpoints will be reduced by this.</param>
    /// <param name="knockback">Repositioning of enemy using this Vector3.</param>
    public new void TakeDamage(int damage, Vector3 knockback)
    {
        hitpointCurrent -= damage;

        if (hitpointCurrent <= 0)
        {
            hitpointCurrent = 1;
        }
        if (knockbackOnDamageEnable)
        {
            transform.position += knockback;
        }
    }

    IEnumerator Routine()
    {
        while(true)
        {
            hitpointCurrent++;
            if (hitpointCurrent > hitpointMaximum) hitpointCurrent = hitpointMaximum;
            yield return new WaitForSeconds(0.1f);
        }
    }
}