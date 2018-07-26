using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_OnClick : MonoBehaviour, IPointerClickHandler {
	public UnityEvent onLeft;
	public UnityEvent onRight;
	public UnityEvent onMiddle;

	public void OnPointerClick (PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left)
			onLeft.Invoke ();
		if (eventData.button == PointerEventData.InputButton.Middle)
			onMiddle.Invoke ();
		if (eventData.button == PointerEventData.InputButton.Right)
			onRight.Invoke ();
	}
}
