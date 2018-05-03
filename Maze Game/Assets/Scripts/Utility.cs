using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Vector3 DivideFloatByVector(float dividend, Vector3 divisor)
    {
        return new Vector3(dividend / divisor.x, dividend / divisor.y, 1);
    }
}