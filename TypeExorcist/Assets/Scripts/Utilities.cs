using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {

    public static float DistanceSquared(Vector2 vec1, Vector2 vec2)
    {
        Vector2 subVec = vec2 - vec1;
        return subVec.x * subVec.x + subVec.y * subVec.y;
    }
}
