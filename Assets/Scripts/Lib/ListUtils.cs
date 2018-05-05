using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// List and IList<T> extension methods and list helpers.
/// </summary>
public static class ListUtils {
	
	/// <summary>
	/// Fisher-Yates shuffle as per http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp.
	/// Implemented as an extension method.
	/// </summary>
	/// <param name='list'>List that should be shuffled.</param>
	/// <typeparam name='T'>The type parameter of the list to be shuffled.</typeparam>
	public static void Shuffle<T>(this IList<T> list) {
		int n = list.Count;
		while (n > 1) {
			int k = Random.Range(0, n);
			--n;
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	/// <summary>
	/// Merge this list with another, optionally only add unique entries.
	/// </summary>
	/// <param name="lhs">A List.</param>
	/// <param name="rhs">Another list.</param>
	/// <param name="unique">Only add unique entries from RHS to LHS.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void Merge<T>(this IList<T> lhs, IList<T> rhs, bool unique) {
		for (int i = 0; i < rhs.Count; ++i) {
			if (!unique || (unique && !lhs.Contains(rhs[i]))) {
				lhs.Add(rhs[i]);
			}
		}
	}

	/// <summary>
	/// Standard three part swap of the items at the two given indexes.
	/// </summary>
	/// <param name="list">List.</param>
	/// <param name="indexA">Index of the first item to swap.</param>
	/// <param name="indexB">Index of the second item to swap.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void Swap<T>(this IList<T> list, int indexA, int indexB) {
		T temp = list[indexB];
		list[indexB] = list[indexA];
		list[indexA] = temp;
	}

	/// <summary>
	/// Gets a random element from the list.
	/// </summary>
	/// <returns>A random element of the list.</returns>
	/// <param name="list">List.</param>
	public static T RandomElement<T>(this IList<T> list) {
		if (list.Count == 0) {
			return default(T);
		}

		return list[Random.Range(0, list.Count)];
	}

	/// <summary>
	/// Allows negative and out of bounds values to be indexed into the list.
	/// </summary>
	/// <returns>An element in the list at the wrapped index.</returns>
	/// <param name="list">The list you'd like to index into.</param>
	/// <param name="index">The (possibly) out of bounds index.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T WrapIndex<T>(this IList<T> list, int index) {
		return list[list.GetWrapIndex<T>(index)];
	}

	/// <summary>
	/// Gets the index that should be wrapped into given a possibly negative or OOB value.
	/// </summary>
	/// <returns>The wrap index.</returns>
	/// <param name="list">The list we're indexing into.</param>
	/// <param name="index">The (possibly) out of bounds index.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static int GetWrapIndex<T>(this IList<T> list, int index) {
		if (list.Count == 0) {
			throw new System.IndexOutOfRangeException("Can't wrap index on an empty array.");
		}

		// Circular indexing operation.
		return ((index % list.Count) + list.Count) % list.Count;
	}

	/// <summary>
	/// This method performs a linear O(n) search over the list and returns true if any element matching the predicate is contained in the list.
	/// Otherwise it will return false.
	/// </summary>
	/// <param name="list">List.</param>
	/// <param name="match">The Predicate<T> delegate that defines the conditions of the element to search for.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static bool Contains<T>(this IList<T> list, System.Predicate<T> match) {
		if (match == null) {
			throw new System.ArgumentNullException("match", "The predicate passed in was null.");
		}

		for (int i = 0; i < list.Count; i++) {
			if (match(list[i])) {
				return true;
			}
		}

		return false;
	}
}
