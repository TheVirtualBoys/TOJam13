using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZLayerByY : MonoBehaviour {

	// Update is called once per frame
	void LateUpdate () {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y); 
	}
}
