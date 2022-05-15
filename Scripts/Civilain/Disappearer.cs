using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappearer : Civilian
{
    [SerializeField] private int _disappearTime;

    public void StartDisappearance()
    {
        StartCoroutine(ReduceScale());
    }

    private IEnumerator ReduceScale()
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * _disappearTime);
            timer += Time.deltaTime;
            yield return null;
            Target.PlayRescuedEffect();
        }
        Die();
        yield break;
    }

    private void Die() => gameObject.SetActive(false);
}
