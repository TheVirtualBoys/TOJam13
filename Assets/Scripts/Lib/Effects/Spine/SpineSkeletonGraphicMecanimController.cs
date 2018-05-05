using Spine;
using Spine.Unity;
using UnityEngine;

/// <summary>
/// This class represents a spine mechanim controller that uses a skeleton graphic instead of a skeleton animation.
/// This is used for Spine animation in the UI.
/// </summary>
[RequireComponent(typeof(SkeletonGraphic))]
public class SpineSkeletonGraphicMecanimController : AbstractSpineMecanimController {

    #region Data

    /// <summary>
    /// The spine skeleton graphic.
    /// </summary>
    [SerializeField]
    private SkeletonGraphic spineSkeletonGraphic = null;

    /// <summary>
    /// Gets the state of the animator.
    /// </summary>
    public override Spine.AnimationState AnimatorState {
        get {
            return this.spineSkeletonGraphic.AnimationState;
        }
    }

    /// <summary>
    /// Gets the animation skeleton for the managed Spine skeleton.
    /// </summary>
    public override Skeleton AnimationSkeleton {
        get {
            return this.spineSkeletonGraphic.Skeleton;
        }
    }

    #endregion

    #region Mecanim controller

    /// <summary>
    /// Helper function that sets up the correct starting state of the animator.
    /// This will also grab the SkeletonGraphic for this object.
    /// </summary>
    protected override void SetupAnimator() {
        base.SetupAnimator();
        this.spineSkeletonGraphic = this.GetComponent<SkeletonGraphic>();
    }

    #endregion

    #region AbstractSpineMecanimController 

    /// <summary>
    /// Give manual access to internal animation update
    /// </summary>
    /// <param name="deltaTime">delta time</param>
    public override void UpdateSpine(float deltaTime) {
        this.spineSkeletonGraphic.Update(deltaTime);
    }

    #endregion
    
}

