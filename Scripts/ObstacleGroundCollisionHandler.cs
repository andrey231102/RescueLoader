using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGroundCollisionHandler : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Obstacle obstacle))
        {
            _player.IncreaseAmountOfScrews();
            obstacle.Disappear();
        }
    }
}
