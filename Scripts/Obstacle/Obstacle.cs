using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _destroyEffect;
    [SerializeField] private GameObject _model;
    [SerializeField] private float _timeBeforeDissapear;
    [SerializeField] private ActionIndicator _indicator;
    [SerializeField] private float _bouncePower;

    private Rigidbody _rigidbody;
    private Transform _civilian;

    private void Awake()
    {
        _destroyEffect.Stop();
        _civilian = GetCivilian();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Slot slot))
            _indicator.StartBlinking();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Slot slot))
            _indicator.StopBlinking();
    }

    private Transform GetCivilian()
    {
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).GetComponent<Civilian>() != null)
                return transform.GetChild(i);

        return null;
    }

    public void EnableIndicator()
    {
        _indicator.Activate();
    }

    public void Disappear()
    {
        _destroyEffect.transform.parent = null;
        _destroyEffect.transform.rotation = Quaternion.identity;
        _destroyEffect.Play();

        StartCoroutine(Bounce());
        gameObject.GetComponent<BoxCollider>().enabled = false;

        StartCoroutine(Disappear(_timeBeforeDissapear));
    }

    public void ReleaseCivilian()
    {
        if (_civilian != null)
            _civilian.parent = null;

        return;
    }

    public void ActivateIndicatorBlow()
    {
        _indicator.StartBlow();
    }

    private IEnumerator Bounce()
    {
        _rigidbody.AddForce(Vector3.up * _bouncePower);
        yield return new WaitForSeconds(0.5f);
        _rigidbody.AddForce(Vector3.down * 2*_bouncePower);
        yield break;
    }

    private IEnumerator Disappear(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        Die();

        yield break;
    }

    private void Die() => gameObject.SetActive(false);
}
