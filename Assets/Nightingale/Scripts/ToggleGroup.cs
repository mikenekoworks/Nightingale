using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

namespace Nightingale {
	public class ToggleGroup : UIBehaviour {

		private List<IToggleGroupHandler> m_Toggles = new List<IToggleGroupHandler>();

		public void Unregister( IToggleGroupHandler toggle ) {

			if ( m_Toggles.Contains( toggle ) ) {
				m_Toggles.Remove( toggle );
			}

		}

		public void Register( IToggleGroupHandler toggle ) {

			if ( !m_Toggles.Contains( toggle ) ) {
				m_Toggles.Add( toggle );
			}

		}

		private void ValidateToggleIsInGroup( IToggleGroupHandler toggle ) {
			if ( toggle == null || !m_Toggles.Contains( toggle ) )
				throw new ArgumentException( string.Format( "Toggle {0} is not part of ToggleGroup {1}", new object[] { toggle, this } ) );
		}

		public void NotifyToggleOn( IToggleGroupHandler toggle ) {

			ValidateToggleIsInGroup( toggle );

			// disable all toggles in the group
			for ( var i = 0; i < m_Toggles.Count; i++ ) {
				if ( m_Toggles[ i ] == toggle ) {
					continue;
				}

				m_Toggles[ i ].OnGroupNotify( false );
			}
		}

		public bool AnyTogglesOn() {
			return m_Toggles.Find( x => x.GetState() ) != null;
		}

	}
}
