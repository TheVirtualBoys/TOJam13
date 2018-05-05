using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InteractionMenu : MonoBehaviour {

    public InteractionButton buttonPrefab;

    public Transform buttonContainer;

    public void Init(List<NodeData> nodes) {
        Debug.Log(nodes.Count);
        foreach (NodeData node in nodes) {
            Debug.Log(node);
            InteractionButton button = GameObject.Instantiate(this.buttonPrefab);
            button.transform.SetParent(this.buttonContainer);
        }
    }

}
