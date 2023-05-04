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
                ClickTarget = hit.transform.position;


                //  Determine right answers
                TargetObject _target = hit.collider.gameObject.GetComponent<TargetObject>();

                if (_target.GetComponent<Animator>()) _target.GetComponent<Animator>().CrossFade("falling", 0);
                
                if (_target.isTrue)
                {
                    StartCoroutine(showResult("Benar", Color.green));
                }
                else
                {
                    StartCoroutine(showResult("Salah", Color.red));
                }

                Invoke("Reload", 3f);
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

    private IEnumerator showResult(string result, Color color)
    {
        _textResult.gameObject.SetActive(true);
        _textResult.color = color;
        _textResult.text = result;
        yield return new WaitForSeconds(3f);
        _textResult.gameObject.SetActive(false);
    }
}
