using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer {

	/// <summary>
	/// Draw a masking field for the property that this is attached to.
	/// </summary>
	/// <param name="pos">Position of the rect.</param>
	/// <param name="property">Property we are drawing, this is assumed to be an enum.</param>
	/// <param name="label">Label data for the editor field.</param>
	public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
		if (property.propertyType == SerializedPropertyType.Enum) {
			property.intValue = EditorGUI.MaskField(pos, label, property.intValue, property.enumNames);
		} else {
			EditorGUI.LabelField(pos, label.text, "Use EnumFlasAttributeDrawer only with a Flags type enum.");
		}
	}
}
