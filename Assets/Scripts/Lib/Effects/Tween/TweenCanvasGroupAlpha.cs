using UnityEngine;
using System.Collections;

public class TweenCanvasGroupAlpha : XMGTween {

	#region Data

	/// <summary>
	/// The starting alpha value for this tween.
	/// </summary>
	[SerializeField, Header("----- Canvas Group -----"), Range(0f, 1f)]
	private float startAlpha = 1f;

	/// <summary>
	/// The end alpha value for this tween.
	/// </summary>
	[SerializeField, Range(0f, 1f)]
	private float endAlpha = 1f;

	/// <summary>
	/// The CanvasGroup that should have it's alpha tweened.
	/// </summary>
	[SerializeField]
	private CanvasGroup canvasGroup = null;

	#endregion

	#region Monobehaviour

	/// <summary>
	/// Ensure we have a canvas group set.
	/// </summary>
	private void Awake() {
		Debug.Assert(this.canvasGroup != null, "Canvas group is null.", this);
	}

	/// <summary>
	/// When added to a component call the set values and try to find our tweening object automatically.
	/// </summary>
	protected override void Reset() {
		base.Reset();
		this.canvasGroup = this.GetComponent<CanvasGroup>();
	}

	#endregion

	#region TweenCanvasGroup Update

	/// <summary>
	/// Calculate the canvas alpha based on the XMGTween update logic.
	/// </summary>
	/// <param name="sampleValue">The sampling value of this Update.</param>
	/// <param name="isFinished">Flag indicating if the tween is finishing this frame.</param>
	protected override void TweenUpdate(float sampleValue, bool isFinished) {
		if (this.canvasGroup != null) {
			this.canvasGroup.alpha = Mathf.Lerp(this.startAlpha, this.endAlpha, sampleValue);
		}
	}

	#endregion

}
