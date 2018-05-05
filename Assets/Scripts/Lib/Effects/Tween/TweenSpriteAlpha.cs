using UnityEngine;
using System.Collections;

public class TweenSpriteAlpha : XMGTween {

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
	private SpriteRenderer sprite = null;

	#endregion

	#region Monobehaviour

	/// <summary>
	/// If there is a sprite renderer on the component when this is added set it as the sprite to colourize.
	/// </summary>
	protected override void Reset() {
		base.Reset();
		this.sprite = this.GetComponent<SpriteRenderer>();
	}

	#endregion

	#region Tween Colour update

	/// <summary>
	/// Overload to supply actual tweening logic.
	/// </summary>
	/// <param name="sampleValue">The sampling value of this Update.</param>
	/// <param name="isFinished">Flag indicating if the tween is finishing this frame.</param>
	protected override void TweenUpdate(float sampleValue, bool isFinished) {
		Color spriteColour = this.sprite.color;
		spriteColour.a = Mathf.Lerp(this.startAlpha, this.endAlpha, sampleValue);
		this.sprite.color = spriteColour;
	}

	#endregion
}

