using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RescuedCiviliansCounter : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _civiliansCount;

    private void OnEnable()
    {
        _player.AmountOfRescuedCiviliansChanged += OnAmountOfScrewsChanged;
    }

    private void OnDisable()
    {
        _player.AmountOfRescuedCiviliansChanged -= OnAmountOfScrewsChanged;
    }

    private void OnAmountOfScrewsChanged(int amountOfScrews)
    {
        _civiliansCount.text = amountOfScrews.ToString();
    }
}
