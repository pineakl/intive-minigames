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

    private Structures.Question _questions;
    private int _currentQuestionId;

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
        LoadQuestion();
    }

    public static SequenceManager Instance
    {
        get { return instance; }
    }

    private void LoadQuestion()
    {
        _questions = JsonUtility.FromJson<Structures.Question>(_questionJSON.text);
    }

    private IEnumerator fetchQuestion(string url, string token)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_server + url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);
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
        if (_currentQuestionId < _gameScenes.Count)
        {
            Invoke("InvokeNextScene", 2f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void InvokeNextScene()
    {
        SceneManager.LoadScene(_gameScenes[_currentQuestionId], LoadSceneMode.Single);
    }
}
