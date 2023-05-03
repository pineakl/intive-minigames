using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textResult;
    private Ray _ray;

    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosWithDepth = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
            _ray = Camera.main.ScreenPointToRay(mousePosWithDepth);

            RaycastHit hit;
            if (Physics.Raycast(_ray, out hit))
            {
                TargetObject _target = hit.collider.gameObject.GetComponent<TargetObject>();
                if (_target.isTrue)
                {
                    StartCoroutine(showResult("Benar", Color.green));
                    _target.GetComponent<Animator>().CrossFade("falling", 0);
                }
                else
                {
                    StartCoroutine(showResult("Salah", Color.red));
                    _target.GetComponent<Animator>().CrossFade("falling", 0);
                }
            }
        }
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
