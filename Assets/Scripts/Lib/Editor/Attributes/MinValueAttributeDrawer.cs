using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(MinValueAttribute))]
public class MinValueAttributeDrawer : PropertyDrawer {

	/// <summary>
	/// Handles drawing the UI for the min value attribute.
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="property">Property.</param>
	/// <param name="label">Label.</param>
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		MinValueAttribute minValue = attribute as MinValueAttribute;

		if (property.propertyType == SerializedPropertyType.Float) {
			float propertyValue = EditorGUI.FloatField(position, label.text, property.floatValue);
			property.floatValue = Mathf.Max(minValue.min, propertyValue);
		} else if (property.propertyType == SerializedPropertyType.Integer) {
			int propertyValue = EditorGUI.IntField(position, label.text, property.intValue);
			property.intValue = Mathf.Max((int)minValue.min, propertyValue);
		} else {
			EditorGUI.LabelField(position, label.text, "Use MinPropertyValue with int or float");
		}
	}

}
