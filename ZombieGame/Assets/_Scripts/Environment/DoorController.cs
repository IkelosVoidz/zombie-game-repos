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
        this.transform.Rotate(new Vector3(0, 1, 0), _degToTurn);
    }

    void Close() {

    }

    void changeBlockedStatus()
    {
        _isBlocked = !_isBlocked;
    }
}

