using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TweenGraphicAlpha : XMGTween {

	#region Data

	/// <summary>
	/// The colour that corresponds with the start of the tween.
	/// </summary>
	[SerializeField, Header("----- Tween Colour -----")]
	private float startAlpha = 1f;

	/// <summary>
	/// The colour that corresponds to the end of the tween.
	/// </summary>
	[SerializeField]
	private float endAlpha = 1f;

	/// <summary>
	/// The sprite to colourize.
	/// </summary>
	[SerializeField]
	private Graphic graphic = null;

	#endregion

	#region Monobehaviour

	/// <summary>
	/// If there is a sprite renderer on the component when this is added set it as the sprite to colourize.
	/// </summary>
	protected override void Reset() {
		base.Reset();
		this.graphic = this.GetComponent<Graphic>();
	}

	#endregion

	#region Tween Colour update

	/// <summary>
	/// Overload to supply actual tweening logic.
	/// </summary>
	/// <param name="sampleValue">The sampling value of this Update.</param>
	/// <param name="isFinished">Flag indicating if the tween is finishing this frame.</param>
	protected override void TweenUpdate(float sampleValue, bool isFinished) {
		Color graphicColour = this.graphic.color;
		graphicColour.a = Mathf.Lerp(this.startAlpha, this.endAlpha, sampleValue);
		this.graphic.color = graphicColour;
	}

	#endregion
}
