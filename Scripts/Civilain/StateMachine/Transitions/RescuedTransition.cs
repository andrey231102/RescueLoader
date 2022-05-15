using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescuedTransition : Transition
{
    private Civilian _civilian;

    private void Start()
    {
        _civilian = GetComponent<Civilian>();
    }

    private void Update()
    {
        if (transform.parent == null)
        {
            NeedTransit = true;
            _civilian.Target.IncreaseAmountOfRescuedCivilians();
        }
    }
}
