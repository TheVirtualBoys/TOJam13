using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This function handles the apartment scene.
/// It should have a list of interaction points, 
/// </summary>
public class ApartmentSceneController : MonoBehaviour
{

    #region Data

    /// <summary>
    /// List of all the interaction points in the apartment.
    /// </summary>
    [SerializeField]
    private List<InteractionPoint> interactionPoints = null;

    #endregion
}
