using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_DifficultySelection : MonoBehaviour
{
    public Camera cameraMain;
    public Color[] colorDifficultyCameraBackground;
    public float floatDifficultyCameraBackgroundLerpRate = 8f;

    public Text textDifficultyValue;

    public Text textDifficultyName;
    public string[] stringDifficultyName = { "Easy", "Normal", "Hard" };

    public Text textDifficultyDescription;
    public string[] stringDifficultyDescription = { "Easy", "Normal", "Hard" };

    public string nextSceneName = "world000";

    private int difficultyID = 1;

    void Start()
    {
        StartCoroutine("Run");
    }

    private void Update()
    {
        if (difficultyID < colorDifficultyCameraBackground.Length)
        {
            cameraMain.backgroundColor = Color.Lerp(cameraMain.backgroundColor, colorDifficultyCameraBackground[difficultyID], Time.deltaTime * floatDifficultyCameraBackgroundLerpRate);
        }

        textDifficultyValue.text = "< " + (difficultyID + 1).ToString() + " / " + stringDifficultyName.Length.ToString() + " >";
        textDifficultyName.text = stringDifficultyName[difficultyID];
        textDifficultyDescription.text = stringDifficultyDescription[difficultyID];
    }

    IEnumerator Run()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                difficultyID--;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                difficultyID++;
            }
            if (difficultyID < 0) difficultyID += stringDifficultyName.Length;
            if (difficultyID >= stringDifficultyName.Length) difficultyID -= stringDifficultyName.Length;

            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            {
                break;
            }

            yield return null;
        }

        GameData.data.fileDifficulty = difficultyID;
        switch(difficultyID)
        {
            default:
                GameData.data.playerHealthMaximum = 30;
                break;
        }

        yield return null;
        GameData.data.playerHealthCurrent = GameData.data.playerHealthMaximum;
        SceneTransition.GoToScene(nextSceneName);
    }
}
