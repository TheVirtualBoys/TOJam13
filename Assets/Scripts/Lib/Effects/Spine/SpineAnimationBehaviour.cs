using UnityEngine;
using Spine.Unity;
using System.Collections;

public class SpineAnimationBehaviour : StateMachineBehaviour {

	#region Data

	/// <summary>
	/// The animation controller for state machine behaviour.
	/// </summary>
    private AbstractSpineMecanimController animationController = null;

	/// <summary>
	/// The name to use for the Spine animation state that this should transition to.
	/// </summary>
	[SerializeField, Header("----- Spine Integration -----")]
	private string spineAnimationName = string.Empty;

	/// <summary>
	/// Should the spine animation be set to loop or not?
	/// </summary>
	[SerializeField]
	private bool loop = false;

	/// <summary>
	/// Cache of the frame speed during the last update frame.
	/// </summary>
	private float lastFrameSpeed = 0f;

	#endregion

	#region StateMachineBehaviour

	/// <summary>
	/// Check that specified serialized objects are set correctly.
	/// </summary>
	private void OnEnable() {
		Debug.Assert(!string.IsNullOrEmpty(this.spineAnimationName), "There must be an associated spine animation name for this spine animation behaviour.");
	}

	/// <summary>
	/// Function is called on the first frame that this state is played.
	/// </summary>
	/// <param name="animator">Animator monobehaviour that is driving this state machine state.</param>
	/// <param name="stateInfo">Information about the current state of that state machine.</param>
	/// <param name="layerIndex">Index of the layer we are currently animating on.</param>
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        this.animationController = animator.GetComponent<AbstractSpineMecanimController>();
		Debug.Assert(this.animationController != null, string.Format("No SpineMecanimController on object: {0}", animator.name));

        this.animationController.AnimatorState.SetAnimation(layerIndex, this.spineAnimationName, this.loop);
        this.animationController.AnimatorState.TimeScale = animator.speed;
		this.lastFrameSpeed = animator.speed;

        // OnStateEnter happens after Update, which means the animation won't start for another frame unless we need manually update now
        this.animationController.UpdateSpine(0);
	}

	/// <summary>
	/// This function will be called every frame 
	/// </summary>
	/// <param name="animator">Animator monobehaviour that is driving this state machine state.</param>
	/// <param name="stateInfo">Information about the current state of that state machine.</param>
	/// <param name="layerIndex">Index of the layer we are currently animating on.</param>
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (this.animationController.AnimatorState != null && !Mathf.Approximately(this.lastFrameSpeed, animator.speed)) {
            this.animationController.AnimatorState.TimeScale = animator.speed;
            this.lastFrameSpeed = animator.speed;
		}
	}

	#endregion
}

