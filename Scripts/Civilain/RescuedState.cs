using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescuedState : State
{
    [SerializeField] private GameObject _model;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && transform.parent == null)
        {
            transform.SetParent(other.transform);
            _animator.Play("Climb");
            gameObject.GetComponent<BoxCollider>().enabled = false;
            //_model.SetActive(false);
        }
    }
}
