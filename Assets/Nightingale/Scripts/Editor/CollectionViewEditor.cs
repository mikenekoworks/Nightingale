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
		SerializedProperty m_DataSourceObjectProperty;

		int PrepareItemCount;
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
			m_ItemResourceProperty = serializedObject.FindProperty( "ViewItemResource" );
			m_DataSourceObjectProperty = serializedObject.FindProperty( "DataSourceObject" );

			PrepareItemCount = view_script.Pool.Count;
		}

		public override void OnInspectorGUI() {

			bool rebuild_view_item_layout = false;
			serializedObject.Update();

			EditorGUILayout.PropertyField( m_ViewTargetProperty );

			CollectionView view_script = ( CollectionView )serializedObject.targetObject;

			EditorGUILayout.PropertyField( m_ItemResourceProperty );

			EditorGUILayout.LabelField( "DataSource" );
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField( m_DataSourceObjectProperty );
			string data_source_label = "Count: ";
			int data_souce_count = 0;
			if ( view_script.DataSource != null ) {
				data_souce_count = view_script.DataSource.Count;
			}
			EditorGUILayout.LabelField( data_source_label + data_souce_count );
			EditorGUI.indentLevel--;

			EditorGUILayout.LabelField( "View Layout" );
			EditorGUI.indentLevel++;
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField( m_CellSizeProperty );
			EditorGUILayout.PropertyField( m_RowProperty );
			EditorGUILayout.PropertyField( m_ColumnProperty );

			if ( EditorGUI.EndChangeCheck() ) {
				rebuild_view_item_layout = true;
			}
			EditorGUI.indentLevel--;

			ObjectPool pool_script = view_script.Pool;

			EditorGUILayout.LabelField( "Pool Object" );
			EditorGUI.indentLevel++;

			EditorGUILayout.LabelField( "Empty: " + pool_script.AvailableCount + " / Use: " + pool_script.UseCount );

//			EditorGUI.indentLevel--;

			var r = EditorGUILayout.BeginHorizontal();
			PrepareItemCount = EditorGUILayout.IntField( "Prepare Items", PrepareItemCount );
			if ( GUILayout.Button( "Create" ) == true ) {

				if ( view_script.ViewItemResource != null ) {

					// Row+1なのはスクロール時に一列余分に表示される可能性があるから。
					view_script.InitializeViewItems( view_script.ViewTarget.content, PrepareItemCount );

					rebuild_view_item_layout = true;
				}

			}
			if ( GUILayout.Button( "Delete" ) == true ) {
				view_script.ViewTarget.content.DetachChildren();
				view_script.DeleteAllViewItem();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUI.indentLevel--;

			//EditorGUILayout.IntField( view_script.PrepareItemCount );

			serializedObject.ApplyModifiedProperties();

			if ( rebuild_view_item_layout == true ) {
				view_script.EditorRebuildViewItemLayout();
			}

		}
	}

}