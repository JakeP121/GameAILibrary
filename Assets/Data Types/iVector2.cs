using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class iVector2 {

    public int x;
    public int y;

    public iVector2()
    {
        x = 0;
        y = 0;
    }

    public iVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public iVector2(Vector2 vector)
    {
        x = (int)vector.x;
        y = (int)vector.y;
    }

    /// <summary>
    /// Returns the magnitude of the vector
    /// </summary>
    /// <returns>Magnitude float</returns>
    public float magnitude()
    {
        return Mathf.Abs(Mathf.Sqrt(x * x + y * y));
    }

    /// <summary>
    /// Returns a visual representation of the x and y values
    /// </summary>
    /// <returns>Readable string version of properties</returns>
    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }







    ///
    /// Operator overloads
    ///


    // Adding

    /// <summary>
    /// Adding two vectors
    /// </summary>
    public static iVector2 operator + (iVector2 a, iVector2 b)
    {
        return new iVector2(a.x + b.x, a.y + b.y);
    }

    /// <summary>
    /// Adding an integer to a vector
    /// </summary>
    public static iVector2 operator + (iVector2 a, int b)
    {
        return new iVector2(a.x + b, a.y + b);
    }

    /// <summary>
    /// Adding an integer to a vector
    /// </summary>
    public static iVector2 operator +(int a, iVector2 b)
    {
        return new iVector2(a + b.x, a + b.y);
    }

    // Subtracting

    /// <summary>
    /// Subtracting two vectors
    /// </summary>
    public static iVector2 operator - (iVector2 a, iVector2 b)
    {
        return new iVector2(a.x - b.x, a.y - b.y);
    }

    /// <summary>
    /// Subtracting an integer from a vector
    /// </summary>
    public static iVector2 operator - (iVector2 a, int b)
    {
        return new iVector2(a.x - b, a.y - b);
    }

    // Multiplication

    /// <summary>
    /// Multiplying two vectors
    /// </summary>
    public static iVector2 operator * (iVector2 a, iVector2 b)
    {
        return new iVector2(a.x * b.x, a.y * b.y);
    }

    /// <summary>
    /// Multiplying a vector by an integer
    /// </summary>
    public static iVector2 operator * (iVector2 a, int b)
    {
        return new iVector2(a.x * b, a.y * b);
    }

    /// <summary>
    /// Multiplying a vector by an integer
    /// </summary>
    public static iVector2 operator *(int a, iVector2 b)
    {
        return new iVector2(a * b.x, a * b.y);
    }

    /// <summary>
    /// Multiplying a vector by a float
    /// </summary>
    public static iVector2 operator *(iVector2 a, float b)
    {
        return new iVector2((int)(a.x * b), (int)(a.y * b));
    }

    /// <summary>
    /// Multiplying a vector by a float
    /// </summary>
    public static iVector2 operator *(float a, iVector2 b)
    {
        return new iVector2((int)(a * b.x), (int)(a * b.y));
    }


    // Division

    /// <summary>
    /// Dividing two vectors
    /// </summary>
    public static iVector2 operator / (iVector2 a, iVector2 b)
    {
        return new iVector2(a.x / b.x, a.y / b.y);
    }

    /// <summary>
    /// Dividing iVector2 by Vector2
    /// </summary>
    public static iVector2 operator / (iVector2 a, Vector2 b)
    {
        return new iVector2((int)(a.x / b.x), (int)(a.y / b.y));
    }

    /// <summary>
    /// Dividing a vector by an integer
    /// </summary>
    public static iVector2 operator / (iVector2 a, int b)
    {
        return new iVector2(a.x / b, a.y / b);
    }

    /// <summary>
    /// Dividing a vector by a float
    /// </summary>
    public static iVector2 operator / (iVector2 a, float b)
    {
        return new iVector2(a.x / (int)b, a.y / (int)b);
    }

    
    // Casting

    /// <summary>
    /// Casting from an iVector2 to a Vector2
    /// </summary>
    /// <param name="iVector2">this</param>
    public static implicit operator Vector2(iVector2 iVector2)
    {
        return new Vector2(iVector2.x, iVector2.y);
    }

    /// <summary>
    /// Casting from a Vector2 to an iVector2
    /// </summary>
    /// <param name="vector2">incoming vector</param>
    public static implicit operator iVector2(Vector2 vector2)
    {
        return new iVector2((int)vector2.x, (int)vector2.y);
    }
}
