using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textResult;
    private Ray _ray;
    private bool _ready;

    public bool OnShoot { get; private set; }
    public Vector3 ClickTarget { get; private set; }
    public Transform ClickedObject { get; private set; }

    void Start()
    {
        _ready = true;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && _ready)
        {
            Vector3 mousePosWithDepth = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
            _ray = Camera.main.ScreenPointToRay(mousePosWithDepth);

            RaycastHit hit;
            if (Physics.Raycast(_ray, out hit))
            {
                _ready = false;

                //  Notify shoot trigger
                OnShoot = true;
                ClickedObject = hit.transform;
                ClickTarget = hit.transform.position;


                //  Determine right answers
                TargetObject _target = hit.collider.gameObject.GetComponent<TargetObject>();

                if (_target.GetComponent<Animator>()) _target.GetComponent<Animator>().CrossFade("falling", 0);
                
                if (_target.isTrue)
                {
                    ShowResult("Benar", Color.green);
                }
                else
                {
                    ShowResult("Salah", Color.red);
                }
                SequenceManager.Instance.NextQuestion();

                //Invoke("Reload", 3f);
            }
        }
    }

    private void LateUpdate() 
    {
        if (OnShoot) OnShoot = false;
        ClickTarget = Vector3.zero;
    }

    private void Reload()
    {
        _ready = true;
    }

    private void ShowResult(string result, Color color)
    {
        _textResult.gameObject.SetActive(true);
        _textResult.color = color;
        _textResult.text = result;
    }
}
