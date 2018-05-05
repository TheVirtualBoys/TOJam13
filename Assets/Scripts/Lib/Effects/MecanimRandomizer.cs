using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MecanimController))]
public class MecanimRandomizer : MonoBehaviour {

	#region Data

	/// <summary>
	/// The minimum value for the randomization change.
	/// </summary>
	[SerializeField, Range(0, 1)]
	public float minRange = 0f;

	/// <summary>
	/// The maximum value for the animation change.
	/// </summary>
	[SerializeField, Range(0, 1)]
	public float maxRange = 1f;

	#endregion

	#region Monobehaviour

	/// <summary>
	/// Set the playback time on the default state to a random time.
	/// </summary>
	private void OnEnable() {
		this.GetComponent<MecanimController>().SetAnimatorTime(Random.Range(minRange, maxRange));
	}

	#endregion
}
