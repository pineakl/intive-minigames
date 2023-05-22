using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FishRoam : MonoBehaviour
{
    [SerializeField] private Shooting _triggerInput;
    [SerializeField] private HookFollowUp _hook;

    private Vector3 _current;
    private Vector3 _destination;
    private bool _isHook;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Start()
    {
        StartTween();
    }

    private void StartTween()
    {
        _current = transform.position;
        _destination = new Vector3(
            Random.Range(transform.position.x - 1, transform.position.x + 1),
            Random.Range(transform.position.y - 1, transform.position.y + 1),
            transform.position.z
        );

        if (_spriteRenderer) _spriteRenderer.flipX = (_destination.x - _current.x < 0);
        float distance = (_destination - _current).magnitude;
        transform.DOMove(_destination, 10f * distance);

        Invoke("StartTween", 10f * distance);
    }

    private void Update() 
    {
        if (_triggerInput)
        {
            if (_triggerInput.OnShoot)
            {
                DOTween.Kill(transform);
                if (transform == _triggerInput.ClickedObject)
                {
                    CancelInvoke("StartTween");
                    Invoke("HookThis", _hook.DiveTime);
                }
            }
        }

        if (_isHook)
        {
            transform.position = _hook.transform.position;
        }    
    }

    private void HookThis()
    {
        _isHook = true;
    }
}
