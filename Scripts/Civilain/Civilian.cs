using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Civilian : MonoBehaviour
{
    [SerializeField] private Player _target;

    public Player Target => _target;
}
