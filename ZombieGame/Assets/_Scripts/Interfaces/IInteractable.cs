using System;

public interface IInteractable
{
    void Interact();


    void OnSelect()
    {
        Console.WriteLine("Estas Mirando al objeto");
    }

    void OnDeselect()
    {
        Console.WriteLine("Ya no estas mirando al objeto");
    }
}
