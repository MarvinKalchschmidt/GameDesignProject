using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayMessage : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI label;

	private void Awake()
	{
		label.outlineColor = Color.black;
		label.outlineWidth = 0.2f;
	}
}
