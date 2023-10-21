using UnityEngine;


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
}
