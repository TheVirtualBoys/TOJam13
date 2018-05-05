using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class StaticData : MonoBehaviour
{
	public static StaticData Instance {
		get;
		private set;
	}

	private StaticData()
	{
	}
}
