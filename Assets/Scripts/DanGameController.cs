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

    #endregion

    private void Start() {
        this.danGameTitleController.gameObject.SetActive(true);

        this.danGameTitleController.StartNewGameSelected += this.HandleTitleSequenceComplete;
    }

    #region Dan Game Control

    /// <summary>
    /// Handles the 
    /// </summary>
    private void HandleTitleSequenceComplete() {
        this.apartmentController.ResetApartment();
        this.danGameTitleController.gameObject.SetActive(false);

        // TODO: Whatever kicks off the game loops.
    }

    #endregion
}
