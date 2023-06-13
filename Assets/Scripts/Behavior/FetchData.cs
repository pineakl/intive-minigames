using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FetchData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textQuestion;
    [SerializeField] private TargetObject[] _targets;
    [SerializeField] private RectTransform[] _answerUI;
    List<TargetObject> _tempoObjects;

    private string[] _letters;

    private void Start() 
    {
        _letters = new string[]{"A","B","C","D"};

        Invoke("Reload", 0.5f);
    }

    private void Reload()
    {
        Structures.QuestionData questionObj = SequenceManager.Instance.GetQuestion();

        if (questionObj != null)
        {
            _textQuestion.text = questionObj.question;

            int startIndex = 0;
            if (questionObj.answers.Length < 4) startIndex = 1;

            List<TargetObject> picked = new List<TargetObject>();
            for (int i = 0; i < _targets.Length; i++)
            {
                if (i >= startIndex && i < startIndex + questionObj.answers.Length) picked.Add(_targets[i]);
                else _targets[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < picked.Count; i++)
            {
                if (picked[i].GetComponent<Animator>()) picked[i].GetComponent<Animator>().CrossFade("standing", 0);
                picked[i].SetText(_letters[i], questionObj.answers[i].answer);
                if (questionObj.answer_correct.id == questionObj.answers[i].id) picked[i].isTrue = true;
                if (_answerUI.Length >= picked.Count)
                {
                    _answerUI[i].gameObject.SetActive(true);
                    _answerUI[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = _letters[i] + ". " + questionObj.answers[i].answer;
                }
            }
        }
        else
        {
            Invoke("Reload", 0.5f);
        }
    }
}
