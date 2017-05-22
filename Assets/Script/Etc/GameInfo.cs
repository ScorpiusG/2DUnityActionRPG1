using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static GameInfo info;

    public ItemList itemListWeaponMelee;
    public ItemList itemListWeaponRange;
    public ItemList itemListAbility;

    private void Start()
    {
        if (info == null)
        {
            info = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}