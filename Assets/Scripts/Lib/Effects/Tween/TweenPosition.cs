using UnityEngine;
using System.Collections;

public class TweenPosition : XMGTween {

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
	public Vector3 fromVector = Vector3.zero;

	/// <summary>
	/// The destination vector.
	/// </summary>
	[SerializeField]
	public Vector3 toVector = Vector3.zero;

	/// <summary>
	/// If set to true the vectors will be in world space, if not the vectors will be treated as local vectors.
	/// </summary>
	[SerializeField]
	private bool inWorldSpace = false;

	#endregion

	#region Monobehaviour

	/// <summary>
	/// Default the operating transform to be on this object's transform.
	/// </summary>
	protected override void Reset() {
		this.operatingTransform = this.gameObject.transform;
		this.SetStartToCurrentValue();
		base.Reset();
	}

	#endregion

	#region XMGTween

	/// <summary>
	/// Begin the tween with the specified gameObject, duration, position and worldSpace.
	/// </summary>
	/// <param name="go">Game object to set as the tween.</param>
	/// <param name="duration">Duration.</param>
	/// <param name="pos">Position.</param>
	/// <param name="worldSpace">If set to <c>true</c> the tween will be in world space.</param>
	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace = false) {
		TweenPosition tweenPos = XMGTween.Begin<TweenPosition>(go, duration);
		tweenPos.operatingTransform = go.transform;
		tweenPos.inWorldSpace = worldSpace;
		tweenPos.fromVector = (worldSpace) ? tweenPos.transform.position : tweenPos.transform.localPosition;
		tweenPos.toVector = pos;

		// If the duration is less than 0 immediately move to the destination position and disable the tween.
		if (duration <= 0f) {
			tweenPos.TweenUpdate(1f, true);
			tweenPos.enabled = false;
		}

		return tweenPos;
	}


	/// <summary>
	/// Update the position of
	/// </summary>
	/// <param name="sampleValue">The sampling value of this Update.</param>
	/// <param name="isFinished">Flag indicating if the tween is finishing this frame.</param>
	protected override void TweenUpdate(float sampleValue, bool isFinished) {
		Vector3 lerpedVector = Vector3.LerpUnclamped(this.fromVector, this.toVector, sampleValue);

		if (this.inWorldSpace) {
			this.operatingTransform.position = lerpedVector;
		} else {
			this.operatingTransform.localPosition = lerpedVector;
		}
	}

	/// <summary>
	/// Set the start value of the tweening variable to the current value.
	/// </summary>
	public override void SetStartToCurrentValue() {
		this.fromVector = (this.inWorldSpace) ? this.operatingTransform.position : this.operatingTransform.localPosition;
	}

	/// <summary>
	/// Sets the end to current value.
	/// </summary>
	public override void SetEndToCurrentValue() {
		this.toVector = (this.inWorldSpace) ? this.operatingTransform.position : this.operatingTransform.localPosition;
	}

	#endregion
}
