using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SequenceManager : MonoBehaviour
{
    [SerializeField] private string _server;
    [SerializeField] private TextAsset _questionJSON;
    [SerializeField] private List<string> _gameScenes;

    [SerializeField] private bool _autoExitOnComplete = true;
    [SerializeField] private string _homeScene = "Halls";

    private Structures.Question _questions;
    private int _currentQuestionId;
    private System.Random _random;
    
    [HideInInspector] public int CurrentSceneId = 0;

    private static SequenceManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() 
    {
        _random = new System.Random();

        LoadQuestion();
    }

    public static SequenceManager Instance
    {
        get { return instance; }
    }

    private void LoadQuestion()
    {
        //_questions = JsonUtility.FromJson<Structures.Question>(_questionJSON.text);

        StartCoroutine(fetchQuestion());
    }

    private IEnumerator fetchQuestion()
    {
        WWWForm form = new();
        form.AddField("sub_master_value_id", 2);
        form.AddField("ticket", "996D73D8");

        using (UnityWebRequest webRequest = UnityWebRequest.Post(_server + "/api/game-question", form))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    _questions = JsonUtility.FromJson<Structures.Question>(webRequest.downloadHandler.text);
                    Debug.Log(webRequest.downloadHandler.text);
                    break;
            }

            webRequest.Dispose();
        }
    }

    public Structures.QuestionData GetQuestion()
    {
        return _questions.data[_currentQuestionId];
    }

    public void NextQuestion()
    {
        _currentQuestionId++;
        if (_currentQuestionId < _questions.data.Length)
        {
            Invoke("InvokeNextScene", 3.5f);
        }
        else
        {
            if (_autoExitOnComplete) Invoke("ExitMinigame", 5f);
        }
    }

    private void InvokeNextScene()
    {
        int nextScene = CurrentSceneId;
        while (nextScene == CurrentSceneId)
        {
            nextScene = _random.Next(0, _gameScenes.Count);
        }

        CurrentSceneId = nextScene;
        SceneManager.LoadScene(_gameScenes[nextScene], LoadSceneMode.Single);
    }

    public void ExitMinigame()
    {
        if (_homeScene != null)
        {
            if (_homeScene != string.Empty)
            {
                SceneManager.LoadScene(_homeScene, LoadSceneMode.Single);
            }
        }
        Destroy(this.gameObject);
    }
}
