using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InteractionPoint : MonoBehaviour {

    #region Events

    /// <summary>
    /// The interaction point being selected will call an event to move Dan around to that position.
    /// </summary>
    public System.Action<InteractionPoint> OnInteractionPointSelected = null;

    #endregion

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

    /// <summary>
    /// The interaction button.
    /// </summary>
    [SerializeField]
    private Button interactionButton = null;

    [SerializeField, Header("----- Interaction Data ------")]
    private string interactionAreaIdleAnimation = "FrontIdle";
    /// <summary>
    /// Idle animation that getting to this interaction point should start playing.
    /// </summary>
    public string InteractionAreaIdleAnimation {
        get {
            return this.interactionAreaIdleAnimation;
        }
    }

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
            Debug.Log("Data loaded!");
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
        Debug.Log("Button");
        NodeData node = DataManager.Instance.rootNode.GetChild(this.interactionRootNodeData.id);
        InteractionMenu menu = Object.Instantiate<InteractionMenu>(this.interactionMenuPrefab);
        menu.Init(AdventureLog.Instance.FilterAvailableNodes(node.children));

        if (this.OnInteractionPointSelected != null) {
            this.OnInteractionPointSelected(this);
        }
    }

    #endregion

}
