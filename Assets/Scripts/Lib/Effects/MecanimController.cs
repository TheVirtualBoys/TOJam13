using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class MecanimController : MonoBehaviour {

	#region Data

	[SerializeField, Header("----- Mecanim controller -----")]
	private Animator myAnim = null;
	/// <summary>
	/// The animator representing the mechanim animations that this is controlling.
	/// </summary>
	/// <value>The animation.</value>
	public Animator Anim {
		get {
			return this.myAnim;
		}
	}

	/// <summary>
	/// If true overrides the update mode when in game.
	/// </summary>
	[SerializeField]
	private bool overrideUpdateInGameMode = false;

	/// <summary>
	/// Update mode when we're in gameplay and not in the unity editor.
	/// This gets around a crash bug in unity 4.6 where the editor would crash if you had the culling mode based on renderers.
	/// TODO: Revisit this when we upgrade to 5.0.
	/// </summary>
	[SerializeField]
	private AnimatorCullingMode updateModeGameOverride = AnimatorCullingMode.CullUpdateTransforms;

	private Dictionary<string, Action> eventDict = null;
	/// <summary>
	/// List of actions grouped by action tag.
	/// </summary>
	private Dictionary<string, Action> EventDict {
		get {
			if (this.eventDict == null) {
				this.eventDict = new Dictionary<string, Action>();
			}

			return this.eventDict;
		}
	}

    private Dictionary<int, Action> stateEnteredEventDict = null;
	/// <summary>
	/// List of actions upon entering a state grouped by the state name hash.
	/// </summary>
	private Dictionary<int, Action> StateEnteredEventDict {
		get {
			if (this.stateEnteredEventDict == null) {
				this.stateEnteredEventDict = new Dictionary<int, Action>();
			}
			return this.stateEnteredEventDict;
		}
	}

    private Dictionary<int, Action> stateExitedEventDict = null;
	/// <summary>
	/// List of actions upon entering a state grouped by the state name hash.
	/// </summary>
	private Dictionary<int, Action> StateExitedEventDict {
		get {
			if (this.stateExitedEventDict == null) {
				this.stateExitedEventDict = new Dictionary<int, Action>();
			}
			return this.stateExitedEventDict;
		}
	}

	#endregion

	#region Monobehaviour

	/// <summary>
	/// Awake this instance.
	/// </summary>
	protected void Awake() {
		// If the event dictionary is not set up yet set it up here.
		if (this.eventDict == null) {
			this.eventDict = new Dictionary<string, Action>();
		}
        if (this.stateEnteredEventDict == null) {
            this.stateEnteredEventDict = new Dictionary<int, Action>();
        }
        if (this.stateExitedEventDict == null) {
            this.stateExitedEventDict = new Dictionary<int, Action>();
        }
		this.SetupAnimator();

		Debug.Assert(this.myAnim != null, string.Format("Mecanim controller on {0} does not have an animator attached.", this.gameObject.name));
	}

    /// <summary>
    /// Start this instance.
    /// </summary>
    protected void Start() {
    }

    /// <summary>
    /// Component became active.
    /// </summary>
    protected void OnEnable() {
        // Hooking up to behaviours needs to be done in OnEnable, because behaviours are re-instantiated when the animator becomes re-activated
        MecanimBasicStateBehaviour[] basicBehaviours = this.myAnim.GetBehaviours<MecanimBasicStateBehaviour>();
        foreach (MecanimBasicStateBehaviour behaviour in basicBehaviours) {
            behaviour.OnStateEntered -= this.HandleStateEntered;
            behaviour.OnStateExited -= this.HandleStateExited;
            behaviour.OnStateEntered += this.HandleStateEntered;
            behaviour.OnStateExited += this.HandleStateExited;
        }
    }

	/// <summary>
	/// Try and grab the animator that is in the same gameobject as the mecanim controller.
	/// </summary>
	protected void Reset() {
		this.SetupAnimator();
	}

	#endregion

	#region Controller Logic
	
	/// <summary>
	/// Handler for an event that happens when a mecanim animation ends.
	/// </summary>
	/// <param name="eventTag">The tag for the event.</param>
	public void MecanimAnimationEvent(string eventTag) {
		Action associatedEvents = null;
		if (this.EventDict.TryGetValue(eventTag, out associatedEvents)) {
			if (associatedEvents != null) {
				associatedEvents.Invoke();
			}
		} 
	}

	/// <summary>
	/// Associates the event with this other events on that tag.
	/// </summary>
	/// <param name="eventTag">Event tag.</param>
	/// <param name="action">Action to add.</param>
	public void AddEvent(string eventTag, Action action) {
		Action associatedEvents = null;
		if (this.EventDict.TryGetValue(eventTag, out associatedEvents)) {
			this.EventDict[eventTag] = associatedEvents + action;
		} else {
			this.EventDict[eventTag] = action;
		}
	}

	/// <summary>
	/// Removes the event from the list of events on that tag.
	/// </summary>
	/// <param name="eventTag">Event tag.</param>
	/// <param name="action">Action to remove.</param>
	public void RemoveEvent(string eventTag, Action action) {
		Action associatedEvents = null;
		if (this.EventDict.TryGetValue(eventTag, out associatedEvents)) {
            this.EventDict[eventTag] = associatedEvents - action;
		}
	}

    /// <summary>
	/// Removes all events under a given event tag.
	/// </summary>
	/// <param name="eventTag">Event tag.</param>
	public void RemoveAllEvents(string eventTag) {
		this.EventDict.Remove(eventTag);
	}

    /// <summary>
    /// Add a callback for a state being entered. The state must have a MecanimBasicStateBehaviour on it for this to work.
    /// </summary>
    /// <param name="stateName">The state name</param>
    /// <param name="action">The callback</param>
    public void AddStateEnteredEvent(string stateName, Action action) {
        int hash = Animator.StringToHash(stateName);
        Action associatedEvents = null;
		if (this.StateEnteredEventDict.TryGetValue(hash, out associatedEvents)) {
			this.StateEnteredEventDict[hash] = associatedEvents + action;
		} else {
			this.StateEnteredEventDict[hash] = action;
		}
    }

    /// <summary>
	/// Removes a callback for a state being entered.
	/// </summary>
	/// <param name="stateName">The state name</param>
    /// <param name="action">The callback</param>
	public void RemoveStateEnteredEvent(string stateName, Action action) {
        int hash = Animator.StringToHash(stateName);
		Action associatedEvents = null;
		if (this.StateEnteredEventDict.TryGetValue(hash, out associatedEvents)) {
            this.StateEnteredEventDict[hash] = associatedEvents - action;
		}
	}

    /// <summary>
    /// Add a callback for a state being exited. The state must have a MecanimBasicStateBehaviour on it for this to work.
    /// </summary>
    /// <param name="stateName">The state name</param>
    /// <param name="action">The callback</param>
    public void AddStateExitedEvent(string stateName, Action action) {
        int hash = Animator.StringToHash(stateName);
        Action associatedEvents = null;
		if (this.StateExitedEventDict.TryGetValue(hash, out associatedEvents)) {
			this.StateExitedEventDict[hash] = associatedEvents + action;
		} else {
			this.StateExitedEventDict[hash] = action;
		}
    }

    /// <summary>
	/// Removes a callback for a state being exited.
	/// </summary>
	/// <param name="stateName">The state name</param>
    /// <param name="action">The callback</param>
	public void RemoveStateExitedEvent(string stateName, Action action) {
        int hash = Animator.StringToHash(stateName);
		Action associatedEvents = null;
		if (this.StateExitedEventDict.TryGetValue(hash, out associatedEvents)) {
            this.StateExitedEventDict[hash] = associatedEvents - action;
		}
	}

	/// <summary>
	/// This registers and event in such a way that it will be called only once and then removed from the event call list.
	/// </summary>
	/// <param name="eventTag">Event tag to bind the event to.</param>
	/// <param name="action">Action delegate for the event that should be performed.</param>
	public void RegisterOneShot(string eventTag, Action action) {
		Debug.Assert(action != null, "A one shot action cannot be null.");

		Action oneShotAction = null;
		oneShotAction = delegate() {
			action();
			this.RemoveEvent(eventTag, oneShotAction);
		};

		this.AddEvent(eventTag, oneShotAction);
	}

    /// <summary>
    /// Handle StateEntered event coming from MecanimBasicStateBehaviour
    /// </summary>
    /// <param name="stateNameHash">Name hash of the entered state</param>
    /// <param name="layerIndex">Layer index of the entered state</param>
    private void HandleStateEntered(int stateNameHash, int layerIndex) {
        Action associatedEvents = null;
		if (this.StateEnteredEventDict.TryGetValue(stateNameHash, out associatedEvents)) {
			associatedEvents.Invoke();
		} 
    }

    /// <summary>
    /// Handle StateExited event coming from MecanimBasicStateBehaviour
    /// </summary>
    /// <param name="stateNameHash">Name hash of the exited state</param>
    /// <param name="layerIndex">Layer index of the exited state</param>
    private void HandleStateExited(int stateNameHash, int layerIndex) {
        Action associatedEvents = null;
		if (this.StateExitedEventDict.TryGetValue(stateNameHash, out associatedEvents)) {
			associatedEvents.Invoke();
		}
    }

	#endregion

	#region Convenience Functions

	/// <summary>
	/// Sets a float variable to a random number between zero and one.
	/// </summary>
	/// <param name="variableToSet">Name of the float variable to set.</param>
	public void SetRandomFloat(string variableToSet) {
		this.Anim.SetFloat(variableToSet, UnityEngine.Random.value);
	}
	
	/// <summary>
	/// Sets the integer specified to the given integer value.
	/// </summary>
	/// <param name="variableName">Variable name.</param>
	/// <param name="integerValue">Integer value.</param>
	public void SetInt(string variableName, int integerValue) {
		this.Anim.SetInteger(variableName, integerValue);
	}

	/// <summary>
	/// Sets a boolean to true.
	/// </summary>
	/// <param name="variableName">Name of bool to set.</param>
	public void SetTrue(string variableName) {
		this.Anim.SetBool(variableName, true);
	}

	/// <summary>
	/// Sets a boolean to false.
	/// </summary>
	/// <param name="variableName">Name of bool to set.</param>
	public void SetFalse(string variableName) {
		this.Anim.SetBool(variableName, false);
	}

	/// <summary>
	/// Toggles a boolean to it's opposite value.
	/// </summary>
	/// <param name="variableName">Name of bool to toggle.</param>
	public void Toggle(string variableName) {
		this.Anim.SetBool(variableName, !this.Anim.GetBool(variableName));
	}

	/// <summary>
	/// Sets the specified bool to the specified value.
	/// </summary>
	/// <param name="booleanName">Name of the boolean to set the value of.</param>
	/// <param name="valueToSet">Value to set the specified boolean to.</param>
	public void SetBool(string booleanName, bool valueToSet) {
		this.Anim.SetBool(booleanName, valueToSet);
	}

	/// <summary>
	/// Sets a trigger on the animation this controller is controlling
	/// </summary>
	/// <param name="variableName">Parameter name of the Trigger.</param>
	public void Trigger(string variableName) {
		this.Anim.SetTrigger(variableName);
	}

	#endregion

	#region Timing Functions

	/// <summary>
	/// Sets the animator to a specified time value in default state.
	/// </summary>
	public virtual void SetAnimatorTime(float timeValue) {
		this.Anim.Play(0, -1, timeValue);
	}

	/// <summary>
	/// Sets the animator time in a specified state in an optional layer.
	/// </summary>
	/// <param name="stateName">Name of the state to set.</param>
	/// <param name="timeValue">Normalized time value to set the state to.</param>
	/// <param name="layerID">ID of the layer this state is found on.</param>
	public virtual void SetAnimatorTime(string stateName, float timeValue, int layerID = -1) {
		this.Anim.Play(stateName, layerID, timeValue);
	}

	/// <summary>
	/// Sets a random  animation time for the currently playing animation state.
	/// </summary>
	public void SetRandomAnimationTime() {
		this.SetAnimatorTime(UnityEngine.Random.value);
	}

	/// <summary>
	/// Sets a random animation time with a specified state name.
	/// </summary>
	/// <param name="stateName">Name of the state to set this to.</param>
	/// <param name="layerID">ID of the layer the specified state is found on.</param>
	public void SetRandomAnimatorTime(string stateName, int layerID = -1) {
		this.SetAnimatorTime(stateName, UnityEngine.Random.value, layerID);
	}

	#endregion

	#region Helpers

	/// <summary>
	/// Helps to set up the animator.
	/// </summary>
	protected virtual void SetupAnimator() {
		if (this.myAnim == null) {
			this.myAnim = this.gameObject.GetComponent<Animator>();
		}
			
		if (this.overrideUpdateInGameMode && !Application.isEditor) {
			this.myAnim.cullingMode = this.updateModeGameOverride;
		}
	}

	#endregion
}
