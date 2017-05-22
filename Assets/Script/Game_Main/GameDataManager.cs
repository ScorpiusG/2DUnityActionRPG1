using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager
{
    /// <summary>
    /// Saves the game data with PlayerPrefs into the specified ID (string).
    /// </summary>
    /// <param name="id">The "slot" in which the data is saved to. If it already exists, it will be overwritten.</param>
    /// <param name="data">The class of the game data what will be recorded.</param>
    public static void Save(string id, GameData data)
    {
        PlayerPrefs.SetString("save-" + id, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the game data by obtaining data from PlayerPrefs and returning it as a game data class. It will return as "null" if the data does not exist.
    /// </summary>
    /// <param name="id">The "slot" to read from.</param>
    /// <returns></returns>
    public static bool Load(string id)
    {
        if (PlayerPrefs.HasKey("save-" + id))
        {
#if UNITY_EDITOR
            Debug.Log("Loading JSON data: " + PlayerPrefs.GetString("save-" + id));
#endif
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("save-" + id),GameData.data);
            return true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("WARNING: Attempt to load failed. Saved data file does not exist.");
#endif
            return false;
        }
    }

    public static bool Delete(string id)
    {
        if (PlayerPrefs.HasKey("save-" + id))
        {
#if UNITY_EDITOR
            Debug.Log("Delete JSON data: " + PlayerPrefs.GetString("SaveData"));
#endif
            PlayerPrefs.DeleteKey("save-" + id);
            return true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("WARNING: Attempt to delete failed. Saved data file does not exist.");
#endif
            return false;
        }
    }
}