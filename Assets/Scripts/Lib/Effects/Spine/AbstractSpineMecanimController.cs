using UnityEngine;
using Spine.Unity;

/// <summary>
/// The base class for controlling spine animations through the same interface as Mecanim animations.
/// </summary>
public abstract class AbstractSpineMecanimController : MecanimController {

    #region Data

    /// <summary>
    /// Gets the AnimationState of the managed Spine animation object.
    /// </summary>
    public abstract Spine.AnimationState AnimatorState {
        get;
    }

    /// <summary>
    /// Gets the Spine.Skeleton of the managed Spine animation object.
    /// </summary>
    public abstract Spine.Skeleton AnimationSkeleton {
        get;
    }

    [SerializeField, Header("----- Spine -----")]
    protected bool flipX = false;
    /// <summary>
    /// Flag inidicating that the animation is currently flipped on it's X axis.
    /// </summary>
    public bool FlipX {
        get {
            return this.flipX;
        }
        set {
            this.flipX = value;
            this.UpdateFlipState();
        }
    }

    [SerializeField]
    protected bool flipY = false;
    /// <summary>
    /// Flag indicating that the animation is currently flipped on it's Y axis.
    /// </summary>
    public bool FlipY {
        get {
            return this.flipY;
        }

        set {
            this.flipY = value;
            this.UpdateFlipState();
        }
    }

    #endregion

    #region Monobehaviour

    /// <summary>
    /// Iniitalize the flip state of the animation here.
    /// </summary>
    protected new void Start() {
        base.Start();
        this.UpdateFlipState();
    }

    #endregion

    #region Timing Functions

    /// <summary>
    /// Sets the current animation on the default layer to a specific time ID.
    /// </summary>
    public override void SetAnimatorTime(float timeValue) {
        float animationDuration = this.AnimatorState.GetCurrent(0).Animation.Duration;
        this.AnimatorState.GetCurrent(0).TrackTime = animationDuration * timeValue;
	}

	/// <summary>
	/// Sets the animator time in a specified state in an optional layer.
	/// </summary>
	/// <param name="stateName">Name of the state to set.</param>
	/// <param name="timeValue">Normalized time value to set the state to.</param>
	/// <param name="layerID">ID of the layer this state is found on.</param>
	public override void SetAnimatorTime(string stateName, float timeValue, int layerID = -1) {
		this.Anim.Play(stateName, layerID);

		// In unity a negative value will get the default layer automagically while in Spine the default layer is always 0.
		if (layerID < 0) {
			layerID = 0;
		}

        float animationDuration = this.AnimatorState.GetCurrent(layerID).Animation.Duration;
        this.AnimatorState.GetCurrent(layerID).TrackTime = animationDuration * timeValue;
	}

    #endregion

    #region AbstractSpineMecanimController

    /// <summary>
    /// Sets the spine skin for this animation.
    /// </summary>
    /// <param name="skinID">Skin identifier.</param>
    public void SetAnimationSkin(string skinID) {
        this.AnimationSkeleton.SetSkin(skinID);
    }

    /// <summary>
    /// Update the flip state of the managed animation.
    /// </summary>
    private void UpdateFlipState() {
        this.AnimationSkeleton.FlipX = this.flipX;
        this.AnimationSkeleton.FlipY = this.flipY;
    }

    /// <summary>
    /// Give manual access to internal animation update
    /// </summary>
    /// <param name="deltaTime">delta time</param>
    public abstract void UpdateSpine(float deltaTime);

	#endregion

}

