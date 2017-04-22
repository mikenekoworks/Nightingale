using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.UI;

namespace Nightingale {

	[CustomEditor( typeof( AccordionButton ), true )]
	[CanEditMultipleObjects]
	public class AccordionButtonEditor : Editor {

		SerializedProperty m_OnStateChangedProperty;
		SerializedProperty m_TransitionProperty;
//		SerializedProperty m_IsExpandProperty;
		SerializedProperty m_IsCollapsibleProperty;
		SerializedProperty m_CollapseSpriteProperty;
		SerializedProperty m_ExpandSpriteProperty;
		SerializedProperty m_HeaderImageProperty;
		SerializedProperty m_IsClickEventHeaderOnlyProperty;

		SerializedProperty m_CollapseSizeProperty;
		SerializedProperty m_ExpandSizeProperty;

		SerializedProperty m_ContentsMaskProperty;
		SerializedProperty m_ContentsRootProperty;

		bool showLayoutSetting = false;

		protected void OnEnable() {

			m_CollapseSizeProperty = serializedObject.FindProperty( "CollapseSize" );
			m_ExpandSizeProperty = serializedObject.FindProperty( "ExpandSize" );

			m_ContentsMaskProperty = serializedObject.FindProperty( "ContentsMask" );
			m_ContentsRootProperty = serializedObject.FindProperty( "ContentsRoot" );

			m_TransitionProperty = serializedObject.FindProperty( "toggleTransition" );
			m_IsClickEventHeaderOnlyProperty = serializedObject.FindProperty( "isClickEventHeaderOnly" );
			m_CollapseSpriteProperty = serializedObject.FindProperty( "CollapseSprite" );
			m_ExpandSpriteProperty = serializedObject.FindProperty( "ExpandSprite" );
			m_HeaderImageProperty = serializedObject.FindProperty( "HeaderImage" );
			//		m_GroupProperty = serializedObject.FindProperty( "m_Group" );
			m_IsCollapsibleProperty = serializedObject.FindProperty( "m_IsCollapsible" );
//			m_IsExpandProperty = serializedObject.FindProperty( "m_IsExpand" );
			m_OnStateChangedProperty = serializedObject.FindProperty( "onStateChanged" );
		}

		public override void OnInspectorGUI() {

			serializedObject.Update();
			AccordionButton button_script = ( AccordionButton )serializedObject.targetObject;

			EditorGUILayout.PropertyField( m_ContentsMaskProperty );
			EditorGUILayout.PropertyField( m_ContentsRootProperty );

			EditorGUILayout.PropertyField( m_IsCollapsibleProperty );
			var expand = EditorGUILayout.Toggle( "Is Expand", button_script.isExpand );
			if ( expand != button_script.isExpand ) {
				button_script.isExpand = expand;
			}
//			EditorGUILayout.PropertyField( m_IsExpandProperty );
			EditorGUILayout.PropertyField( m_TransitionProperty );
			EditorGUILayout.PropertyField( m_IsClickEventHeaderOnlyProperty );
			EditorGUILayout.PropertyField( m_HeaderImageProperty );
			EditorGUILayout.PropertyField( m_CollapseSpriteProperty );
			EditorGUILayout.PropertyField( m_ExpandSpriteProperty );
			//		EditorGUILayout.PropertyField( m_GroupProperty );

			showLayoutSetting = EditorGUILayout.Foldout( showLayoutSetting, "Layout Setting" );
			if ( showLayoutSetting == true ) {
				EditorGUILayout.PropertyField( m_CollapseSizeProperty );
				EditorGUILayout.PropertyField( m_ExpandSizeProperty );

				if ( GUILayout.Button( "Calculate Layout" ) ) {
					button_script.CalculateLayout();
				}
			}

			EditorGUILayout.Space();

			// Draw the event notification options
			EditorGUILayout.PropertyField( m_OnStateChangedProperty );

			EditorGUILayout.Space();

			serializedObject.ApplyModifiedProperties();
		}



	}

}
