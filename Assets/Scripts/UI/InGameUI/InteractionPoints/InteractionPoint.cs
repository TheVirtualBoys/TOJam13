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
    /// The interaction button.
    /// </summary>
    [SerializeField]
    private Button interactionButton = null;

    /// <summary>
    /// This is the actual display that shows dan options.
    /// </summary>
    [SerializeField]
    private GameObject interactionTitleBar = null;

    /// <summary>
    /// The interaction menu selector.
    /// </summary>
    [SerializeField]
    private PointMenu interactionMenuSelector = null;

    [SerializeField]
    private Text interactionName = null;

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

    private bool interactionPointOpen = false;

    #endregion

    #region Monobehaviour

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start() {
        DataManager.Instance.DoOnLoaded(() => {
            this.interactionRootNodeData = DataManager.Instance.rootNode.GetChild(this.interactionRootID);
            if (this.interactionRootNodeData == null) {
                Debug.LogError("The data for node ID " + this.interactionRootID + " is null");
            }

            this.interactionName.text = this.interactionRootNodeData.title;
        });

        this.interactionButton.onClick.AddListener(this.HandleButtonPressed);
    }

    /// <summary>
    /// Ons the destroy.
    /// </summary>
    private void OnDestroy() {
        this.interactionButton.onClick.RemoveListener(this.HandleButtonPressed);
    }

    public void SetTitleDisplayState(bool displayState) {
        this.interactionTitleBar.gameObject.SetActive(displayState);
    }

    public void CloseInteractionPoint() {
        this.SetTitleDisplayState(false);
        this.interactionMenuSelector.ClearMenu();
    }

    public void OpenInteractionPoint() {
        this.SetTitleDisplayState(true);

        // Get populate the menu of this display with the menu items for the display.
        List<NodeData> rootNodeDataList = AdventureLog.Instance.FilterAvailableNodes(this.interactionRootNodeData.children);
        this.interactionMenuSelector.SetRootNode(this.interactionRootNodeData);

        this.transform.SetAsLastSibling();
    }

    public void CheckVisibleState() {
        List<NodeData> rootNodeDataList = AdventureLog.Instance.FilterAvailableNodes(this.interactionRootNodeData.children);

        this.gameObject.SetActive(rootNodeDataList.Count > 0);
    }

    /// <summary>
    /// Handles the button being pressed.
    /// </summary>
    private void HandleButtonPressed() {
        if (this.OnInteractionPointSelected != null) {
            this.OnInteractionPointSelected(this);
        }
    }

    #endregion

}
