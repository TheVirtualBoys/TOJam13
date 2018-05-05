using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This function handles the apartment scene.
/// It should have a list of interaction points, and manage the gameplay world of dan.
/// </summary>
public class ApartmentSceneController : MonoBehaviour {

    #region Constants

    private const string DEFAULT_SKIN = "Naked";

    /// <summary>
    /// The position to default dan to.
    /// </summary>
    private static readonly Vector3 DEFAULT_DAN_POSITION = new Vector3(127, -39, 0);

    #endregion

    #region Data

    /// <summary>
    /// The apartment 
    /// </summary>
    [SerializeField]
    private DanController apartmentDan = null;

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
        this.apartmentDan.SetDanClothes(DEFAULT_SKIN);
        this.apartmentDan.transform.position = DEFAULT_DAN_POSITION;
    }

    #endregion
}
