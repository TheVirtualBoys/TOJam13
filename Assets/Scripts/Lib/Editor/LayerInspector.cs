using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Reflection;

[CustomEditor(typeof(LayerController))]
public class LayerInspector : Editor {

	#region Constants
	
	// Constants for names of the private serialized fields in LayerController.
	private const string FIELD_SORTING_LAYER_NAME = "sortingLayerName";
	private const string FIELD_SORTING_ORDER = "sortingOrder";

	#endregion

	#region Data

	/// <summary>
	/// The sorting layer names.
	/// </summary>
	private string[] sortingLayerNames = null;

	/// <summary>
	/// Reference to the layer controller.
	/// </summary>
	private LayerController controller = null;

	/// <summary>
	/// The serialized fields controller.
	/// </summary>
	private XMGSerializedFieldsController<LayerController> serializedFieldsController = null;

	#endregion

	#region Editor

	/// <summary>
	/// Sets up the required inspector data.
	/// </summary>
	void OnEnable() {
		System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		// GetValue takes two parameters
		//  - first denote the instance of the object to get the property value from - since sortingLayerNames is a
		//    static property, we pass null.
		//  - second denote the list of indices used if the property is a indexed property - since sortingLayerNames is
		//    not one, we pass null.
		this.sortingLayerNames = (string[])sortingLayersProperty.GetValue(null, null);

		this.controller = this.target as LayerController;
		this.serializedFieldsController = new XMGSerializedFieldsController<LayerController>(this.controller);
	}

	/// <summary>
	/// Draws custom inspector.
	/// </summary>
	public override void OnInspectorGUI() {
		int oldSelectedIndex = Array.IndexOf(this.sortingLayerNames, this.serializedFieldsController.GetValue(FIELD_SORTING_LAYER_NAME) as string);
		if (oldSelectedIndex < 0) {
			oldSelectedIndex = 0;
		}

		bool hasChanges = false;

		int newSelectedIndex = EditorGUILayout.Popup("Sorting Layer", oldSelectedIndex, this.sortingLayerNames);
		hasChanges |= this.serializedFieldsController.SetValue(FIELD_SORTING_LAYER_NAME, this.sortingLayerNames[newSelectedIndex]);

		int oldSortingOrder = (int)this.serializedFieldsController.GetValue(FIELD_SORTING_ORDER);
		int sortingOrder = EditorGUILayout.IntField("Order In Layer", oldSortingOrder);
		hasChanges |= this.serializedFieldsController.SetValue(FIELD_SORTING_ORDER, sortingOrder);

		if (hasChanges) {
			Renderer targetRenderer = this.controller.GetComponent<Renderer>();
			targetRenderer.sortingLayerName = this.sortingLayerNames[newSelectedIndex];
			targetRenderer.sortingOrder = sortingOrder;
			EditorUtility.SetDirty(this.controller);
		}
	}

	#endregion
}
