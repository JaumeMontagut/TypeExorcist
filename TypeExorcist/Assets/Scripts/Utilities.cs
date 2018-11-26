using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {

    public static float DistanceSquared(Vector2 vec1)
    {
        return vec1.x * vec1.x + vec1.y * vec1.y;
    }

    public static float DistanceSquared(Vector2 vec1, Vector2 vec2)
    {
        Vector2 subVec = vec2 - vec1;
        return subVec.x * subVec.x + subVec.y * subVec.y;
    }
}
