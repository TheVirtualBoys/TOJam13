using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(LayoutElement))]
public class TweenPreferredDimensions : XMGTween {

	#region Data

	/// <summary>
	/// The size to tween from.
	/// </summary>
	[SerializeField]
	public Vector2 fromPreferredDimensions = Vector2.zero;

	/// <summary>
	/// The preferred dimensions to tween to.
	/// </summary>
	[SerializeField]
	public Vector2 toPreferredDimensions = Vector2.zero;

	/// <summary>
	/// The component's layout element.
	/// </summary>
	[SerializeField]
	public LayoutElement componentLayoutElement = null;

	#endregion

	#region Monobehaviour

	/// <summary>
	/// Default the operating transform to be on this object's transform.
	/// </summary>
	protected override void Reset() {
		this.componentLayoutElement = this.GetComponent<LayoutElement>();
		this.fromPreferredDimensions = new Vector2(this.componentLayoutElement.preferredWidth, this.componentLayoutElement.preferredHeight);
		base.Reset();
	}

	#endregion

	#region XMGTween

	/// <summary>
	/// Begin a tween on the specified gameobject with a given duration and destination dimensions.
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="duration">Duration.</param>
	/// <param name="destPreferredDimensions">Destination preferred dimensions.</param>
	public static TweenPreferredDimensions Begin(GameObject go, float duration, Vector2 destPreferredDimensions) {
		TweenPreferredDimensions tweenScale = XMGTween.Begin<TweenPreferredDimensions>(go, duration);
		tweenScale.componentLayoutElement = go.GetComponent<LayoutElement>();
		tweenScale.fromPreferredDimensions = new Vector2(tweenScale.componentLayoutElement.preferredWidth, tweenScale.componentLayoutElement.preferredHeight);
		tweenScale.toPreferredDimensions = destPreferredDimensions;

		// If the duration is less than 0 immediately move to the destination dimenstions and disable the tween.
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
		Vector2 lerpedVector = Vector2.Lerp(this.fromPreferredDimensions, this.toPreferredDimensions, sampleValue);
		this.componentLayoutElement.preferredWidth = lerpedVector.x;
		this.componentLayoutElement.preferredHeight = lerpedVector.y;
	}

	/// <summary>
	/// Set the start value of the tweening variable to the current value.
	/// </summary>
	public override void SetStartToCurrentValue() {
		this.fromPreferredDimensions = new Vector2(this.componentLayoutElement.preferredWidth, this.componentLayoutElement.preferredHeight);
	}

	/// <summary>
	/// Sets the end to current value.
	/// </summary>
	public override void SetEndToCurrentValue() {
		this.toPreferredDimensions = new Vector2(this.componentLayoutElement.preferredWidth, this.componentLayoutElement.preferredHeight);
	}

	#endregion

}
