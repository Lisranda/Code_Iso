using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_OnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public UnityEvent onMouseOver;
	public UnityEvent onMouseExit;

	public void OnPointerEnter (PointerEventData eventData) {
		onMouseOver.Invoke ();
	}

	public void OnPointerExit (PointerEventData eventData) {
		onMouseExit.Invoke ();
	}

}
