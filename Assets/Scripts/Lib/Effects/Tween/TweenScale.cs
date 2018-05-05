using UnityEngine;
using System.Collections;

public class TweenScale : XMGTween {

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
	public Vector3 fromScale = Vector3.zero;

	/// <summary>
	/// The destination vector.
	/// </summary>
	[SerializeField]
	public Vector3 toScale = Vector3.zero;

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
	/// <param name="destScale">The vector to use for the scale.</param>
	public static TweenScale Begin(GameObject go, float duration, Vector3 destScale) {
		TweenScale tweenScale = XMGTween.Begin<TweenScale>(go, duration);
		tweenScale.operatingTransform = go.transform;
		tweenScale.fromScale = go.transform.localScale;
		tweenScale.toScale = destScale;

		// If the duration is less than 0 immediately move to the destination scale and disable the tween.
		if (duration <= 0f) {
			tweenScale.TweenUpdate(1f, true);
			tweenScale.enabled = false;
		}

		return tweenScale;
	}

	/// <summary>
	/// Update the scale of the tweened object.
	/// </summary>
	/// <param name="sampleValue">The sampling value of this Update.</param>
	/// <param name="isFinished">Flag indicating if the tween is finishing this frame.</param>
	protected override void TweenUpdate(float sampleValue, bool isFinished) {
		Vector3 lerpedVector = Vector3.Lerp(this.fromScale, this.toScale, sampleValue);
		this.operatingTransform.localScale = lerpedVector;

	}

	/// <summary>
	/// Set the start value of the tweening variable to the current value.
	/// </summary>
	public override void SetStartToCurrentValue() {
		this.fromScale = this.operatingTransform.localScale;
	}

	/// <summary>
	/// Sets the end to current value.
	/// </summary>
	public override void SetEndToCurrentValue() {
		this.toScale = this.operatingTransform.localScale;
	}

	#endregion
}