using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Vector2 MousePos { get { return _mousePos; } }
    private static Vector2 _mousePos { get; set; }

    public static Vector3 DivideFloatByVector(float dividend, Vector3 divisor)
    {
        return new Vector3(dividend / divisor.x, dividend / divisor.y, 1);
    }

    public static bool Contains(Cell element, Cell[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (element == array[i])
                return true;
        }

        return false;
    }

    private void Update()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}