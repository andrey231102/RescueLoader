using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpState : State
{
    [SerializeField] private ParticleSystem _particles;

    private void Start()
    {
        _particles.Play();
    }

    private void Update()
    {
        if (gameObject.transform.parent == null)
            _particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
