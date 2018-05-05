using UnityEngine;
using System.Collections;

/// <summary>
/// This uses an XMGTween to control a shader variable's value.
/// </summary>
public class TweenShaderValue : XMGTween {

	#region Operating Values

	/// <summary>
	/// The name of the shader variable to affect.
	/// </summary>
	[SerializeField]
	private string shaderVariableName = "_AnimationPosition";

	/// <summary>
	/// Value to tweent the variable from
	/// </summary>
	public float from = 0f;

	/// <summary>
	/// Value to tween the variable to.
	/// </summary>
	public float to = 1f;

	/// <summary>
	/// The current value of the animation
	/// </summary>
	private float animationValue = 0f;

	/// <summary>
	/// Flag that indicates if this animation's needed operating values have been cached or not.
	/// </summary>
	private bool hasCachedValues = false;

	/// <summary>
	/// The material property block to send to the renderer
	/// </summary>
	private MaterialPropertyBlock operatingMaterialBlock = null;

	/// <summary>
	/// The renderer to send the material block to and from
	/// </summary>
	private Renderer operatingRenderer = null;

	/// <summary>
	/// Gets or sets the value of the tween.
	/// </summary>
	public float value {
		get {
			if (!this.hasCachedValues) {
				this.CacheValues();
			}
			return this.animationValue;
		}
		set {
			if (!this.hasCachedValues) {
				this.CacheValues();
			}

			if (this.operatingRenderer != null) {
				this.animationValue = value;
				// Update the renderer's animation value by using the renderer's material property block so that we don't spawn a new material and create a new drawcall.
				this.operatingRenderer.GetPropertyBlock(this.operatingMaterialBlock);
				this.operatingMaterialBlock.SetFloat(this.shaderVariableName, value);
				this.operatingRenderer.SetPropertyBlock(this.operatingMaterialBlock);
			}
		}
	}

	#endregion

	#region TweenAnimation logic

	/// <summary>
	/// Caches the values tween animation needs to operate.
	/// </summary>
	private void CacheValues() {
		this.hasCachedValues = true;
		this.operatingRenderer = this.GetComponent<Renderer>();
		if (this.operatingRenderer != null) {
			this.operatingMaterialBlock = new MaterialPropertyBlock();
		}
	}

	#endregion

	#region UITweener Implementation

	/// <summary>
	/// Actual tweening logic should go here.
	/// </summary>
	/// <param name="factor">Factor for how far along the tween is.</param>
	/// <param name="isFinished">If set to <c>true</c> the tween is finished.</param>
	protected override void TweenUpdate(float factor, bool isFinished) {
		this.value = Mathf.Lerp(from, to, factor);
	}

	/// <summary>
	/// Begin a tween animation on the specified game object that will last the given duration animating from the given animation positions.
	/// </summary>
	/// <param name="go">GameObject with the TweenAnimation on it.</param>
	/// <param name="duration">Duration of the tween.</param>
	/// <param name="fromValue">The starting value for the shader variable</param>
	/// <param name="toValue">The ending value of the shader variable</param>
	static public TweenShaderValue Begin(GameObject go, float duration, float fromValue = 0f, float toValue = 1f) {
		TweenShaderValue comp = XMGTween.Begin<TweenShaderValue>(go, duration);
		comp.from = fromValue;
		comp.to = toValue;
		return comp;
	}

	/// <summary>
	/// Set the 'from' value to the current one.
	/// </summary>
	public override void SetStartToCurrentValue() {
		this.from = this.value;
	}

	/// <summary>
	/// Set the 'to' value to the current one.
	/// </summary>
	public override void SetEndToCurrentValue() {
		this.to = this.value;
	}

	#endregion
}
