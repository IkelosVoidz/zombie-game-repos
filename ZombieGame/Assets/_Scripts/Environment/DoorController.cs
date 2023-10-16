using System;
using UnityEngine;


public class DoorController : MonoBehaviour, IInteractable
{

    [SerializeField, Tooltip("Wether the door can be opened or not by default")] private bool _isBlocked = false;
    [SerializeField, Tooltip("Wether the door will open towards the player when seen from the outside")] private bool _opensOutwards = true;
    [SerializeField, Range(0, 180), Tooltip("How much the door will rotate, in degrees")] private float _degToTurn = 120f;
    [SerializeField, Tooltip("Speed of the open and close animation")] private int _speed = 200;

    private const float LERP_TIME = 0.01f;
    private float t = 0f;
    private float _startingAngle;
    private bool _isOpen = false;

    private void Start()
    {
        _startingAngle = transform.localEulerAngles.y;
        if (_opensOutwards) _degToTurn *= -1;
    }

    //interface method
    public void Interact()
    {
        if (!_isBlocked)
        {
            _isOpen = !_isOpen; //te he reducido dos funciones a una linea de codigo manin
        }
        else { } //play deny sound i guess
    }

    public void changeBlockedStatus()
    {
        _isBlocked = !_isBlocked;
    }

    private void Update()
    {
        if (_isOpen)
        {
            Vector3 targetRotation = new Vector3(0, _degToTurn + _startingAngle, 0);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, LERP_TIME * Time.deltaTime * _speed);
        }
        else
        {
            Vector3 targetRotation = new Vector3(0, _startingAngle, 0);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, LERP_TIME * Time.deltaTime * _speed);
        }

        t = Mathf.Lerp(t, LERP_TIME, Time.deltaTime * _speed);

        if (t >= 1f)
        {
            t = 0f;
        }
    }
}

