using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    [SerializeField] private float _speed;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator.Play("Run");
    }

    private void OnDisable()
    {
        _animator.StopPlayback();
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, _speed * Time.fixedDeltaTime);
        transform.LookAt(Target.transform);
    }
}
