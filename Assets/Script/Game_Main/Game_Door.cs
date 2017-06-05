using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Game_Door : MonoBehaviour
{
    // When player object collides with this door, transition to next scene.

    public string sceneName = "world";
    public Vector2 playerLocation = Vector2.zero;

    private BoxCollider2D mCol;
    private bool isUsed = false;

    private void Start()
    {
        mCol = GetComponent<BoxCollider2D>();
        mCol.isTrigger = true;
    }

    private void Update()
    {
        if (Game_PlayerControl.control.transform.position.x > transform.position.x - (mCol.size.x / 2f) &&
            Game_PlayerControl.control.transform.position.x < transform.position.x + (mCol.size.x / 2f) &&
            Game_PlayerControl.control.transform.position.y > transform.position.y - (mCol.size.y / 2f) &&
            Game_PlayerControl.control.transform.position.y < transform.position.y + (mCol.size.y / 2f) )
        {
            UseDoor();
        }
    }

    private void UseDoor()
    {
        if (isUsed) return;

        isUsed = true;
        Game_PlayerControl.control.isControllable = false;
        GameData.data.playerScene = sceneName;
        GameData.data.playerLocation = playerLocation;
        SceneTransition.GoToScene(sceneName);
    }
}