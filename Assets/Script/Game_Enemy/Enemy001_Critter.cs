using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy001_Critter : Game_EnemyCore
{
    public float positionMoveToLerpRate = 4f;
    public float randomPositionPlayerStalkX = 5f;
    public float randomPositionPlayerStalkY = 5f;
    public float waitBetweenPositionChange = 3.5f;

    private void Start()
    {
        Initialize();
        StartCoroutine("Routine");
    }

    IEnumerator Routine()
    {
        while(true)
        {
            float posX = 0f;
            float posY = 0f;
            do
            {
                posX = Random.Range(Game_PlayerControl.control.transform.position.x - randomPositionPlayerStalkX, Game_PlayerControl.control.transform.position.x + randomPositionPlayerStalkX);
                posY = Random.Range(Game_PlayerControl.control.transform.position.y - randomPositionPlayerStalkY, Game_PlayerControl.control.transform.position.y + randomPositionPlayerStalkY);
            } while (Mathf.Abs(posX) + Mathf.Abs(posY) < (randomPositionPlayerStalkX + randomPositionPlayerStalkY) / 3f);
            positionMoveTo = new Vector3(posX, posY);

            yield return new WaitForSeconds(waitBetweenPositionChange);
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, positionMoveTo, positionMoveToLerpRate * Time.deltaTime);
    }
}