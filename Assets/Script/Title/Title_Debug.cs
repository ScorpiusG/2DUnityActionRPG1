using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_Debug : MonoBehaviour
{
    public string stringSceneNewGame = "";
    public Text[] textData;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (GameDataManager.Load("0"))
        {
            textData[0].text =
                "[1]  Name: " + GameData.data.fileName + "  Playtime: " + GameData.data.filePlayTime.ToString("f0") + "\n" +
                "Difficulty: " + GameData.data.fileDifficulty.ToString();
        }
        else
        {
            textData[0].text = "EMPTY";
        }
        if (GameDataManager.Load("1"))
        {
            textData[1].text =
            "[2]  Name: " + GameData.data.fileName + "  Playtime: " + GameData.data.filePlayTime.ToString("f0") + "\n" +
            "Difficulty: " + GameData.data.fileDifficulty.ToString();
        }
        else
        {
            textData[1].text = "EMPTY";
        }
        if (GameDataManager.Load("2"))
        {
            textData[2].text =
            "[3]  Name: " + GameData.data.fileName + "  Playtime: " + GameData.data.filePlayTime.ToString("f0") + "\n" +
            "Difficulty: " + GameData.data.fileDifficulty.ToString();
        }
        else
        {
            textData[2].text = "EMPTY";
        }
    }

    private void Update()
    {
        // Load = ZXC
        if (Input.GetKeyDown(KeyCode.Z))
        {
            LoadFile("0");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            LoadFile("1");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            LoadFile("2");
        }

        // New Game = QWE
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateFile("SIN", "0");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CreateFile("SIN", "1");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CreateFile("SIN", "2");
        }
    }

    void PlayGame()
    {
#if UNITY_EDITOR
        Debug.Log("Starting the game...");
#endif
        if (GameData.data.playerScene == "")
        {
            SceneTransition.GoToScene(stringSceneNewGame);
        }
        else
        {
            SceneTransition.GoToScene(GameData.data.playerScene);
        }
    }

    public void CreateFile(string name, string id)
    {
        GameData.data.Initialize();
        GameData.data.fileID = id;
        GameData.data.fileName = name;

        GameDataManager.Save(id, GameData.data);

        Initialize();
    }

    public void LoadFile(string id)
    {
        if (GameDataManager.Load(id))
        {
            PlayGame();
        }
        else
        {
            Debug.Log("There is no data in this slot.");
        }
    }

    public void DeleteFile(string id)
    {
        GameDataManager.Delete(id);

        Initialize();
    }
}