using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// A numbe rof commonly used functions
/// </summary>
public class Utility : MonoBehaviour
{
    /// <summary>
    /// Return a random value within a range of n-r and n+r
    /// </summary>
    /// <param name="n"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static float RandomApproximateValue(float n, float r)
    {
        return Random.Range(n - r, n + r);
    }

    /// <summary>
    /// Return a random 3 dimensional vector between a maximum/minimum x, y, and z value
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Vector3 RandomVector3(float x, float y, float z)
    {
        var randomVector3 = new Vector3(Random.Range(-x, x),
            Random.Range(-y, y),
            Random.Range(-z, z)
        );
        return randomVector3;
    }

    /// <summary>
    /// Return a random vector3 where the x value is within a range of 0 to 360 (degrees)
    /// </summary>
    /// <returns></returns>
    public static Vector3 RandomRotationalVector()
    {
        return new Vector3(0.0F, Random.Range(0.0F, 360.0F), 0.0F);
    }

    /// <summary>
    /// Return a random Vector3 where each parameter is a random value between 0 and 360 (degrees)
    /// </summary>
    /// <returns></returns>
    public static Vector3 RandomVector3()
    {
        return new Vector3(
            Random.Range(0.0F, 360.0F),
            Random.Range(0.0F, 360.0F),
            Random.Range(0.0F, 360.0F)
        );
    }

    /// <summary>
    /// Return a  vector3 where each parameter is a random point between the bounding points the passed vector3
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    public static Vector3 RandomPointInsideCube(Vector3 bounds)
    {
        return new Vector3(
            Random.Range(-bounds.x, bounds.x) / 2,
            Random.Range(-bounds.y, bounds.y) / 2,
            Random.Range(-bounds.z, bounds.z) / 2
        );
    }

    //http://stackoverflow.com/questions/929103/convert-a-number-range-to-another-range-maintaining-ratio
    public static float ConvertRange(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        float newValue;
        var oldRange = oldMax - oldMin;
        if (oldRange == 0)
        {
            newValue = newMin;
        }
        else
        {
            var newRange = newMax - newMin;
            newValue = (oldValue - oldMin) * newRange / oldRange + newMin;
        }

        return newValue;
    }
}
