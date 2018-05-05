using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class DirectorController : MonoBehaviour {

    [SerializeField, Header("----- Director Controller -----")]
    private PlayableDirector director = null;
    /// <summary>
    /// Gets the PlayableDirector that this asset is controlling.
    /// </summary>
    public PlayableDirector Director {
        get {
            return this.director;
        }
    }

    #region Monobehaviour

    /// <summary>
    /// Check required serialized variables.
    /// </summary>
    private void Awake() {
        Debug.Assert(this.Director != null, "No director attached to DirectorController on " + this.name);
    }

    /// <summary>
    /// Automatically attach the PlayableDirector that is on the same game object as this component.
    /// </summary>
    private void Reset() {
        this.director = this.GetComponent<PlayableDirector>();
    }

    #endregion

    #region Director Control Functions

    /// <summary>
    /// Sets the normalized time.
    /// </summary>
    /// <param name="normalizedTime">Normalized time to set the director time to.</param>
    public void SetNormalizedTime(double normalizedTime) {
        normalizedTime = MathUtils.Clamp01(normalizedTime);
        this.SetDirectorTime((float)this.Director.duration * normalizedTime);
    }

    /// <summary>
    /// Sets the director time explicitly.
    /// </summary>
    /// <param name="time">Time in seconds to set the Director time to.</param>
    public void SetDirectorTime(double time) {
        this.Director.time = time;
        this.Director.Evaluate();
    }

    #endregion

    #region Convenience Functions

    /// <summary>
    /// Explicitly set the director time to zero.
    /// </summary>
    public void ResetDirector() {
        this.SetDirectorTime(0.0);
    }

    #endregion

}
