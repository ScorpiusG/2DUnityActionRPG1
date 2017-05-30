using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy000_UnkillableDummy : Game_EnemyCore
{
    private void Start()
    {
        Initialize();
        StartCoroutine("Routine");
    }

    IEnumerator Routine()
    {
        while(true)
        {
            hitpointCurrent++;
            if (hitpointCurrent > hitpointMaximum) hitpointCurrent = hitpointMaximum;
            yield return new WaitForSeconds(0.04f);
        }
    }
}