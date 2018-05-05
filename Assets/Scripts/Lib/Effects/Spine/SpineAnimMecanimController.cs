using Spine;
using Spine.Unity;
using UnityEngine;

/// <summary>
/// This class manages a spine skeleton animation using the Mecanim Controller methods. 
/// This is used for general Spine animations.
/// </summary>
[RequireComponent(typeof(SkeletonAnimation))]
public class SpineAnimMecanimController : AbstractSpineMecanimController {

    #region Data

    /// <summary>
    /// The Spine SkeletonAnimation that this class is managing.
    /// </summary>
    [SerializeField]
    private SkeletonAnimation spineAnimation = null;

    /// <summary>
    /// Gets the Spine.Animation state of the SkeletonAnimation that this class is managing.
    /// </summary>
    public override Spine.AnimationState AnimatorState {
        get {
            return this.spineAnimation.AnimationState;
        }
    }

    /// <summary>
    /// Gets the Skeleton data from the SkeletonAnimation that this class is managing.
    /// </summary>
    public override Skeleton AnimationSkeleton {
        get {
            return this.spineAnimation.Skeleton;
        }
    }

    #endregion

    #region Mecanim Controller

    /// <summary>
    /// Helper function that sets up the correct starting state of the animator.
    /// This will also grab the SkeletonGraphic for this object.
    /// </summary>
    protected override void SetupAnimator() {
        base.SetupAnimator();
        this.spineAnimation = this.GetComponent<SkeletonAnimation>();
    }

    #endregion

    #region AbstractSpineMecanimController 

    /// <summary>
    /// Give manual access to internal animation update
    /// </summary>
    /// <param name="deltaTime">delta time</param>
    public override void UpdateSpine(float deltaTime) {
        this.spineAnimation.Update(deltaTime);
    }

    #endregion
    
}

