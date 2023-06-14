using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Animator _resultAnimator;

    private Ray _ray;
    private bool _ready;

    private bool _markOnShoot;
    public bool OnShoot { get; private set; }
    public Vector3 ClickTarget { get; private set; }
    public Transform ClickedObject { get; private set; }

    void Start()
    {
        _ready = true;
    }

    private void FixedUpdate() 
    {
        if (_markOnShoot)
        {
            _markOnShoot = false;
            OnShoot = true;
        }
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

                //  Notify shoot trigger
                _markOnShoot = true;
                ClickedObject = hit.transform;
                ClickTarget = hit.transform.position;

                //  Determine right answers
                TargetObject _target = hit.collider.gameObject.GetComponent<TargetObject>();

                if (_target.GetComponent<Animator>())
                {
                    _target.GetComponent<Animator>().CrossFade("falling", 0);
                }

                StartCoroutine(ShowResult(_target.isTrue));

                SequenceManager.Instance.NextQuestion(_target.AnswerId);

                //Invoke("Reload", 3f);
            }
        }
    }

    private void LateUpdate() 
    {
        if (OnShoot) OnShoot = false;
    }

    private IEnumerator ShowResult(bool isTrue)
    {
        yield return new WaitForSeconds (1f);

        if (_resultAnimator)
        {
            if (isTrue) _resultAnimator.CrossFade("result_right", 0f);
            else _resultAnimator.CrossFade("result_wrong", 0f);
        }
    }
}
