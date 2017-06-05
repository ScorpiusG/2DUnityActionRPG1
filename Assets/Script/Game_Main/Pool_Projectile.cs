using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * HOW TO USE
 * 
 * Create an object with this script just once in any intro scene.
 * 
 * Assign a prefab to prefabBullet in Inspector.
 * 
 * Pool_Projectile.pool to access it.
 * 
 * SpawnBullet() to spawn bullets. Arguments are used to change an object's parameters.
 * 
 * ResetPool() to clear list, usually done at a scene's end or a new scene's start.
 */

public class Pool_Projectile : MonoBehaviour
{
    public static Pool_Projectile pool;
    public bool disableSpawnLimit = false;

    public Game_Projectile prefabBullet;
    List<Game_Projectile> listBullet = new List<Game_Projectile>();

    public static int bulletQuantity = 0;
    public int bulletQuantityMax = 1000;

    void Start ()
    {
        pool = this;
        //DontDestroyOnLoad(gameObject);
        ResetPool();
    }

    private void FixedUpdate()
    {
        bulletQuantity = 0;
        foreach (Game_Projectile x in listBullet)
        {
            if (x.gameObject.activeInHierarchy) bulletQuantity++;
            if (!disableSpawnLimit && bulletQuantity >= bulletQuantityMax) break;
        }
    }

    /// <summary>
    /// Spawns an enemy bullet at a spot with rotation and speed values assigned to it.
    /// </summary>
    /// <param name="speed">Speed of the bullet.</param>
    /// <param name="rotation">Rotation of the bullet in degrees - where 0 is up, 90 is left, and 180 is down.</param>
    /// <param name="position">Spawn position of the bullet.</param>
    /// <returns></returns>
    public Game_Projectile SpawnBullet(float speed, float rotation, Vector3 position)
    {
        if (!disableSpawnLimit && bulletQuantity >= bulletQuantityMax) return null;

        // Try to get any inactive bullet in the list
        foreach (Game_Projectile x in listBullet)
        {
            if(!x.gameObject.activeInHierarchy)
            {
                x.movementSpeed = speed;
                x.transform.position = position;
                x.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

                x.gameObject.SetActive(true);
                return x;
            }
        }

        // If no inactive bullet was returned, create a new one
        Game_Projectile newBullet = Instantiate(prefabBullet);
        newBullet.movementSpeed = speed;
        newBullet.transform.position = position;
        newBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        newBullet.gameObject.SetActive(true);
        listBullet.Add(newBullet);
        return newBullet;
    }

    /// <summary>
    /// Spawns an enemy bullet at a spot with rotation and speed values assigned to it. It will also be aimed at an assigned target at spawn.
    /// </summary>
    /// <param name="speed">Speed of the bullet.</param>
    /// <param name="rotation">Rotation of the bullet in degrees - where 0 is up, 90 is left, and 180 is down.</param>
    /// <param name="position">Spawn position of the bullet.</param>
    /// <param name="target">A destination in which the bullet will travel to. Note that the rotation parameter will be added on top of this.</param>
    /// <returns></returns>
    public Game_Projectile SpawnBullet(float speed, float rotation, Vector3 position, Vector3 target)
    {
        Vector3 dir = (target - position).normalized;
        rotation += (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90f;
        return SpawnBullet(speed, rotation, position);
    }

    static public float GetRotation(Vector3 posShooter, Vector3 posTarget)
    {
        Quaternion r = Quaternion.FromToRotation(Vector3.up, posTarget - posShooter);
        return r.z;
    }

    public void ResetPool()
    {
        bulletQuantity = 0;
        listBullet.Clear();
    }
}
