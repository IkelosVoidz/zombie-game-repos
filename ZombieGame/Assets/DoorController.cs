using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;


public class DoorController : MonoBehaviour , IInteractable
{
    [SerializeField] private bool _isBlocked;
    [SerializeField] private bool _isOpen;
    [SerializeField] private bool _opensOutwards;
    [SerializeField] private float _degToTurn;


    //interface method
    public void Interact()
    {
       Open();
    }

    void Open()
    {

    }
}
