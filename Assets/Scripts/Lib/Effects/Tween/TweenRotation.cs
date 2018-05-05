using UnityEngine;
using System.Collections;

public class TweenRotation : XMGTween {

	#region Data

	/// <summary>
	/// The transform to tween on.
	/// </summary>
	[SerializeField]
	private Transform operatingTransform = null;

	/// <summary>
	/// The origin vector.
	/// </summary>
	[SerializeField]
	public Vector3 fromRotation = Vector3.zero;

	/// <summary>
	/// The destination vector.
	/// </summary>
	[SerializeField]
	public Vector3 toRotation = Vector3.zero;

	#endregion

	#region Monobehaviour

	/// <summary>
	/// Default the operating transform to be on this object's transform.
	/// </summary>
	protected override void Reset() {
		this.operatingTransform = this.gameObject.transform;
		base.Reset();
	}

	#endregion

	#region XMGTween

	/// <summary>
	/// Begin the tween with the specified gameObject, duration, position and worldSpace.
	/// </summary>
	/// <param name="go">Game object to set as the tween.</param>
	/// <param name="duration">Duration.</param>
	/// <param name="desinationRotation">The vector to use for destination of the scale.</param>
	public static TweenRotation Begin(GameObject go, float duration, Vector3 destinationRotation) {
		TweenRotation tweenRotation = XMGTween.Begin<TweenRotation>(go, duration);
		tweenRotation.operatingTransform = go.transform;
		tweenRotation.fromRotation = go.transform.localEulerAngles;
		tweenRotation.toRotation = destinationRotation;

		// If the duration is less than 0 immediately move to the destination scale and disable the tween.
		if (duration <= 0f) {
			tweenRotation.TweenUpdate(1f, true);
			tweenRotation.enabled = false;
		}

		return tweenRotation;
	}

	/// <summary>
	/// Update the position of
	/// </summary>
	/// <param name="sampleValue">The sampling value of this Update.</param>
	/// <param name="isFinished">Flag indicating if the tween is finishing this frame.</param>
	protected override void TweenUpdate(float sampleValue, bool isFinished) {
		Vector3 lerpedVector = Vector3.Lerp(this.fromRotation, this.toRotation, sampleValue);
		this.operatingTransform.localEulerAngles = lerpedVector;
	}

	/// <summary>
	/// Set the start value of the tweening variable to the current value.
	/// </summary>
	public override void SetStartToCurrentValue() {
		this.fromRotation = this.operatingTransform.localEulerAngles;
	}

	/// <summary>
	/// Sets the end to current value.
	/// </summary>
	public override void SetEndToCurrentValue() {
		this.toRotation = this.operatingTransform.localEulerAngles;
	}

	#endregion
}