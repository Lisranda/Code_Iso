using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UI_OnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public UnityEvent onBeginDrag;
	public UnityEvent onDrag;
	public UnityEvent onEndDrag;

	public void OnBeginDrag (PointerEventData eventData) {
		onBeginDrag.Invoke ();
	}

	public void OnDrag (PointerEventData eventData) {
		onDrag.Invoke ();
	}

	public void OnEndDrag (PointerEventData eventData) {
		onEndDrag.Invoke ();
	}
}
