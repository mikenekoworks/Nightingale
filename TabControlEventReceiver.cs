using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using System.Collections;

namespace Nightingale {
	public class TabControlEventReceiver : Selectable, IPointerClickHandler {

	//	public TabControlGroup Controller;

		[System.Serializable]
		public class EventClick : UnityEvent< Selectable > {}

		// Event delegates triggered on click.
		[FormerlySerializedAs("onClick")]
		[SerializeField]
		private EventClick m_OnClick = new EventClick();
		public EventClick onClick
		{
			get { return m_OnClick; }
			set { m_OnClick = value; }
		}

		private void Press()
		{
			if (!IsActive() || !IsInteractable() ) {
				return;
			}

			m_OnClick.Invoke( this );
		}

		// Trigger all registered callbacks.
		public virtual void OnPointerClick(PointerEventData eventData)
		{

			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			Press();
		}

	}

}