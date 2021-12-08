using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UI;
#endif

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject inputField;
    public string playerName;
    public int highScore;
    public string highName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadState();
        var input = inputField.GetComponent<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        input.onEndEdit = se;

        //or simply use the line below, 
        //input.onEndEdit.AddListener(SubmitName);  // This also works
    }

    private void SubmitName(string arg0)
    {
        playerName = arg0;
        Debug.Log(arg0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("main");
        SaveState();
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public string highName;
        public int Score;
    }

    public void SaveState()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.Score = highScore;
        data.highName = highName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadState()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            highScore = data.Score;
            highName = data.highName;
        }
        else
        {
            playerName = "Mark";
            highScore = 0;
            highName = "Mark";
        }
    }

}
