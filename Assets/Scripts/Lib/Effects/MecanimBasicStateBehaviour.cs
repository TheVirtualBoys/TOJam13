using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This state behaviour exposes basic animation informatioon to MecanimController
/// </summary>
public class MecanimBasicStateBehaviour : StateMachineBehaviour {

    #region Delegates and Events

    /// <summary>
    /// Delegate for a state being entered
    /// </summary>
    /// <param name="stateNameHash">Hash of the state name</param>
    /// <param name="layerIndex">Layer index of the state</param>
    public delegate void StateEventDelegate(int stateNameHash, int layerIndex);

    /// <summary>
    /// Called when this state is entered
    /// </summary>
    public event StateEventDelegate OnStateEntered = null;
    
    /// <summary>
    /// Called when this state is exited
    /// </summary>
    public event StateEventDelegate OnStateExited = null;

    #endregion
    
    #region StateMachineBehaviour

    /// <summary>
    /// Called when this state is entered
    /// </summary>
    /// <param name="animator">The animator this state is controlled by</param>
    /// <param name="stateInfo">Information about this state</param>
    /// <param name="layerIndex">The layer index of this state</param>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (this.OnStateEntered != null) {
            this.OnStateEntered(stateInfo.shortNameHash, layerIndex);
        }
    }

    /// <summary>
    /// Called when this state is exited
    /// </summary>
    /// <param name="animator">The animator this state is controlled by</param>
    /// <param name="stateInfo">Information about this state</param>
    /// <param name="layerIndex">The layer index of this state</param>
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if (this.OnStateExited != null) {
            this.OnStateExited(stateInfo.shortNameHash, layerIndex);
        }
    }

    #endregion

}