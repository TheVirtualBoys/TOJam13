using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Graphic))]
public class TweenUGUIShaderValue  : XMGTween, IMaterialModifier {

	#region Operating Values

	/// <summary>
	/// The name of the shader variable to affect.
	/// </summary>
	[SerializeField, Header("----- Tween UGUI Shader -----")]
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
	/// The current value of the animation variable.
	/// </summary>
	private float animationValue = 0f;

	/// <summary>
	/// Flag that indicates if this animation's needed operating values have been cached or not.
	/// </summary>
	private bool hasCachedValues = false;

	/// <summary>
	/// The material property block to send to the renderer
	/// </summary>
	private Material shaderMaterial = null;

	/// <summary>
	/// The graphic that we are operating on.
	/// </summary>
	private Graphic operatingGraphic = null;

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
			this.operatingGraphic.SetMaterialDirty();
			this.animationValue = value;
		}
	}

	#endregion

	#region TweenAnimation logic

	/// <summary>
	/// Caches the values tween animation needs to operate.
	/// </summary>
	private void CacheValues() {
		this.hasCachedValues = true;
		this.operatingGraphic = this.GetComponent<Graphic>();
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

	#region IMaterialModifier

	/// <summary>
	/// Gets the modified material and passes it along the chain.
	/// </summary>
	/// <returns>The modified material.</returns>
	/// <param name="baseMaterial">Base material.</param>
	public Material GetModifiedMaterial(Material baseMaterial) {
		if (this.shaderMaterial == null) {
			this.shaderMaterial = new Material(baseMaterial);
		}

		this.shaderMaterial.SetFloat(shaderVariableName, this.value);
		return this.shaderMaterial;
	}

	#endregion

}