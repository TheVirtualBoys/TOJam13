using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[UnityEngine.RequireComponent(typeof(Text))]
public class AdventureLogView : MonoBehaviour
{
	private Text text = null;

	private void Awake()
	{
		this.text = this.GetComponent<Text>();
		this.text.text = string.Empty;
		AdventureLog.Instance.logView = this;
	}

	public void AddLogLine(string text)
	{
		this.text.text = this.text.text + "\n" + text;
	}
}
