using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIndicator : MonoBehaviour
{
    [SerializeField] private MeshRenderer _indicator;
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _finalScale;

    private IEnumerator _blinkSwitch;
    private IEnumerator _fadeSwitch;
    private Vector3 _startScale;

    private void Awake()
    {
        _indicator.material = new Material(_indicator.material);

        _blinkSwitch = Blink(_duration);
        _fadeSwitch = Fade(_duration);

        _startScale = _indicator.transform.localScale;
    }

    private IEnumerator Blink(float duration)
    {
        float time = 0;
        while (true)
        {
            _indicator.material.color = new Color(_indicator.material.color.r, _indicator.material.color.g, _indicator.material.color.b,  Mathf.PingPong(time / duration, 0.5f));
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Fade(float duration)
    {
        float time = 0;
        while (true)
        {
            _indicator.material.color = new Color(_indicator.material.color.r, _indicator.material.color.g, _indicator.material.color.b, Mathf.Lerp(_indicator.material.color.a, 0f, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void Activate() => _indicator.enabled = true;

    private IEnumerator Blow()
    {
        for (float t = 0; t < 1; t += Time.deltaTime / 0.5f)
        {
            _indicator.material.color = new Color(_indicator.material.color.r, _indicator.material.color.g, _indicator.material.color.b, Mathf.Lerp(0.5f, 0f, t));
            _indicator.transform.localScale = Vector3.Lerp(_startScale, _finalScale, t);
            StopCoroutine(_fadeSwitch);
            StopCoroutine(_blinkSwitch);
            yield return null;
        }
        _indicator.transform.localScale = _startScale;
        _indicator.enabled = false;
        yield break;
    }

    public void StartBlow()
    {
        StartCoroutine(Blow());
    }

    public void StartBlinking()
    {
        if (_fadeSwitch != null)
        {
            StopCoroutine(_fadeSwitch);
            StopCoroutine(_blinkSwitch);
            StartCoroutine(_blinkSwitch);
        }
    }

    public void StopBlinking()
    {
        if (_blinkSwitch != null)
        {
            StopCoroutine(_blinkSwitch);
            StopCoroutine(_fadeSwitch);
            StartCoroutine(_fadeSwitch);
        }
    }
}
