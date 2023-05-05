using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetObject : MonoBehaviour
{
    [SerializeField] private string _letter;
    [SerializeField] private TextMeshPro _labelLetter;
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private TextMeshProUGUI _textUI;
    
    [HideInInspector] public bool isTrue;

    public void SetText(string text)
    {
        if (_labelLetter) _labelLetter.text = _letter;

        if (_text) _text.text = _letter + ". " + text;
        if (_textUI) _textUI.text = _letter + ". " + text;
    }
}
