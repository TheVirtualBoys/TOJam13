using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class will override 
/// </summary>
public class MecanimStateOverride : MonoBehaviour {

	#region Enum

	/// <summary>
	/// Enumration of states we can override in.
	/// </summary>
	private enum UnityState {
		Awake,
		Start,
		Enable,
	}

	/// <summary>
	/// Enumeration of types we support.
	/// </summary>
	private enum MecanimParamType {
		Trigger,
		Bool 
	}

	#endregion

	#region Inner Class

	[Serializable]
	private class MecanimOverride {

		/// <summary>
		/// The state to apply the override in.
		/// </summary>
		public UnityState overrideState = UnityState.Enable;

		/// <summary>
		/// The type of the param we are setting.
		/// </summary>
		public MecanimParamType overrideType = MecanimParamType.Trigger;

		/// <summary>
		/// The name of the parameter to override.
		/// </summary>
		public string paramName = "";
	}

	#endregion

	#region Data

	/// <summary>
	/// The controller for the values being overridden.
	/// </summary>
	[SerializeField]
	private MecanimController controller = null;

	/// <summary>
	/// List of overrides.
	/// </summary>
	[SerializeField]
	private List<MecanimOverride> overrideList = null;

	#endregion

	#region Monobehaviour

	/// <summary>
	/// Set variables on awake.
	/// </summary>
	private void Awake() {
		for (int i = 0; i < this.overrideList.Count; i++) {
			if (this.overrideList[i].overrideState == UnityState.Awake) {
				this.ApplyOverride(this.overrideList[i]);
			}
		}
	}

	/// <summary>
	/// Set variables on start.
	/// </summary>
	private void Start () {
		for (int i = 0; i < this.overrideList.Count; i++) {
			if (this.overrideList[i].overrideState == UnityState.Start) {
				this.ApplyOverride(this.overrideList[i]);
			}
		}
	}

	/// <summary>
	/// Sets the variables on enable.
	/// </summary>
	private void OnEnable() {
		for (int i = 0; i < this.overrideList.Count; i++) {
			if (this.overrideList[i].overrideState == UnityState.Enable) {
				this.ApplyOverride(this.overrideList[i]);
			}
		}
	}

	#endregion

	#region Helpers

	/// <summary>
	/// Applies the override specified.
	/// </summary>
	/// <param name="mecanimOverride">Mecanim override.</param>
	private void ApplyOverride(MecanimOverride mecanimOverride) {
		if (mecanimOverride.overrideType == MecanimParamType.Trigger) {
			this.controller.Trigger(mecanimOverride.paramName);
		} else if (mecanimOverride.overrideType == MecanimParamType.Bool) {
			this.controller.SetTrue(mecanimOverride.paramName);
		}
	}

	#endregion
}
