using UnityEngine;
using System.Collections;

/// <summary>
/// This attribute will give a value a lower bound while allowing it to go as high as it can.
/// </summary>
public class MinValueAttribute : PropertyAttribute {

	/// <summary>
	/// The minimum value that a property with this attribute can have.
	/// </summary>
	public float min = 0f;

	/// <summary>
	/// Initializes a new instance of the <see cref="MinValueAttribute"/> class.
	/// </summary>
	/// <param name="minimumValue">Minimum value.</param>
	public MinValueAttribute(float minimumValue) {
		this.min = minimumValue;
	}
}
