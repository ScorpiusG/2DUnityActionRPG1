using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData : MonoBehaviour
{
    public static GameData data;

    public string fileID = "";
    public string fileName = "";
    public float filePlayTime = 0f;
    public int fileDifficulty = 0;

    public string playerScene = "";
    public Vector2 playerLocation = Vector2.zero;
    public int playerHealth = 1;
    public float playerEnergy = 100;
    public float playerInvincibility = 0f;
    public List<int> playerSkills;
    public List<int> playerWeaponMeleeList;
    public int playerWeaponMeleeCurrent = 1;
    public List<int> playerWeaponRangeList;
    public int playerWeaponRangeCurrent = 0;
    public Vector2 playerFacing = Vector2.down;

    private void Awake()
    {
        if (data == null)
        {
            data = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Deleted all PlayerPrefs keys.");
            PlayerPrefs.DeleteAll();
        }
    }

    public void Initialize()
    {
        filePlayTime = 0f;
        fileDifficulty = 0;

        playerScene = "";
        playerLocation = Vector2.zero;
        playerHealth = 1;
        playerSkills = new List<int>();
        playerWeaponMeleeList = new List<int>();
        playerWeaponRangeList = new List<int>();
        playerWeaponMeleeCurrent = 1;
        playerWeaponRangeCurrent = 0;

        playerSkills.Add(10);
        playerWeaponMeleeList.Add(0);
        playerWeaponMeleeList.Add(1);
        playerWeaponRangeList.Add(0);
    }
}
