using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// XMG serialized fields controller.
/// A class that enables getting and setting private serialized fields on any MonoBehaviour.
/// </summary>
public class XMGSerializedFieldsController<T> where T : MonoBehaviour {

	#region Data and Accessors

	/// <summary>
	/// The serialized fields map.
	/// </summary>
	private Dictionary<string, FieldInfo> serializedFieldsMap = new Dictionary<string, FieldInfo>();

	/// <summary>
	/// The instance to get and set values.
	/// </summary>
	private T instance = null;

	#endregion

	#region Constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="XMGSerializedFieldsController`1"/> class.
	/// </summary>
	/// <param name="instance">Instance to get and set values.</param>
	public XMGSerializedFieldsController(T instance) {
		DebugUtils.Assert(instance != null, "Instance cannot be null!");

		this.instance = instance;

		System.Type type = typeof(T);
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
		for (int i = 0; i < fields.Length; i++) {
			FieldInfo info = fields[i];
			object[] customAttributes = info.GetCustomAttributes(true);
			if (System.Array.Exists(customAttributes, attr => attr.GetType() == typeof(SerializeField))) {
				this.serializedFieldsMap[info.Name] = info;
			}
		}
	}

	#endregion

	#region Logic

	/// <summary>
	/// Gets the value of the specified field name.
	/// </summary>
	/// <returns>The value.</returns>
	/// <param name="fieldName">Field name.</param>
	public object GetValue(string fieldName) {
		return this.serializedFieldsMap[fieldName].GetValue(this.instance);
	}

	/// <summary>
	/// Sets the value of the specified field name.
	/// </summary>
	/// <returns><c>true</c>, if value was set, <c>false</c> otherwise.</returns>
	/// <param name="fieldName">Field name.</param>
	/// <param name="value">Value to set.</param>
	public bool SetValue(string fieldName, object value) {
		// Using Equals() here instead of != since primitive values will be boxed into objects, and we want to use
		// the actual runtime types to determine equality.
		bool hasChanges = !value.Equals(this.GetValue(fieldName));
		if (hasChanges) {
			this.serializedFieldsMap[fieldName].SetValue(this.instance, value);
		}

		return hasChanges;
	}

	#endregion

}
