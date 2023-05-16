using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Marking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textResult;
    [SerializeField] private SpriteRenderer _runnerSprite;

    private Ray _ray;
    private bool _ready;

    void Start()
    {
        Invoke("PauseScroll", 8f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _ready)
        {
            Vector3 mousePosWithDepth = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
            _ray = Camera.main.ScreenPointToRay(mousePosWithDepth);

            RaycastHit hit;
            if (Physics.Raycast(_ray, out hit))
            {
                _ready = false;

                ObjectScroll targetObjScroll = hit.transform.GetComponent<ObjectScroll>();
                if (targetObjScroll)
                {
                    ResumeScroll(targetObjScroll);
                }
            }
        }
    }

    private void PauseScroll()
    {
        if (_runnerSprite)
        {
            _runnerSprite.color = new Color(1f, 1f, 1f, 0.5f);
        }

        _ready = true;
        MinigameManager.instance.Pause = true;
    }

    private void ResumeScroll(ObjectScroll objScroll)
    {
        if (_runnerSprite)
        {
            _runnerSprite.transform.position = new Vector3(
                 objScroll.GetStandX(),
                _runnerSprite.transform.position.y,
                _runnerSprite.transform.position.z
            );
            _runnerSprite.color = new Color(1f, 1f, 1f, 1f);
        }
        MinigameManager.instance.Pause = false;
        
        TargetObject targetObj = objScroll.transform.GetComponent<TargetObject>();
        if (targetObj) CompareResult(targetObj);
    }

    private void CompareResult(TargetObject targetObj)
    {
        if (targetObj.isTrue)
        {
            ShowResult("Benar", Color.green);
        }
        else
        {
            ShowResult("Salah", Color.red);
        }

        targetObj.gameObject.SetActive(false);

        SequenceManager.Instance.NextQuestion();
    }
    
    private void ShowResult(string result, Color color)
    {
        _textResult.gameObject.SetActive(true);
        _textResult.color = color;
        _textResult.text = result;
    }
}
