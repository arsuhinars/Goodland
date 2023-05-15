﻿using UnityEngine;

public static class Utils
{
    public static bool LayerMaskContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
