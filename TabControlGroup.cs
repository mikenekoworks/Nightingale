using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Nightingale {
	[RequireComponent (typeof (RectTransform))]
	public class TabControlGroup : UIBehaviour {

		public enum LayoutAlignment {
			Left,
			Center,
			Right
		}

		LayoutAlignment tabLayout;
		public LayoutAlignment TabLayout {
			get {
				return tabLayout;
			}
			set {
				tabLayout = value;
				RebuildTabLayout();
			}
		}

		public RectTransform ContentsRoot;
		public List< Selectable > TabControls = new List< Selectable >();
		[SerializeField] private int selectedTabIndex;
		public int SelectedTabIndex {
			get {
				return selectedTabIndex;
			}
			set {
				
				if ( TabControls.Count < value ) {
					return;
				}

				if ( selectedTabIndex != value ) {
					selectedTabIndex = value;
					SetTabIndex( selectedTabIndex, true );
				}

			}
		}

		public int TabCount {
			get {
				return TabControls.Count;
			}
		}

		public Sprite SelectedSprite;		// 選択状態
		public Sprite DeselectedSprite;		// 非選択状態
		public Sprite DisableSprite;		// 選択できない状態

		public Vector2 TabSize = new Vector2( 80.0f, 30.0f );

		[System.Serializable]
		public class EventTabIndexChanged : UnityEvent<int> { }

		public class EventTabClicked : UnityEvent< Selectable > { }

		public EventTabIndexChanged onTabIndexChanged = new EventTabIndexChanged();

		public TabControlGroup() {
			//TabControls = new List< Selectable >();
		}

		private void SetTabIndex( int index, bool send_callback ) {

			for ( int i = 0; i < TabControls.Count; ++i ) {
				if ( TabControls[ i ].interactable == false ) {
					TabControls[ i ].image.sprite = DisableSprite;

					TabControls[ i ].gameObject.transform.SetAsFirstSibling();

					continue;
				}

				if ( i == index ) {
					TabControls[ i ].image.sprite = SelectedSprite;

					TabControls[ i ].gameObject.transform.SetAsLastSibling();
					continue;
				}

				TabControls[ i ].gameObject.transform.SetAsFirstSibling();
				TabControls[ i ].image.sprite = DeselectedSprite;
			}

			if ( send_callback == true ) {
				onTabIndexChanged.Invoke( selectedTabIndex );
			}

		}

		private void SetSprite( int index ) {
			for ( int i = 0; i < TabControls.Count; ++i ) {
				if ( TabControls[ i ].interactable == false ) {
					TabControls[ i ].image.sprite = DisableSprite;
					continue;
				}

				if ( i == index ) {
					TabControls[ i ].image.sprite = SelectedSprite;
					continue;
				}
				TabControls[ i ].image.sprite = DeselectedSprite;
			}
		}

		public void OnClickTab( Selectable sender ) {

			for ( int i = 0; i < TabControls.Count; ++i ) {

				if ( TabControls[ i ] == sender ) {

					SelectedTabIndex = i;

					break;
				}

			}

		}

		protected override void OnEnable() {
			SetTabIndex( selectedTabIndex, false );
		}

		protected override void OnValidate() {
			selectedTabIndex = Mathf.Clamp( selectedTabIndex, 0, TabControls.Count - 1 );

			SetTabIndex( selectedTabIndex, false );
		}

		public void SetEnabled( int tab_index, bool flag ) {

			if ( tab_index >= TabControls.Count ) {
				return;
			}

			TabControls[ tab_index ].image.sprite = ( flag == true ) ? DeselectedSprite : DisableSprite;
			TabControls[ tab_index ].interactable = flag;

		}

		public void AddTab( Selectable new_tab ) {

			TabControls.Add( new_tab );

			SetSprite( selectedTabIndex );
			RebuildTabLayout();
		}

		private void RebuildTabLayout() {
			RebuildTabLayout( TabLayout );
		}

		/*!
		 * 
		 * @note
		 * 子要素のpivotなどに対応は今後する予定。
		 * 
		 */
		private void RebuildTabLayout( LayoutAlignment tab_align ) {

			Vector2 v = new Vector2( 0.0f, 0.0f );

			Vector2 contents_root_size = ContentsRoot.sizeDelta;

			float tab_width = TabSize.x * TabCount;
			switch ( tab_align ) {
			case LayoutAlignment.Left:
				v.x = -( contents_root_size.x / 2.0f ) + ( TabSize.x / 2.0f );
				break;
			case LayoutAlignment.Center:
				v.x = -( tab_width / 2.0f ) + ( TabSize.x / 2.0f );
				break;
			case LayoutAlignment.Right:
				v.x = ( contents_root_size.x / 2.0f ) - tab_width + ( TabSize.x / 2.0f );;
				break;
			}

			foreach ( var tab in TabControls ) {
				tab.transform.localPosition = v;

				v.x += TabSize.x;
			}


		}

	}
}
