using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FetchData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textQuestion;
    [SerializeField] private TargetObject[] _targets;
    List<TargetObject> _tempoObjects;

    private System.Random _random;

    private void Start() 
    {

        _random = new System.Random();

        Invoke("Reload", 0.5f);
    }

    private void Reload()
    {
        Structures.QuestionData questionObj = SequenceManager.Instance.GetQuestion();

        if (questionObj != null)
        {
            _textQuestion.text = questionObj.question;

            int answers = questionObj.answers.Length;
            
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
                picked[i].SetText(questionObj.answers[i].answer);
                if (questionObj.answer_correct.id == questionObj.answers[i].id) picked[i].isTrue = true;
            }
        }
        else
        {
            Invoke("Reload", 0.5f);
        }
    }
}
