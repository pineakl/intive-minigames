using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    
    [HideInInspector] public bool isTrue;

    public void SetText(string text)
    {
        if (_text != null)
        {
            _text.text = text;
        }
    }
}
