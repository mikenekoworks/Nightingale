using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;
using UnityEditor.Events;
using UnityEditorInternal;
using System.Collections;

namespace Nightingale {
	[CustomEditor( typeof( Nightingale.TabControlGroup ), true )]
	[CanEditMultipleObjects]
	public class TabControlGroupEditor : Editor {

		bool m_IsShowItems = true;

		SerializedProperty m_ContentsRootProperty;		// 選択状態

		SerializedProperty m_SelectedSpriteProperty;		// 選択状態
		SerializedProperty m_DeselectedSpriteProperty;		// 非選択状態
		SerializedProperty m_DisableSpriteProperty;		// 選択できない状態
		SerializedProperty m_TabControlsProperty;		//
		SerializedProperty m_TabSizeProperty;		//
		SerializedProperty m_TabLayoutProperty;		//

		SerializedProperty m_OnTabIndexChangedProperty;

		ReorderableList m_TabControlsList;

		protected void OnEnable() {

			m_ContentsRootProperty = serializedObject.FindProperty( "ContentsRoot" );

			m_SelectedSpriteProperty = serializedObject.FindProperty( "SelectedSprite" );
			m_DeselectedSpriteProperty = serializedObject.FindProperty( "DeselectedSprite" );
			m_DisableSpriteProperty = serializedObject.FindProperty( "DisableSprite" );

			m_TabSizeProperty = serializedObject.FindProperty( "TabSize" );
			m_TabControlsProperty = serializedObject.FindProperty( "TabControls" );

	//		m_TabLayoutProperty = serializedObject.FindProperty( "tabLayout" );

			m_TabControlsList = new ReorderableList(serializedObject, m_TabControlsProperty, true, false, true, true );
			m_TabControlsList.headerHeight = 2.0f;
			m_TabControlsList.drawElementCallback = ( rect, index, is_active, is_focused) => {
				TabControlGroup tab_script = (TabControlGroup)serializedObject.targetObject;
				var obj = tab_script.TabControls[ index ];
				EditorGUI.LabelField( rect, obj.name );
				//EditorGUI.ObjectField( rect, obj, typeof( Selectable ) );
			};
			m_TabControlsList.onAddCallback = ( ReorderableList list ) => {
				TabControlGroup tab_script = (TabControlGroup)serializedObject.targetObject;

				CreateNewTab( tab_script, tab_script.ContentsRoot );
			};
			m_TabControlsList.onSelectCallback = ( ReorderableList list ) => {
				TabControlGroup tab_script = (TabControlGroup)serializedObject.targetObject;
				EditorGUIUtility.PingObject( tab_script.TabControls[ list.index ].gameObject );
			};
			m_TabControlsList.onRemoveCallback = ( ReorderableList list ) => {
				TabControlGroup tab_script = (TabControlGroup)serializedObject.targetObject;
				GameObject.DestroyImmediate( tab_script.TabControls[ list.index ].gameObject );

				tab_script.TabControls.RemoveAt( list.index );
			};
	//		m_TabControlsList.onChangedCallback = ( ReorderableList list ) => {
	//			TabControlGroup tab_script = (TabControlGroup)serializedObject.targetObject;
	//			Debug.Log( "Channged List" );
	//		};


			m_OnTabIndexChangedProperty = serializedObject.FindProperty( "onTabIndexChanged" );
		}

		public override void OnInspectorGUI() {

			serializedObject.Update();

			EditorGUILayout.PropertyField( m_ContentsRootProperty );
			EditorGUILayout.PropertyField( m_TabSizeProperty );

			TabControlGroup tab_script = (TabControlGroup)serializedObject.targetObject;
			var op = (TabControlGroup.LayoutAlignment)EditorGUILayout.EnumPopup( "Tab Layout", tab_script.TabLayout );
	//		EditorGUILayout.PropertyField( m_TabLayoutProperty );
			if ( op != tab_script.TabLayout) {
				tab_script.TabLayout = op;
			}

			m_IsShowItems = EditorGUILayout.Foldout( m_IsShowItems, "Tab Collections" );
			if ( m_IsShowItems ) {
				m_TabControlsList.DoLayoutList();
			}

			EditorGUILayout.PropertyField( m_SelectedSpriteProperty );
			EditorGUILayout.PropertyField( m_DeselectedSpriteProperty );
			EditorGUILayout.PropertyField( m_DisableSpriteProperty );

			EditorGUILayout.PropertyField( m_OnTabIndexChangedProperty );

			serializedObject.ApplyModifiedProperties();

		}

		public static Selectable CreateNewTab( TabControlGroup tab_script, RectTransform content_root ) {

			// 簡単にひとつだけボタンを追加する
			DefaultControls.Resources res = new DefaultControls.Resources();
			res.standard = null;//AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
			var tab_button_obj = DefaultControls.CreateButton( res );

			tab_button_obj.name = GameObjectUtility.GetUniqueNameForSibling( content_root, "Button" );

			tab_button_obj.transform.SetParent( content_root );
			tab_button_obj.transform.localPosition = Vector3.zero;
			tab_button_obj.transform.SetAsLastSibling();
			var tab_button_rect_trans = tab_button_obj.GetComponent< RectTransform >();
			tab_button_rect_trans.sizeDelta = tab_script.TabSize;

			// ボタンのスクリプトはいらないので剥がす
			var tab_button_script = tab_button_obj.GetComponent< Button >();
			GameObject.DestroyImmediate( tab_button_script );

	//		var layout_element = tab_button_obj.AddComponent< LayoutElement >();
	//		layout_element.minWidth = tab_script.TabSize.x;
	//		layout_element.minHeight = tab_script.TabSize.y;

			var tab_button_event_receiver = tab_button_obj.AddComponent< TabControlEventReceiver >();
			tab_button_event_receiver.transition = Selectable.Transition.None;

	//		tab_button_event_receiver.Controller = tab_script;
	//		tab_button_event_receiver.onClick.AddListener( tab_script.OnClickTab );

			UnityEventTools.AddPersistentListener( tab_button_event_receiver.onClick, tab_script.OnClickTab );

			tab_script.AddTab( tab_button_event_receiver );

			return tab_button_event_receiver;

		}




	}

}	// end of namespace 

