using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_DialogueBoxControl : MonoBehaviour
{
    /*
     * FUNCTIONS
     *  Game_DialogueBoxControl.control.ShowMessage(string[] message);
     *  Game_DialogueBoxControl.control.ShowMessage(List<string> message);
     *  Game_DialogueBoxControl.control.DialogueBoxOpen();
     *  Game_DialogueBoxControl.control.DialogueBoxClose();
     *  Game_DialogueBoxControl.control.GetBoxState(); (returns boolean)
     *  
     * HOW TO USE EXAMPLES
     *  Game_InterfaceControl.control.gameObject.SetActive(false);
     *  Game_PlayerControl.control.isControllable = false;
     *  
     *  Game_DialogueBoxControl.control.ShowMessage(string[] message, false);
     *  yield return new WaitWhile(() => Game_DialogueBoxControl.control.GetState());
     *  Game_DialogueBoxControl.control.DialogueBoxClose();
     *  
     *  Game_InterfaceControl.control.gameObject.SetActive(true);
     *  Game_PlayerControl.control.isControllable = true;
     *  
     *   OR
     *  
     *  Game_DialogueBoxControl.control.ShowMessage(string[] message, true);
     *  yield return new WaitWhile(() => Game_DialogueBoxControl.control.GetState());
     *  Game_DialogueBoxControl.control.DialogueBoxClose();
     */

    public static Game_DialogueBoxControl control;

    public Text textMessage;
    public Animator anim;

    private bool isDisplayingMessage = false;

    /// <summary>
    /// A coroutine to animate and display the message.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private IEnumerator DisplayMessage(string[] message, bool disableControl)
    {
        if (disableControl)
        {
            Game_InterfaceControl.control.gameObject.SetActive(false);
            Game_PlayerControl.control.isControllable = false;
        }

        if (anim.GetInteger("state") == 0)
        {
            DialogueBoxOpen();
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;

        for (int i = 0; i < message.Length; i++)
        {
            textMessage.text = "";
            for (int letter = 0; letter < message[i].Length; letter++)
            {
                textMessage.text += message[i][letter];
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                {
                    textMessage.text = message[i];
                    yield return null;
                    break;
                }
                yield return null;
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space));
            yield return null;
        }

        yield return null;
        isDisplayingMessage = false;

        if (disableControl)
        {
            Game_InterfaceControl.control.gameObject.SetActive(true);
            Game_PlayerControl.control.isControllable = true;
        }
    }

    /// <summary>
    /// Display the messages in the list.
    /// </summary>
    /// <param name="message">The messages to display in the text box.</param>
    /// <param name="disableControl">If true, this additionally disables player control and the interface.</param>
    public void ShowMessage(List<string> message, bool disableControl = false)
    {
        string[] arrayMessage = message.ToArray();
        ShowMessage(arrayMessage, disableControl);
    }
    /// <summary>
    /// Display the messages in the list.
    /// </summary>
    /// <param name="message">The messages to display in the text box.</param>
    /// <param name="disableControl">If true, this additionally disables player control and the interface.</param>
    public void ShowMessage(string[] message, bool disableControl = false)
    {
        isDisplayingMessage = true;
        StartCoroutine(DisplayMessage(message, disableControl));
    }
    /// <summary>
    /// Manually open the dialogue box. It will also automatically open if the box hasn't been opened yet.
    /// </summary>
    public void DialogueBoxOpen()
    {
        textMessage.text = "";
        anim.SetInteger("state", 1);
    }
    /// <summary>
    /// Manually close the dialogue box. It must be called after end of dialogue.
    /// </summary>
    public void DialogueBoxClose()
    {
        textMessage.text = "";
        anim.SetInteger("state", 0);
    }
    /// <summary>
    /// Get the current state of the dialogue box. Returns true if box is visible. Returns false otherwise.
    /// </summary>
    /// <returns></returns>
    public bool GetState()
    {
        return isDisplayingMessage;
    }
    private void Awake()
    {
        control = this;
    }
}