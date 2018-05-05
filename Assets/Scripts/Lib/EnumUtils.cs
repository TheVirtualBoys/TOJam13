using System;
using UnityEngine;
using System.Collections;

public static class EnumUtils {

	/// <summary>
	/// Gets the length of an enumeration via nefarious means.
	/// </summary>
	/// <returns>The length.</returns>
	/// <typeparam name="T">The Enum.</typeparam>
	public static int GetLength<T>() {
		return System.Enum.GetValues(typeof(T)).Length;
	}

	/// <summary>
	/// Gets the long length.
	/// </summary>
	/// <returns>The long length.</returns>
	/// <typeparam name="T">The Enum.</typeparam>
	public static long GetLongLength<T>() {
		return System.Enum.GetValues(typeof(T)).LongLength;
	}

	/// <summary>
	/// Parses an enum (used internally)
	/// </summary>
	/// <returns>The internal.</returns>
	/// <param name="t">Type of variable to return</param>
	/// <param name="value">Value to parse.</param>
	/// <param name="ignoreCase">If set to <c>true</c> ignore case.</param>
	/// <typeparam name="T">The enum type.</typeparam>
	private static T ParseInternal<T>(System.Type t, string value, bool ignoreCase) {
		return (T)System.Enum.Parse(t, value, ignoreCase);
	}

	/// <summary>
	/// Parse the specified value and ignoreCase.
	/// </summary>
	/// <param name="value">Value to parse.</param>
	/// <param name="ignoreCase">If set to <c>true</c> ignore case.</param>
	/// <typeparam name="T">The enum.</typeparam>
	public static T Parse<T>(string value, bool ignoreCase = false) {
		return ParseInternal<T>(typeof(T), value, ignoreCase);
	}

	/// <summary>
	/// Gets a random entry of an enum.
	/// </summary>
	/// <returns>A random enum entry.</returns>
	/// <typeparam name="T">The enum.</typeparam>
	public static T RandomElement<T>() {
		return RandomElement<T>(GetLength<T>());
	}

	/// <summary>
	/// Gets a random entry of an enum.
	/// </summary>
	/// <returns>A random enum entry at index less than max.</returns>
	/// <param name="max">Maximum (exclusive.)</param>
	/// <typeparam name="T">The enum.</typeparam>
	public static T RandomElement<T>(int max) {
		return RandomElement<T>(0, max);
	}

	/// <summary>
	/// Gets a random entry of an enum between min (inclusive) and max (exclusive.)
	/// </summary>
	/// <param name="min">Minimum (inclusive.)</param>
	/// <param name="max">Maximum (exclusive.)</param>
	/// <typeparam name="T">The enum.</typeparam>
	public static T RandomElement<T>(int min, int max) {
		System.Type t = typeof(T);
		int length = System.Enum.GetValues(t).Length;
		Debug.Assert(min >= 0 && max >= min && min < length && max <= length, "Random min/max out of enum bounds.");
		int index = UnityEngine.Random.Range(min, max);
		string name = System.Enum.GetName(t, index);
		return ParseInternal<T>(t, name, false);
	}

	/// <summary>
	/// Determines whether one or more bit fields are set in the current instance.
	/// thisInstance And flag = flag 
	/// </summary>
	/// <param name="flag">An enumeration value.</param>
	/// <returns>true if the bit field or bit fields that are set in flag are also set in the current instance; otherwise, false.</returns>
	public static bool HasFlag(this Enum thisInstance, Enum flag) {
		long setBits = Convert.ToInt64(flag);
		return (Convert.ToInt64(thisInstance) & setBits) == setBits;
	}

}
