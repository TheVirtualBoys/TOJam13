using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InteractionPoint : MonoBehaviour {

    #region Data

    /// <summary>
    /// The interaction root identifier connects this interaction point to it's list of interaction options in the data controller.
    /// </summary>
    [SerializeField]
    private string interactionRootID = string.Empty;

    /// <summary>
    /// The button that'll handle the interaction with this interaction point.
    /// </summary>
    [SerializeField]
    private Button interactionPointButton = null;

    #endregion

    #region Monobehaviour

    /// <summary>
    /// Get the buttons.
    /// </summary>
    private void Reset() {
        this.interactionPointButton = this.GetComponent<Button>();
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start() {
        this.interactionPointButton.onClick.AddListener(this.HandleButtonPressed);
    }

    /// <summary>
    /// Ons the destroy.
    /// </summary>
    private void OnDestroy() {
        this.interactionPointButton.onClick.RemoveListener(this.HandleButtonPressed);
    }

    #endregion

    #region UI Handling

    /// <summary>
    /// Handles the button being pressed.
    /// </summary>
    private void HandleButtonPressed() {
        Debug.LogFormat("Interaction Point {0} for interaction root {1} pressed.", this.name, this.interactionRootID);
    }

    #endregion
}
