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
    [SerializeField, Header("----- Interaction -----")]
    private string interactionRootID = string.Empty;

    /// <summary>
    /// The interaction menu prefab.
    /// </summary>
    [SerializeField]
    private InteractionMenu interactionMenuPrefab = null;

    [SerializeField]
    private Button interactionButton = null;

    /// <summary>
    /// The NodeData root of the node.
    /// </summary>
    private NodeData interactionRootNodeData = null;

    [SerializeField, Header("----- Dan Interaction -----")]
    private Transform danLocation = null;
    /// <summary>
    /// The transform that this InteractionPoint should move the Dan to.
    /// </summary>
    public Transform DanLocation {
        get {
            return this.danLocation;
        }
    }

    #endregion

    #region Monobehaviour

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start() {
        DataManager.Instance.DoOnLoaded(() => {
            this.interactionRootNodeData = DataManager.Instance.allNodes.Find((NodeData data) => {
                return data.id == this.interactionRootID;
            });

            this.interactionButton.onClick.AddListener(this.HandleButtonPressed);
        });
    }

    /// <summary>
    /// Ons the destroy.
    /// </summary>
    private void OnDestroy() {
        this.interactionButton.onClick.RemoveListener(this.HandleButtonPressed);
    }

    /// <summary>
    /// Handles the button being pressed.
    /// </summary>
    private void HandleButtonPressed() {
        NodeData node = DataManager.Instance.rootNode.GetChild(this.interactionRootNodeData.id);
        InteractionMenu menu = Object.Instantiate<InteractionMenu>(this.interactionMenuPrefab);
        menu.Init(AdventureLog.Instance.FilterAvailableNodes(node.children));
    }

    #endregion

}
