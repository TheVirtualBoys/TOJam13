using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour {

    public InteractionMenu menuPrefab;

    private NodeData node;

    /// <summary>
    /// The button that'll handle the interaction with this interaction point.
    /// </summary>
    [SerializeField]
    private Button button = null;

    public Text buttonText = null;

    private InteractionMenu parentMenu;

    public void Init(NodeData node, InteractionMenu parentMenu) {
        this.node = node;
        this.parentMenu = parentMenu;
        if (this.buttonText != null) {
            this.buttonText.text = node.title;
        }
    }

    #region Monobehaviour

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start() {
        this.button.onClick.AddListener(this.HandleButtonPressed);
    }

    /// <summary>
    /// Ons the destroy.
    /// </summary>
    private void OnDestroy() {
        this.button.onClick.RemoveListener(this.HandleButtonPressed);
    }

    #endregion

    /// <summary>
    /// Handles the button being pressed.
    /// </summary>
    private void HandleButtonPressed() {
        if (this.node is InteractionNodeData) {
            // leaf node, do the thing
            InteractionNodeData interaction = (InteractionNodeData)this.node;
            Debug.Log(interaction.description);

            AdventureLog.Instance.UseAction(interaction);

        } else {
            List<NodeData> avaliableNodes = AdventureLog.Instance.FilterAvailableNodes(this.node.children);

            if (avaliableNodes.Count > 0) {
                // submenu time
                InteractionMenu menu = GameObject.Instantiate(this.menuPrefab);
                menu.transform.SetParent(this.parentMenu != null ? this.parentMenu.transform.parent : this.transform);
                menu.Init(avaliableNodes);
            }
        }

        if (this.parentMenu != null) {
            this.parentMenu.Close();
        }
    }

}