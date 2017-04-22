using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace Nightingale {

	[RequireComponent (typeof (RectTransform))]
	public class AccordionButton : UIBehaviour, IPointerClickHandler {
		public enum ToggleTransition {
			None,
			Slide
		}
		public ToggleTransition toggleTransition = ToggleTransition.Slide;

		// オープン出来るか？
		[SerializeField] private bool m_IsCollapsible;
		public bool isCollapsible {
			get { return m_IsCollapsible; }
			set {
				m_IsCollapsible = value;
			}
		}
		// オープンしているか？
		[SerializeField] private bool m_IsExpand;
		public bool isExpand {
			get { return m_IsExpand; }
			set {
				if ( m_IsExpand == value ) {
					return;
				}

				m_IsExpand = value;

				PlayEffect();
			}
		}

		public bool isClickEventHeaderOnly;
		public Sprite CollapseSprite;
		public Sprite ExpandSprite;
		public Image HeaderImage;

		public RectTransform ContentsMask;
		public RectTransform ContentsRoot;

		public Vector2 CollapseSize;
		public Vector2 ExpandSize;

		[ Serializable]
		public class StateChangedEvent : UnityEvent<bool> { }

		public StateChangedEvent onStateChanged = new StateChangedEvent();

		protected override void Start() {
			PlayEffect();
		}

		// コンテンツを表示する
		public void Expand() {

			// 開いたり、閉じたり出来ない状態なので処理しない
			if ( m_IsCollapsible == false ) {
				return;
			}

			isExpand = true;
		}

		// コンテンツを非表示にする。
		public void Collapse() {

			// 開いたり、閉じたり出来ない状態なので処理しない
			if ( m_IsCollapsible == false ) {
				return;
			}

			isExpand = false;
		}

		private void InternalToggleState() {

			// 開いたり、閉じたり出来ない状態なので処理しない
			if ( m_IsCollapsible == false ) {
				return;
			}

			isExpand = !isExpand;

		}

		private void PlayEffect() {

			// 開閉ボタングラフィックの書き換え
			if ( ExpandSprite != null ) {

				if ( isExpand == true ) {
					HeaderImage.sprite = ExpandSprite;
					//ExpandGraphic.gameObject.SetActive( true );
					//CollapseGraphic.gameObject.SetActive( false );
				} else {
					HeaderImage.sprite = CollapseSprite;
					//ExpandGraphic.gameObject.SetActive( false );
					//CollapseGraphic.gameObject.SetActive( true );
				}

			}

			switch ( toggleTransition ) {
			case ToggleTransition.None:
			default:
				Vector3 v = ContentsRoot.transform.localPosition;
				if ( isExpand == true ) {
					v.y = 0.0f;
				} else {
					v.y = ContentsRoot.rect.height;
				}
				ContentsRoot.transform.localPosition = v;

				break;
			}

		}

		public void OnPointerClick( PointerEventData eventData ) {
			if ( eventData.button != PointerEventData.InputButton.Left ) {
				return;
			}

			// クリックに反応するのはヘッダーのみか？
			if ( isClickEventHeaderOnly == true ) {
				if ( eventData.pointerCurrentRaycast.gameObject == HeaderImage.gameObject ) {
					InternalToggleState();
				}
			} else {
				InternalToggleState();
			}

		}

		public void CalculateLayout() {

			RectTransform rt = gameObject.GetComponent<RectTransform>();
			rt.sizeDelta = CollapseSize;

			RectTransform image_rt = HeaderImage.gameObject.GetComponent<RectTransform>();
			image_rt.sizeDelta = CollapseSize;

			ContentsRoot.sizeDelta = ExpandSize;
			ContentsMask.sizeDelta = ExpandSize;

		}

	}

}
