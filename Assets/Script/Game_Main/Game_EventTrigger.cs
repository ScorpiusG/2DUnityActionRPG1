using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Game_EventTrigger : MonoBehaviour
{
    public enum EventType
    {
        Normal,
        OnlyOnce
    }

    public float triggerRange = 1f;
    public AudioClip clipTrigger;
    private AudioSource aSrc;
    public string[] triggerMessage;

    public EventType eventType = EventType.Normal;
    public string eventFlag = "";
    public string eventSpecial = "";

    /// <summary>
    /// Required flag to make this object remain on the map. Leave blank for no condition.
    /// </summary>
    public string eventTriggerCondition = "";

    private void Start()
    {
        aSrc = GetComponent<AudioSource>();

        // If the event cannot be repeated and has been triggered before, delete itself.
        if (eventType == EventType.OnlyOnce)
        {
#if UNITY_EDITOR
            if (eventFlag == "") Debug.LogWarning("WARNING: There is no named flag set for this single-trigger object.");
#endif
            foreach (string x in GameData.data.eventsTriggered)
            {
                if (x == eventFlag)
                {
#if UNITY_EDITOR
                    Debug.Log("The following flag exists, in which will destroy this object: " + eventFlag);
#endif
                    Destroy(gameObject);
                    return;
                }
            }
        }

        // If the event has a trigger condition, check for the acquired flag. If it doesn't exist, the object will be deleted.
        if (eventTriggerCondition != "")
        {
            bool conditionMet = false;
            foreach (string x in GameData.data.eventsTriggered)
            {
                if (x == eventTriggerCondition)
                {
                    conditionMet = true;
                    break;
                }
            }
            if (!conditionMet)
            {
#if UNITY_EDITOR
                Debug.Log("The following flag does not exist, in which will destroy this object: " + eventTriggerCondition);
#endif
                Destroy(gameObject);
                return;
            }
        }

        StartCoroutine("Routine");
    }

    private IEnumerator Routine()
    {
        yield return null;
        while (Vector3.Distance(transform.position, Game_PlayerControl.control.transform.position) > triggerRange)
        {
            yield return null;
        }

        if (aSrc != null && clipTrigger != null)
        {
            aSrc.PlayOneShot(clipTrigger);
        }

        if (triggerMessage.Length > 0)
        {
            Game_DialogueBoxControl.control.ShowMessage(triggerMessage, true);
            yield return new WaitWhile(() => Game_DialogueBoxControl.control.GetState());
            Game_DialogueBoxControl.control.DialogueBoxClose();
        }

        if (eventSpecial != null)
        {
            string[] spc = eventSpecial.Split('|');

            switch (spc[0])
            {
                case "weaponmelee":
                    GameData.data.playerWeaponMeleeList.Add(int.Parse(spc[1]));
                    GameData.data.playerWeaponMeleeListLevel.Add(0);
                    break;
                case "weaponrange":
                    GameData.data.playerWeaponRangeList.Add(int.Parse(spc[1]));
                    GameData.data.playerWeaponRangeListLevel.Add(0);
                    break;
                case "teleport":
                    GameData.data.playerScene = spc[1];
                    GameData.data.playerLocation = new Vector2(float.Parse(spc[2]), float.Parse(spc[3]));
                    SceneTransition.GoToScene(spc[1]);
                    break;
            }
        }

        if (eventFlag != "")
        {
            bool alreadyExists = false;
            foreach (string x in GameData.data.eventsTriggered)
            {
                if (x == eventFlag) alreadyExists = true;
            }
            if (!alreadyExists)
            {
#if UNITY_EDITOR
                Debug.Log("New flag triggered: " + eventFlag);
#endif
                GameData.data.eventsTriggered.Add(eventFlag);
            }
        }

        if (eventType == EventType.OnlyOnce)
        {
            Destroy(gameObject);
        }
    }
}