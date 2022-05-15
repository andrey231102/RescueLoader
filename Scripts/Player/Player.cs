using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationalSpeed = 360;
    [SerializeField] private float _takeDistance;
    [SerializeField] private float _liftTime;
    [SerializeField] private Transform _cargoSlot;
    [SerializeField] private Transform _fork;
    [SerializeField] private Transform _maxLiftHeight;
    [SerializeField] private Transform _minLiftHeight;
    [SerializeField] private ParticleSystem _rescued;
        
    private GameObject _currentObject;
    private PlayerInput _input;
    private bool _isLoaded = false;
    private int _amountOfScrews;
    private int _amountOfRescuedCivilians;

    public event UnityAction<int> AmountOfScrewsChanged;
    public event UnityAction<int> AmountOfRescuedCiviliansChanged;

    private void Awake()
    {
        _rescued.Stop();
        _input = new PlayerInput();
        _input.Enable();

        _input.Player.Lift.performed += context =>
        {
            if (context.interaction is TapInteraction)
            { 
                if (_isLoaded == false)
                {
                    TryLift();
                }
                else
                {
                    TryDrop();
                }
            }
        };
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

        if (direction!=Vector3.zero)
        {
            Move(direction);
            Rotate(direction);
        }
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction * _speed * Time.deltaTime;
    }

    private void Rotate(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationalSpeed * direction.magnitude * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.5f);
    }

    private void TryLift()
    {
        Obstacle obstacle;
        if (Physics.Raycast(transform.position, transform.forward, out var hitInfo, _takeDistance) && hitInfo.collider.gameObject.TryGetComponent<Obstacle>(out obstacle))
        {
            obstacle.ReleaseCivilian();
            obstacle.ActivateIndicatorBlow();

            _currentObject = hitInfo.collider.gameObject;

            _currentObject.transform.position = default;
            _currentObject.transform.SetParent(_fork);
            _currentObject.transform.position = _cargoSlot.position;

            _currentObject.GetComponent<Rigidbody>().isKinematic = true;
            _currentObject.GetComponent<BoxCollider>().enabled = false;

            _fork.DOMoveY(_maxLiftHeight.position.y, _liftTime);

            _isLoaded = true;
        }
    }

    private void TryDrop()
    {
        if (_currentObject.transform.parent != null)
        {
            _currentObject.transform.parent = null;
            _currentObject.GetComponent<BoxCollider>().enabled = true;
            _currentObject.GetComponent<Obstacle>().EnableIndicator();
            _cargoSlot.GetComponent<Slot>().enabled = true;
            _currentObject.GetComponent<Rigidbody>().isKinematic = false;
            
            _fork.DOMoveY(_minLiftHeight.position.y, _liftTime);

            _isLoaded = false;
        }
    }

    public void Throw(float throwForce)
    {
        TryDrop();
        _currentObject.GetComponent<Rigidbody>().AddForce(Vector3.right*throwForce);
    }

    public void IncreaseAmountOfScrews()
    {
        _amountOfScrews++;
        AmountOfScrewsChanged?.Invoke(_amountOfScrews);
    }

    public void IncreaseAmountOfRescuedCivilians()
    {
        _amountOfRescuedCivilians++;
        AmountOfRescuedCiviliansChanged?.Invoke(_amountOfRescuedCivilians);
    }

    public void PlayRescuedEffect() => _rescued.Play();
}
