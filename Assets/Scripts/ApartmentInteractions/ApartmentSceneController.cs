using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This function handles the apartment scene.
/// It should have a list of interaction points, and manage the gameplay world of dan.
/// </summary>
public class ApartmentSceneController : MonoBehaviour {

    #region Data

    /// <summary>
    /// List of all the interaction points in the apartment.
    /// </summary>
    [SerializeField]
    private List<InteractionPoint> interactionPoints = null;

    #endregion

    #region Apartment Control

    /// <summary>
    /// Resets the apartment scene so that we can play a new dan session.
    /// </summary>
    public void ResetApartment() {
        
    }

    #endregion
}
