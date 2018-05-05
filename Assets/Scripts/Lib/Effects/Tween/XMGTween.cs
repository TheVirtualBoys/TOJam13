using UnityEngine;
using System.Collections;

public abstract class XMGTween : MonoBehaviour {

	#region Constants

	/// <summary>
	/// A default amount per delta, this will complete the tween in a single timestep by default.
	/// </summary>
	private const float DEFAULT_AMOUNT_PER_DELTA = 1f;

	#endregion

	#region Enum

	/// <summary>
	/// The possible play modes for XMGTweens.
	/// </summary>
	public enum PlayMode {
		// Play once and then deactivate.
		Once,
		/// <summary>
		/// Loop the animation, by resetting the tweening variable to zero when it reaches 1.
		/// </summary>
		Loop,
		/// <summary>
		/// Ping pong the animation by reversing play direction when it reaches 0 or 1.
		/// </summary>
		PingPong
	}

	#endregion

	#region Data

	/// <summary>
	/// The curve to use for this tween.
	/// </summary>
	[SerializeField, Header("----- XMG Tween variables -----")]
	private AnimationCurve tweenCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	/// <summary>
	/// How the tween will 
	/// </summary>
	[SerializeField]
	private PlayMode repeatMode = PlayMode.Once;

	/// <summary>
	/// The duration of the tween.
	/// </summary>
	[SerializeField, MinValueAttribute(0f)]
	private float tweenDuration = 1f;

	/// <summary>
	/// Flag indicating that this tween should operate in unscaled time.
	/// </summary>
	[SerializeField]
	private bool useRealTime = false;

	/// <summary>
	/// Optional delay on the start of the tween. The tween will wait this long before beginning each time it loops.
	/// </summary>
	[SerializeField]
	private float startDelay = 0f;

	/// <summary>
	/// The UnityEvent to call when the tween finishes.
	/// </summary>
	[SerializeField]
	private UnityEngine.Events.UnityEvent OnFinished = null;

	/// <summary>
	/// The UnityEvent list to call when the tween loops or when it switches direction during a PingPong.
	/// </summary>
	[SerializeField]
	private UnityEngine.Events.UnityEvent OnLoop = null;

	/// <summary>
	/// Flag that indicates that this tween has been started.
	/// </summary>
	private bool started = false;

	/// <summary>
	/// The start time.
	/// </summary>
	private float startTime = 0f;

	/// <summary>
	/// The current duration, taking into account the direction of travel.
	/// </summary>
	private float currentDuration = 0f;

	/// <summary>
	/// The current tween factor, always between 0 and 1.
	/// </summary>
	private float tweenFactor = 0f;

	private float amountPerDelta = 1f;
	/// <summary>
	/// Amount advanced per delta time, recalculated whenever the duration changes.
	/// </summary>
	public float AmountPerDelta {
		get {
            if (!Mathf.Approximately(this.currentDuration, this.tweenDuration)) {
				this.currentDuration = this.tweenDuration;
				// Save the direction of the delta.
				float sign = Mathf.Sign(this.amountPerDelta);

				// Reset the amount per delta.
				this.amountPerDelta = DEFAULT_AMOUNT_PER_DELTA;
	
				if (this.tweenDuration > 0f) {
					this.amountPerDelta = Mathf.Abs(1f / this.tweenDuration);
				}

				this.amountPerDelta *= sign;
			}

			return this.amountPerDelta;
		}
	}

	#endregion

	#region Monobehaviour

	/// <summary>
	/// If the tween element is disabled we set the started flag to false so when re-enabled it updates the start time correctly and recalculates the start delay.
	/// </summary>
	protected virtual void OnDisable() {
		this.started = false;
	}

	/// <summary>
	/// Ensure that Update is run on the frame it's initialized so that all the variables get set up correctly.
	/// </summary>
	protected virtual void Start() {
		this.Update();
	}

	/// <summary>
	/// During the update we need to move the value forward in the direction of it's 
	/// </summary>
	private void Update() {
		float timeDelta = this.useRealTime ? Time.unscaledDeltaTime : Time.deltaTime;
		float time = this.useRealTime ? Time.unscaledTime : Time.time;

		if (!this.started) {
			this.started = true;
			this.startTime = time + this.startDelay;
		}

		// If the time to start hasn't come yet just return.
		if (time < this.startTime) {
			return;
		}

		// Progress the tweenFactor.
		this.tweenFactor += this.AmountPerDelta * timeDelta;

		// Check for special factors based on the repeat mode.
		bool isFinished = false;

		switch(this.repeatMode) {
		case PlayMode.Loop:
			if (this.tweenFactor > 1f) {
				// Reset the tween factor but keep any fractional remainder so we don't end up out of step with the time mode.
				this.tweenFactor -= Mathf.Floor(this.tweenFactor);
				if (this.OnLoop != null) {
					this.OnLoop.Invoke();
				}
			}
			break;
		case PlayMode.PingPong:
			if (this.tweenFactor > 1f) {
				// Invert the tween factor, keeping any remainder so that our amountperdelta is still valid.
				// NOTE: If the there is a frame delay so long that we skip an entire loop in the tween we can end up at the opposite end of the ping pong leading to unexpected behaviour.
				this.tweenFactor = 1f - (this.tweenFactor - Mathf.Floor(this.tweenFactor));
				// Set the function to run the opposite way.
				this.amountPerDelta *= -1f;

				if (this.OnLoop != null) {
					this.OnLoop.Invoke();
				}
			} else if (this.tweenFactor < 0f) {
				this.tweenFactor *= -1f;
				this.tweenFactor -= Mathf.Floor(this.tweenFactor);

				// Reverse direction.
				this.amountPerDelta *= -1f;

				if (this.OnLoop != null) {
					this.OnLoop.Invoke();
				}
			}
			break;
		case PlayMode.Once:
            if (Mathf.Approximately(this.tweenDuration, 0f) || this.tweenFactor >= 1f || this.tweenFactor <= 0f) {
				// Clamp the tween factor.
				this.tweenFactor = Mathf.Clamp01(this.tweenFactor);

				isFinished = true;
				// Call update and Finished functions before we disable this.
				this.Sample(this.tweenFactor, true);

				// Disable the component when the tween finishes.
				this.enabled = false;

				if (this.OnFinished != null) {
					this.OnFinished.Invoke();
				}
			}
			break;
		}

		// If we haven't finished yet call the Sample function.
		if (!isFinished) {
			this.Sample(this.tweenFactor, false);
		}
	}

	/// <summary>
	/// When added to a component call the set values.
	/// </summary>
	protected virtual void Reset() {
		if (!this.started) {
			this.SetStartToCurrentValue();
			this.SetEndToCurrentValue();
		}
	}

	#endregion

	#region Tween Component Logic

	/// <summary>
	/// Play this tween.
	/// </summary>
	/// <param name="playForward">If set to <c>true</c> play the tween forward, otherwise play it backwards.</param>
	public void Play(bool playForward = true) {
		this.amountPerDelta = Mathf.Abs(this.AmountPerDelta);
		if (!playForward) {
			this.amountPerDelta *= -1f;
		}

		this.enabled = true;
	}

	/// <summary>
	/// Resets the tween to the beginning.
	/// </summary>
	/// <param name="forceEnableTween">If set to <c>True</c> enable the tween component after the reset, otherwise leave the state be.</param>
	public void ResetToBeginning(bool forceEnableTween = false) {
		this.started = false;

		// If we are forcing the tween to be enabled we set it enabled. Otherwise leave it in it's current state.
		if (forceEnableTween) {
			this.enabled = true;
		}
		this.tweenFactor = (this.AmountPerDelta < 0f) ? 1f : 0f;
		this.Sample(this.tweenFactor, false);
	}

	/// <summary>
	/// Handles sampling the tween factor from the animation curve if there is one present and passing the value to the TweenUpdate function.
	/// </summary>
	/// <param name="sampleValue">The tween value to use for sampling.</param>
	/// <param name="isFinished">If set to <c>true</c> is finished.</param>
	private void Sample(float sampleValue, bool isFinished) {
		if (this.tweenCurve != null) {
			sampleValue = this.tweenCurve.Evaluate(sampleValue);
		} 

		this.TweenUpdate(sampleValue, isFinished);
	}

	/// <summary>
	/// Sets up the tween of the specified type on the given gameobject with default params.
	/// </summary>
	/// <param name="go">Game object to attach the tween to.</param>
	/// <param name="duration">Duration of one "cycle" of the tween.</param>
	/// <typeparam name="T">The type of tween to Begin.</typeparam>
	public static T Begin<T>(GameObject go, float duration) where T : XMGTween {
		T tween = go.GetComponent<T>();

		if (tween == null) {
			tween = go.AddComponent<T>();

			if (tween == null) {
				Debug.LogError(string.Format("Couldn't add XMGTween of type {0} to gameobject {1}", typeof(T), go.name), go);
			}
		}

		tween.started = false;
		tween.tweenDuration = duration;

		// Need to do some setup in leu of Start running because this Tween might be setup buy the calling function.
		tween.amountPerDelta = Mathf.Abs(tween.AmountPerDelta);
		tween.enabled = true;

		return tween;
	}

	/// <summary>
	/// Overload to supply actual tweening logic.
	/// </summary>
	/// <param name="sampleValue">The sampling value of this Update.</param>
	/// <param name="isFinished">Flag indicating if the tween is finishing this frame.</param>
	protected abstract void TweenUpdate(float sampleValue, bool isFinished);

	/// <summary>
	/// Set the start value of the tweening variable to the current value.
	/// </summary>
	public virtual void SetStartToCurrentValue() {
		// There is no implementation of the actual variable to tween in the base class so this can do nothing by default.
	}

	/// <summary>
	/// Sets the end to current value.
	/// </summary>
	public virtual void SetEndToCurrentValue() {
		// Simlar to setting the start value since there isn't an implementation for the actual object being tweened (since it could be a float, a position or just a number) so by default this does nothing.
	}

	/// <summary>
	/// Sets the tween directly to the starting value.
	/// </summary>
	public virtual void SetTweenToStart() {
		this.TweenUpdate((this.AmountPerDelta < 0f) ? 1f : 0f, true);
	}

	/// <summary>
	/// Sets the tween directly to the end value.
	/// </summary>
	public virtual void SetTweenToEnd() {
		this.TweenUpdate((this.AmountPerDelta >= 0f) ? 1f : 0f, true);
	}

	#endregion
}
