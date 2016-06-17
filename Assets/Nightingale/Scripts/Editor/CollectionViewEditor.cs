using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Events;
using System.Collections;
using System.Collections.Generic;

namespace Nightingale {

	[CustomEditor( typeof( CollectionView ), true )]
	[CanEditMultipleObjects]
	public class CollectionViewEditor : Editor {

		SerializedProperty m_ViewTargetProperty;

		SerializedProperty m_RowProperty;
		SerializedProperty m_ColumnProperty;
		SerializedProperty m_CellSizeProperty;
		SerializedProperty m_ItemResourceProperty;

		protected void OnEnable() {

			CollectionView view_script = ( CollectionView )serializedObject.targetObject;

			if ( view_script.ViewTarget == null ) {
				view_script.ViewTarget = view_script.gameObject.GetComponent<ScrollRect>();

				UnityEventTools.AddPersistentListener( view_script.ViewTarget.onValueChanged, view_script.OnValueChanged );
			}
			m_ViewTargetProperty = serializedObject.FindProperty( "ViewTarget" );

			m_RowProperty = serializedObject.FindProperty( "Row" );
			m_ColumnProperty = serializedObject.FindProperty( "Column" );
			m_CellSizeProperty = serializedObject.FindProperty( "CellSize" );
			m_ItemResourceProperty = serializedObject.FindProperty( "ItemResource" );
		}

		public override void OnInspectorGUI() {

			bool rebuild_view_item_layout = false;
			serializedObject.Update();

			EditorGUILayout.PropertyField( m_ViewTargetProperty );
			EditorGUILayout.PropertyField( m_ItemResourceProperty );

			CollectionView view_script = ( CollectionView )serializedObject.targetObject;

			EditorGUILayout.LabelField( "View Layout" );
			EditorGUI.indentLevel++;
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField( m_CellSizeProperty );
			EditorGUILayout.PropertyField( m_RowProperty );
			EditorGUILayout.PropertyField( m_ColumnProperty );

			if ( EditorGUI.EndChangeCheck() ) {
				rebuild_view_item_layout = true;
			}

			var r = EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button( "Prepare Items" ) == true ) {

				if ( view_script.ItemResource != null ) {

					view_script.InitializeViewItems( view_script.ViewTarget.content, view_script.Row * view_script.Column );

					rebuild_view_item_layout = true;
				}

			}
			if ( GUILayout.Button( "Remove Items" ) == true ) {
				view_script.ViewTarget.content.DetachChildren();
				view_script.DeleteAllViewItem();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUI.indentLevel--;

			EditorGUILayout.IntField( view_script.PrepareItemCount );

			serializedObject.ApplyModifiedProperties();

			if ( rebuild_view_item_layout == true ) {
				view_script.EditorRebuildViewItemLayout();
			}

		}
	}

}