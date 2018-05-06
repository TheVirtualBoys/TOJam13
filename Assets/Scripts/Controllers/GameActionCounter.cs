using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionCounter : MonoBehaviour {

    /// <summary>
    /// Happens when the game action limit is reached.
    /// </summary>
    public System.Action OnGameActionLimitReached = null;

    #region Data

    /// <summary>
    /// The list of number counters 
    /// </summary>
    [SerializeField]
    private Sprite[] numbers = null;

    /// <summary>
    /// The number of actions that are avaliable.
    /// </summary>
    [SerializeField]
    private int maxActions = 10;

    /// <summary>
    /// The image for displaying the current action number.
    /// </summary>
    [SerializeField]
    private UnityEngine.UI.Image image = null;

    /// <summary>
    /// The current action count.
    /// </summary>
    private int actionCount = 0;

    #endregion

    #region Monobehaviour

    private void Start() {
        AdventureLog.Instance.OnActionUsed += this.RegisterAction;
    }

    private void OnDestroy() {
        AdventureLog.Instance.OnActionUsed -= this.RegisterAction;
    }

    #endregion

    #region Data

    /// <summary>
    /// Resets the action counter to zero.
    /// </summary>
    public void ResetActionCounter() {
        this.actionCount = 0;
        this.UpdateGraphics();
    }

    /// <summary>
    /// Handles registering an action with the adventure log.
    /// </summary>
    [EditorButton]
    public void RegisterAction() {
        actionCount++;
        if (actionCount < maxActions) {
            this.UpdateGraphics();
        } else {
            if (this.OnGameActionLimitReached != null) {
                this.OnGameActionLimitReached();
            }
        }
    }

    /// <summary>
    /// Updates the action count.
    /// </summary>
    private void UpdateGraphics() {
        this.image.sprite = this.numbers[actionCount];
    }

    #endregion

}
