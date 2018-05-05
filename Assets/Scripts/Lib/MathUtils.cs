using System;
using UnityEngine;
using System.Collections;

public static class MathUtils {

    #region Bit Twiddling

    /// <summary>
    /// Flips the endianness of the given integer.
    /// </summary>
    /// <returns>The endian-flipped value.</returns>
    /// <param name="toFlip">The integer to flip.</param>
    /// <param name="numLeastSignificantBits">Number of least significant bits that are relevant to the flip.</param>
    public static int FlipEndianness(int toFlip, int numLeastSignificantBits = 0) {
        int flipped = 0;

        int numBitsToFlip = numLeastSignificantBits;
        if (numBitsToFlip <= 0) {
            // 8 bits in a byte, sizeof gives the size of int in bytes.
            numBitsToFlip = sizeof(int) * 8;
        }

        for (int i = 0; i < numBitsToFlip; i++) {
            int leastSigBit = toFlip & 1;
            toFlip >>= 1;
            flipped <<= 1;
            flipped += leastSigBit;
        }

        return flipped;
    }

    #endregion

    #region Vector Multiplication

    /// <summary>
    /// Extension method to multiply component-wise for Vector3.
    /// </summary>
    /// <param name="v1">First part of the component.</param>
    /// <param name="v2">Second part of the component.</param>
    public static Vector3 Mul(this Vector3 v1, Vector3 v2) {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    /// <summary>
    /// Extention method to multiply component-wise for Vector2.
    /// </summary>
    /// <param name="v1">First part of the component.</param>
    /// <param name="v2">Second part of the component.</param>
    public static Vector2 Mul(this Vector2 v1, Vector2 v2) {
        return new Vector2(v1.x * v2.x, v1.y * v2.y);
    }

    /// <summary>
    /// Extention method to do a component-wise multiply for Vector4.
    /// </summary>
    /// <param name="v1">First part of the component.</param>
    /// <param name="v2">Second part of the component.</param>
    public static Vector4 Mul(this Vector4 v1, Vector4 v2) {
        return new Vector4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
    }

    #endregion

    #region Vector Division

    /// Extension method to divide component-wise for Vector3.
    /// </summary>
    /// <param name="v1">First part of the component.</param>
    /// <param name="v2">Second part of the component.</param>
    public static Vector3 ComponentDivide(this Vector3 v1, Vector3 v2) {
        // Don't let a divide by zero happen.
        if (v2.x == 0.0f || v2.y == 0.0f || v2.z == 0.0f) {
            throw new System.DivideByZeroException(string.Format("Attempting to divide by zero: {0}", v2));
        }

        return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
    }

    /// <summary>
    /// Extention method to divide component-wise for Vector2.
    /// </summary>
    /// <param name="v1">First part of the component.</param>
    /// <param name="v2">Second part of the component.</param>
    public static Vector2 ComponentDivide(this Vector2 v1, Vector2 v2) {
        // Don't let a divide by zero happen.
        if (v2.x == 0.0f || v2.y == 0.0f) {
            throw new System.DivideByZeroException(string.Format("Attempting to divide by zero: {0}", v2));
        }

        return new Vector2(v1.x / v2.x, v1.y / v2.y);
    }

    /// <summary>
    /// Extention method to divide component-wise for Vector4.
    /// </summary>
    /// <param name="v1">First part of the component.</param>
    /// <param name="v2">Second part of the component.</param>
    public static Vector4 ComponentDivide(this Vector4 v1, Vector4 v2) {
        // Don't let a divide by zero happen.
        if (v2.x == 0.0f || v2.y == 0.0f || v2.y == 0.0f || v2.y == 0.0f) {
            throw new System.DivideByZeroException(string.Format("Attempting to divide by zero: {0}", v2));
        }

        return new Vector4(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z, v1.w / v2.w);
    }

    #endregion

    #region Vector Rotations

    /// <summary>
    /// Rotate the specified vector by the specified number of degrees clockwise.
    /// </summary>
    /// <param name="v">Vector to rotate.</param>
    /// <param name="degrees">Degrees to rotate the vector.</param>
    public static Vector2 Rotate(this Vector2 v, float degrees) {
        float radians = degrees * Mathf.Deg2Rad;

        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        return new Vector2((cos * v.x) - (sin * v.y), (sin * v.x) + (cos * v.y));
    }

    #endregion

    #region Between

    /// <summary>
    /// Check if the integer is between two numbers, this check is inclusive.
    /// </summary>
    /// <returns><c>true</c> if this int is between the specified min and max; otherwise, <c>false</c>.</returns>
    /// <param name="check">Int to use to compare</param>
    /// <param name="min">Low end to check.</param>
    /// <param name="max">High end to check.</param>
    public static bool Between(this int check, int min, int max) {
        return (min <= check && max >= check);
    }

    /// <summary>
    /// Check if a float is between two numbers, this check is inclusive.
    /// </summary>
    /// /// <returns><c>true</c> if this float is between the specified min and max; otherwise, <c>false</c>.</returns>
    /// <param name="check">Comparison float.</param>
    /// <param name="min">Minimum to accept.</param>
    /// <param name="max">Max to accept.</param>
    public static bool Between(this float check, float min, float max) {
        return (min <= check && max >= check);
    }

    #endregion

    #region Lerp

    /// <summary>
    /// The unbounded version of Lerp.
    /// Mathf.Lerp's timestep is clamped to [0, 1]. This method will not clamp the timestep value, and allows extrapolating.
    /// </summary>
    /// <returns>The lerped value. Note that the value can be outside of the range of [from, to].</returns>
    /// <param name="from">The value to lerp from.</param>
    /// <param name="to">The value to lerp to.</param>
    /// <param name="timeStep">Time step of the lerp.</param>
    public static float UnboundedLerp(float from, float to, float timeStep) {
        return (1 - timeStep) * from + timeStep * to;
    }

    /// <summary>
    /// Coroutine that rolls up a score.
    /// </summary>
    /// <returns>IEnumerator</returns>
    /// <param name="score">Starting value</param>
    /// <param name="endScore">Ending value</param>
    /// <param name="duration">Duration in seconds.</param>
    /// <param name="updateScore">Callback called each time the score is modified. Use this to update your text field or something.</param>
    /// <param name="finishedCallback">Optional callback called when the current value equals endValue.</param>
    public static IEnumerator ScoreCounter(int score, int endScore, float duration,
                                           System.Action<int> updateScore, System.Action finishedCallback = null) {
        float startTime = Time.time;
        float endTime = startTime + duration;
        float min = Mathf.Min(score, endScore);
        float max = Mathf.Max(score, endScore);
        while (Time.time <= endTime && score < endScore) {
            score = (int)Mathf.Clamp(Mathf.Lerp(score, endScore, MathUtils.Map(Time.time, startTime, endTime, 0.0f, 1.0f)),
                                     min, max);
            updateScore(score);
            yield return new WaitForEndOfFrame();
        }
        updateScore(endScore);

        if (finishedCallback != null) {
            finishedCallback();
        }
    }

    #endregion

    #region Map

    /// <summary>
    /// Remaps a value for one range to another.
    /// That is, a value of fromLow would be changed to toLow, fromHigh -> toHigh,
    /// and values in between mapped appropriately.
    /// This function does not clamp if value is outside of the fromLow/High. Clamp using the new range if you so desire.
    /// The integer version truncates any fractional values caused by division while the float version does not.
    /// </summary>
    /// <param name="value">Value to map.</param>
    /// <param name="fromLow">Original range minimum.</param>
    /// <param name="fromHigh">Original range maximum.</param>
    /// <param name="toLow">New range minimum.</param>
    /// <param name="toHigh">New range maximum.</param>
    public static float Map(int value, int fromLow, int fromHigh, float toLow, float toHigh) {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }

    /// <summary>
    /// Remaps a value for one range to another.
    /// That is, a value of fromLow would be changed to toLow, fromHigh -> toHigh,
    /// and values in between mapped appropriately.
    /// This function does not clamp if value is outside of the fromLow/High. Clamp using the new range if you so desire.
    /// The integer version truncates any fractional values caused by division while the float version does not.
    /// </summary>
    /// <param name="value">Value to map.</param>
    /// <param name="fromLow">Original range minimum.</param>
    /// <param name="fromHigh">Original range maximum.</param>
    /// <param name="toLow">New range minimum.</param>
    /// <param name="toHigh">New range maximum.</param>
    public static float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh) {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }

    #endregion

    #region Clamp

    /// <summary>
    /// Clamp a float value between the specified max and min.
    /// </summary>
    /// <returns>If the value passed is less than min it will return min, if it is more than max it will return max.
    /// Otherwise it will return the value.
    /// </returns>
    /// <param name="value">Value to calmp.</param>
    /// <param name="min">Minimum allowed value.</param>
    /// <param name="max">Maximum allowed value.</param>
    public static float Clamp(this float value, float min, float max) {
        return value < min ? min : ((value > max) ? max : value);
    }

    /// <summary>
    /// Clamps this value to a number between zero and one.
    /// </summary>
    /// <returns>The clamp01.</returns>
    /// <param name="value">Dub.</param>
    public static float Clamp01(this float value) {
        return value.Clamp(0.0f, 1.0f);
    }

    /// <summary>
    /// Clamp a double value between the specified max and min.
    /// </summary>
    /// <returns>If the value passed is less than min it will return min, if it is more than max it will return max.
    /// Otherwise it will return the value.
    /// </returns>
    /// <param name="value">Value to calmp.</param>
    /// <param name="min">Minimum allowed value.</param>
    /// <param name="max">Maximum allowed value.</param>
    public static double Clamp(double value, double min, double max) {
        return value < min ? min : ((value > max) ? max : value);
    }

    /// <summary>
    /// Clamps this value to a number between zero and one.
    /// </summary>
    /// <returns>The clamp01.</returns>
    /// <param name="value">Dub.</param>
    public static double Clamp01(double value) {
        return Clamp(value, 0.0d, 1.0d);
    }

    /// <summary>
    /// Clamps the specified value between the min value and the max value.
    /// </summary>
    /// <returns>Min if the value is less than min, max if the value is greater than max, returns the value otherwise..</returns>
    /// <param name="value">Value.</param>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    public static long Clamp(long value, long min, long max) {
        return value < min ? min : ((value > max) ? max : value);
    }

    /// <summary>
    /// Clamp01 the specified value.
    /// </summary>
    /// <returns>The clamp01.</returns>
    /// <param name="value">Value.</param>
    public static long Clamp01(long value) {
        return Clamp(value, 0L, 1L);
    }

    #endregion

    #region Misc

    /// <summary>
    /// Converts a decimal value into an integer percentage.
    /// </summary>
    /// <returns>The decimal percentage multiplied by 100 and rounded to the nearest int.</returns>
    /// <param name="decimalPercentage">Decimal percentage.</param>
    public static int ToPercenti(float decimalPercentage) {
		return Mathf.RoundToInt(ToPercent(decimalPercentage));
	}

	/// <summary>
	/// Converts a decimal value into a float percentage.
	/// </summary>
	/// <returns>The decimal percentage multiplied by 100f.</returns>
	/// <param name="decimalPercentage">Decimal percentage.</param>
	public static float ToPercent(float decimalPercentage) {
		return decimalPercentage * 100f;
	}

	#endregion

}
