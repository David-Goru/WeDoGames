using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public static class LayerMaskUtilities
{
    /// <summary>
    /// Returns true if the layer is part of the layermask
    /// </summary>
    public static bool ContainsLayer(LayerMask layerMask, int layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}