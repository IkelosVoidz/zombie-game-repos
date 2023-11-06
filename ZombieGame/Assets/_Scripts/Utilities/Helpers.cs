using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Static class for methods that dont use monobehaviour functions
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Extension method of GameObject
    /// <br></br>
    /// Will Swap the layer of the object to the one defined by layerName
    /// <br></br>
    /// if children is true it will also change the layer of all the children of OBJ, recursively
    /// </summary>
    public static void SwapLayer(this GameObject obj, string layerName, bool children)
    {
        obj.layer = LayerMask.NameToLayer(layerName);
        if (children)
        {
            foreach (Transform t in obj.transform)
            {
                SwapLayer(t.gameObject, layerName, children);
            }
        }
    }

    /// <summary>
    /// Will immediatly hide the object and will destroy it from the scene after the specified delay
    /// <br></br>
    /// Useful for when you need to wait a few frames for an object to finish doing something before being deleted
    /// </summary>
    public static void HideAndDestroyAfterDelay(this GameObject obj, float delay)
    {
        obj.SetActive(false);
        Object.Destroy(obj, delay);
    }


    static bool _uiIsOpen;
    public static void OpenAndCloseUI(PlayerInput _playerInput)
    {
        if (!_uiIsOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //referencia al componente player input de player
            //player input . switch action mode to UI
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            //referencia al componente player input de player
            //player input . switch action mode to Player
        }
        _uiIsOpen = !_uiIsOpen;
    }

}
