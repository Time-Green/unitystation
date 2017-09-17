﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI{
	/// <summary>
	/// Custom ResizePanel for the PANEL_Right UI element
	/// </summary>
	public class RightPanelResize : ResizePanel {

		public RectTransform hudRight;
		public RectTransform hudBottom;
		float hudRight_dist;
		float leftRange;
		float rightRange;
		float cacheWidth;

		void Start(){
			cacheWidth = panelRectTransform.sizeDelta.x;
			leftRange = maxSize.x - cacheWidth;
			rightRange = cacheWidth - minSize.x;
		}
		public override void OnPointerDown(PointerEventData data){
			hudRight_dist = transform.position.x - hudRight.position.x;
			base.OnPointerDown(data);
		}
		//TODO handle max size on x and hide panel when below min x (showing the transparent chatbox)
		public override void OnDrag(PointerEventData data){
			if (panelRectTransform == null)
				return;

			Vector2 localPointerPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out localPointerPosition);
			Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;

			Vector2 sizeDelta = originalSizeDelta + new Vector2(-offsetToOriginal.x, 0f);

			if (sizeDelta.x < maxSize.x) {
				panelRectTransform.sizeDelta = sizeDelta;
			} else {
				sizeDelta.x = maxSize.x;
				panelRectTransform.sizeDelta = sizeDelta;
			}

			AdjustHudRight();
			AdjustHudBottom();
		}

		void AdjustHudRight(){
			Vector3 newHudRight_Pos = hudRight.position;
			newHudRight_Pos.x = transform.position.x - hudRight_dist;
			hudRight.position = newHudRight_Pos;
		}

		void AdjustHudBottom(){
			if (panelRectTransform.sizeDelta.x > cacheWidth) {
				Vector3 newScale = hudBottom.localScale;
				newScale.x = (((maxSize.x - panelRectTransform.sizeDelta.x) / leftRange)
				* 0.4f) + 0.6f;
				newScale.y = newScale.x;
				hudBottom.localScale = newScale;

                Vector3 newPos = hudBottom.anchoredPosition;
                newPos.x = (((maxSize.x - panelRectTransform.sizeDelta.x) / leftRange)
                * 51f) + 2f;
                hudBottom.anchoredPosition = newPos;
			} else {
				Vector3 newScale = hudBottom.localScale;
				newScale.x = 1.2f - (((panelRectTransform.sizeDelta.x - minSize.x) / rightRange)
					* 0.2f);
				newScale.y = newScale.x;
				hudBottom.localScale = newScale;

                Vector3 newPos = hudBottom.anchoredPosition;
                newPos.x = (((rightRange - (panelRectTransform.sizeDelta.x - minSize.x)) / rightRange)
                    * 128f) + 51f;
                hudBottom.anchoredPosition = newPos;
			}
			UIManager.DisplayManager.SetCameraFollowPos();
		}
}
}