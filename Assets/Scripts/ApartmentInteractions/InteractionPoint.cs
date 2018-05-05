using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractionButton))]
public class InteractionPoint : MonoBehaviour {

    /// <summary>
    /// The interaction root identifier connects this interaction point to it's list of interaction options in the data controller.
    /// </summary>
    [SerializeField]
    private string interactionRootID = string.Empty;

    public void Start() {
        DataManager.Instance.DoOnLoaded(()=>{
            this.GetComponent<InteractionButton>().Init(DataManager.Instance.rootNode.GetChild(this.interactionRootID));
        });
    }

}
