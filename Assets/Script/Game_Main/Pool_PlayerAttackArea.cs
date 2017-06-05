using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_PlayerAttackArea : MonoBehaviour
{
    // Use this line below to spawn stuff:
    // Pool_PlayerAttackArea.pool.Spawn(Vector3, Quaternion);

    public static Pool_PlayerAttackArea pool;

    public Game_PlayerAttackArea prefabObject;
    List<Game_PlayerAttackArea> listObject = new List<Game_PlayerAttackArea>();

    void Start()
    {
        pool = this;
        //DontDestroyOnLoad(gameObject);
        ResetPool();
    }

    /// <summary>
    /// Attempt to get an object from the pool, and create a new copy otherwise.
    /// </summary>
    /// <param name="position">Location of the object to be placed on.</param>
    /// <param name="rotation">Rotation of the object when spawned.</param>
    /// <returns></returns>
    public Game_PlayerAttackArea Spawn(Vector3 position, Quaternion rotation)
    {
        // Get inactive existing bullet
        foreach (Game_PlayerAttackArea x in listObject)
        {
            if (!x.gameObject.activeInHierarchy)
            {
                x.transform.position = position;
                x.transform.rotation = rotation;

                x.gameObject.SetActive(true);
                return x;
            }
        }
        
        // Create new bullet
        Game_PlayerAttackArea newObj = Instantiate(prefabObject);
        newObj.transform.position = position;
        newObj.transform.rotation = rotation;
        newObj.gameObject.SetActive(true);
        listObject.Add(newObj);
        return newObj;
    }

    public void ResetPool()
    {
        listObject.Clear();
    }
}