using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanGameController : MonoBehaviour {

    #region Data

    /// <summary>
    /// This controls the title screen and intro overlays.
    /// </summary>
    [SerializeField]
    private TitleController danGameTitleController = null;

    /// <summary>
    /// The apartment controller controls dan's world view.
    /// </summary>
    [SerializeField]
    private ApartmentSceneController apartmentController = null;

    /// <summary>
    /// The game action counter.
    /// </summary>
    [SerializeField]
    private GameActionCounter actionCounter = null;

    #endregion

    /// <summary>
    /// Starts the game.
    /// </summary>
    private void Start() {
        this.danGameTitleController.gameObject.SetActive(true);
        this.danGameTitleController.StartNewGameSelected += this.HandleTitleSequenceComplete;
        this.actionCounter.OnGameActionLimitReached += this.HandleActionLimitReleased;
    }

    #region Dan Game Control

    /// <summary>
    /// Handles the 
    /// </summary>
    [EditorButton]
    private void HandleTitleSequenceComplete() {
        this.apartmentController.ResetApartment();
        this.danGameTitleController.gameObject.SetActive(false);
        this.actionCounter.ResetActionCounter();


        // TODO: Whatever kicks off the game loops.
    }

    private void HandleActionLimitReleased() {
        this.danGameTitleController.gameObject.SetActive(true);
        this.danGameTitleController.GameOver();
    }

    #endregion
}
