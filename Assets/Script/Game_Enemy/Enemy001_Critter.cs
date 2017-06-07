using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy001_Critter : Game_EnemyCore
{
    public float aggroDistance = 3f;
    public List<string> aggroMessage = new List<string>();
    public float positionMoveToLerpRate = 4f;
    public float randomPositionPlayerStalkX = 5f;
    public float randomPositionPlayerStalkY = 5f;
    public float waitBetweenPositionChange = 3.5f;

    private void Start()
    {
        positionMoveTo = transform.position;
        Initialize();
        StartCoroutine("Routine");
    }

    IEnumerator Routine()
    {
        yield return new WaitUntil(() => Vector3.Distance(transform.position, Game_PlayerControl.control.transform.position) < aggroDistance);

        if (aggroMessage.Count > 0)
        {
            //Game_InterfaceControl.control.gameObject.SetActive(false);
            //Game_PlayerControl.control.isControllable = false;

            Game_DialogueBoxControl.control.ShowMessage(aggroMessage, true);
            yield return new WaitWhile(() => Game_DialogueBoxControl.control.GetState());
            Game_DialogueBoxControl.control.DialogueBoxClose();

            //Game_InterfaceControl.control.gameObject.SetActive(true);
            //Game_PlayerControl.control.isControllable = true;
        }

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