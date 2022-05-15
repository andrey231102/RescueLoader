using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ScrewsCounter : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _screwsCount;

    private void OnEnable()
    {
        _player.AmountOfScrewsChanged += OnAmountOfScrewsChanged;
    }

    private void OnDisable()
    {
        _player.AmountOfScrewsChanged -= OnAmountOfScrewsChanged;
    }

    private void OnAmountOfScrewsChanged(int amountOfScrews)
    {
        _screwsCount.text = amountOfScrews.ToString();
    }
}
