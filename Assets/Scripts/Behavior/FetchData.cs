using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class FetchData : MonoBehaviour
{
    [SerializeField] private string _server;
    [SerializeField] private TextAsset _questionJSON;
    [SerializeField] private TextMeshProUGUI _textQuestion;
    [SerializeField] private TargetObject[] _targets;
    List<TargetObject> _tempoObjects;

    private Structures.Question _questions;

    private System.Random _random;

    private void Start() 
    {
        _random = new System.Random();

        Invoke("Reload", 0.5f);
    }

    private void Reload()
    {
        _questions = null;
        loadQuestion();
        if (_questions != null)
        {
            _textQuestion.text = _questions.data[0].question;

            int answers = _questions.data[0].answers.Length;
            
            _tempoObjects = new List<TargetObject>();
            for (int i = 0; i < _targets.Length; i++)
            {
                _tempoObjects.Add(_targets[i]);
            }

            List<TargetObject> picked = new List<TargetObject>();
            for (int i = 0; i < answers; i++)
            {
                int indexer = _random.Next(0, _tempoObjects.Count);
                if (_tempoObjects[indexer])
                {
                    picked.Add(_tempoObjects[indexer]);
                    _tempoObjects.Remove(_tempoObjects[indexer]);
                }
            }

            for (int i = 0; i < picked.Count; i++)
            {
                if (picked[i].GetComponent<Animator>()) picked[i].GetComponent<Animator>().CrossFade("standing", 0);
                picked[i].SetText(_questions.data[0].answers[i].answer);
                if (_questions.data[0].answer_correct.id == _questions.data[0].answers[i].id) picked[i].isTrue = true;
            }
        }
    }

    private void loadQuestion()
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
}
