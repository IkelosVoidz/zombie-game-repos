using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;


public class DoorController : MonoBehaviour , IInteractable
{
    [SerializeField] private bool _isBlocked = false;
    [SerializeField] private bool _isOpen = false;
    [SerializeField] private bool _opensOutwards = false;
    [SerializeField] private float _degToTurn = 160f;

    private int _speed = 1;
    private bool _hasToMove = false;


    private void Start()
    {
        if (_opensOutwards) _degToTurn *= -1;
    }

    //interface method
    public void Interact()
    {
        if (!_isBlocked) 
        {
            if (!_isOpen) {
                Open();
            }
            else
            {
                Close();
            }
        }
        
    }

    void Open()
    {
        _hasToMove = true;
    }

    void Close() {
        _hasToMove = true;
        _degToTurn = -_degToTurn;
    }

    public void changeBlockedStatus()
    {
        _isBlocked = !_isBlocked;
    }

    private void Update()
    {
        if (_hasToMove)
        {
            if (this.transform.eulerAngles.y < _degToTurn && this.transform.rotation.y >= 0) {
                //If the rotation of the door is not bigger than the total rotation it has to do to open or close itself.

                this.transform.Rotate(Vector3.up, _degToTurn * Time.deltaTime * _speed);
                Debug.Log("ESTA ROTANDO");
            }
            else _hasToMove = false;
            
        }
    }
}

