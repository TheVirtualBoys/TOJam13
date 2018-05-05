using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InteractionButton : MonoBehaviour {

    public InteractionMenu menuPrefab;

    private NodeData node;

    /// <summary>
    /// The button that'll handle the interaction with this interaction point.
    /// </summary>
    [SerializeField]
    private Button interactionPointButton = null;

    public void Init(NodeData node) {
        this.node = node;
    }

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

    /// <summary>
    /// Handles the button being pressed.
    /// </summary>
    private void HandleButtonPressed() {
        NodeData node = DataManager.Instance.rootNode.GetChild(this.node.id);
        InteractionMenu menu = GameObject.Instantiate<InteractionMenu>(this.menuPrefab);
        menu.Init(AdventureLog.Instance.FilterAvailableNodes(node.children));
    }

}