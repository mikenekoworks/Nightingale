using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Nightingale {
	public class ToggleImage : UIBehaviour, IPointerClickHandler, IToggleGroupHandler {
		public enum ToggleTransition {
			None,
			Fade
		}
		public ToggleTransition toggleTransition = ToggleTransition.Fade;

		public Graphic onGraphic;
		public Graphic offGraphic;

		[Serializable]
		public class ToggleEvent : UnityEvent<bool> { }

		public ToggleEvent onValueChanged = new ToggleEvent();

		[SerializeField]
		private ToggleGroup m_Group;

		public ToggleGroup group {
			get { return m_Group; }
			set {
				m_Group = value;
	//#if UNITY_EDITOR
	//			if ( Application.isPlaying )
	//#endif
	//				{
					SetToggleGroup( m_Group, true );
					PlayEffect( true );
	//			}
			}
		}

		private void SetToggleGroup( ToggleGroup newGroup, bool setMemberValue ) {
			ToggleGroup oldGroup = m_Group;

			// Sometimes IsActive returns false in OnDisable so don't check for it.
			// Rather remove the toggle too often than too little.
			if ( m_Group != null )
				m_Group.Unregister( this );

			// At runtime the group variable should be set but not when calling this method from OnEnable or OnDisable.
			// That's why we use the setMemberValue parameter.
			if ( setMemberValue )
				m_Group = newGroup;

			// Only register to the new group if this Toggle is active.
			if ( m_Group != null && IsActive() )
				m_Group.Register( this );

			// If we are in a new group, and this toggle is on, notify group.
			// Note: Don't refer to m_Group here as it's not guaranteed to have been set.
			if ( newGroup != null && newGroup != oldGroup && isOn && IsActive() )
				m_Group.NotifyToggleOn( this );
		}

		[SerializeField]
		private bool m_IsOn;
		public bool isOn {
			get { return m_IsOn; }
			set {
				Set( value );
			}
		}
		void Set( bool value ) {
			Set( value, true );
		}

		void Set( bool value, bool sendCallback ) {
			if ( m_IsOn == value ) {
				return;
			}

			// if we are in a group and set to true, do group logic
			m_IsOn = value;
			if ( m_Group != null && IsActive() ) {
			//	if ( m_IsOn || ( !m_Group.AnyTogglesOn() && !m_Group.allowSwitchOff ) ) {
			//		m_IsOn = true;
				if ( m_IsOn == true ) {
					m_Group.NotifyToggleOn( this );
				}

			}

			// Always send event when toggle is clicked, even if value didn't change
			// due to already active toggle in a toggle group being clicked.
			// Controls like Dropdown rely on this.
			// It's up to the user to ignore a selection being set to the same value it already was, if desired.
			PlayEffect( toggleTransition == ToggleTransition.None );
			if ( sendCallback ) {
				onValueChanged.Invoke( m_IsOn );
			}
		}

		private void PlayEffect( bool instant ) {

			if ( ( onGraphic == null ) || ( offGraphic == null ) ) {
				return;
			}

			//		if ( graphic == null )
			//			return;

			//#if UNITY_EDITOR
			//		if ( !Application.isPlaying )
			//			graphic.canvasRenderer.SetAlpha( m_IsOn ? 1f : 0f );
			//		else
			//#endif
			//			graphic.CrossFadeAlpha( m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true );
			onGraphic.CrossFadeAlpha( m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true );
			offGraphic.CrossFadeAlpha( m_IsOn ? 0f : 1f, instant ? 0f : 0.1f, true );

		}

		/// <summary>
		/// Assume the correct visual state.
		/// </summary>
		protected override void Start() {
			PlayEffect( true );
		}

		private void InternalToggle() {
			//if ( !IsActive() || !IsInteractable() ) {
			if ( !IsActive() ) {
				return;
			}

			isOn = !isOn;
		}

		protected ToggleImage() { }

		public void OnPointerClick( PointerEventData eventData ) {

			if ( eventData.button != PointerEventData.InputButton.Left ) {
				return;
			}

			InternalToggle();

		}

		public void OnGroupNotify( bool value ) {

			Set( value );

			//throw new NotImplementedException();
		}

		protected override void OnEnable() {
			base.OnEnable();
			SetToggleGroup( m_Group, false );
			PlayEffect( true );
		}

		protected override void OnDisable() {
			SetToggleGroup( null, false );
			base.OnDisable();
		}

		public bool GetState() {
			return m_IsOn;
		}
	}

}
