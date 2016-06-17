using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Nightingale {

	[RequireComponent (typeof (RectTransform))]
	public class CollectionView : UIBehaviour {


	public ScrollRect ViewTarget;
		public ICollectionDataSource DataSource;
		//public bool PrepareItems;

		public int PrepareItemCount;

		public int Row;
		public int Column;

		public Vector2 CellSize;

		public GameObject ItemResource;

		[SerializeField]
		ObjectPool Pool;
		//private List<GameObject> ManagedItems;

		public void SetupItem( GameObject obj, Transform parent ) {
			
			if ( parent != null ) {
				obj.transform.SetParent( parent );
			}
			obj.transform.localPosition = Vector3.zero;
		}

		public void InitializeViewItems( Transform parent, int count ) {

			Pool.Initialize( ItemResource, count );

			for ( int i = 0; i < count; ++i ) {
				SetupItem( Pool.Pop(), parent );
			}

		}

		public void DeleteAllViewItem() {

			Pool.Clear();

		}

		public void EditorRebuildViewItemLayout() {

			//Debug.Log( "RebuildViewItemLayout" );

			//int item_count = Row * Column;
			//int diff_count = item_count - ManagedItems.Count;

			//if ( diff_count >= 0 ) {
			//	CreateViewItems( ViewTarget.content, diff_count );
			//} else {
			//	DeleteViewItem( Mathf.Abs( diff_count ) );
			//}

			//// ひとまず縦方向だけを考慮して・・・。
			//var size_delta = ViewTarget.content.sizeDelta;
			//size_delta.y = Row * CellSize.y;
			//// スクロールバーのサイズを考慮するのは後にしよう
			//if ( ViewTarget.horizontalScrollbar != null ) {
			//}
			//if ( ViewTarget.verticalScrollbar != null ) {
			//}
			//ViewTarget.content.sizeDelta = size_delta;

			//for ( int y = 0; y < Row; ++ y ) {
			//	for ( int x = 0; x < Column; ++x ) {

			//		var rt = ManagedItems[ y * Column + x ].GetComponent<RectTransform>();

			//		rt.transform.localPosition = new Vector3(
			//			x * CellSize.x + rt.pivot.x * CellSize.x,
			//			-( y * CellSize.y + rt.pivot.y * CellSize.y ),
			//			0.0f
			//		);

			//	}
			//}

		}

		public void RuntimeRebuildViewItemLayout() {

			Debug.Log( "RebuildViewItemLayout" );

			// ひとまず縦方向だけを考慮して計算
			int data_count = DataSource.Count;
			int draw_row_count = Mathf.CeilToInt( ( float )data_count / Column );

			var v = ViewTarget.content.sizeDelta;
			v.y = draw_row_count * CellSize.y;

			ViewTarget.content.sizeDelta = v;


			//for ( int y = 0; y < Row; ++y ) {
			//	for ( int x = 0; x < Column; ++x ) {
			//		var obj = ManagedItems[ y * Column + x ];
			//		if ( obj.activeSelf == false ) {
			//			continue;
			//		}
			//		var rt = obj.GetComponent<RectTransform>();

			//		rt.transform.localPosition = new Vector3(
			//			x * CellSize.x + rt.pivot.x * CellSize.x,
			//			-( y * CellSize.y + rt.pivot.y * CellSize.y ),
			//			0.0f
			//		);

			//	}
			//}

		}

		void RemoveAllViewItem() {
			//foreach ( var item in ManagedItems ) {
			//	item.SetActive( false );
			//}
		}

		GameObject PopViewItem() {
			return null;
		}
		void PushViewItem( GameObject obj ) {
		}

		void ItemUpdateAll() {

			if ( DataSource == null ) {
				return;
			}

			RemoveAllViewItem();
				
			int data_count = DataSource.Count;

			int i = 0;
			//foreach ( var item in ManagedItems ) {

			//	Debug.Log( "Data Index: " + i );

			//	if ( data_count > i ) {
			//		item.SetActive( true );

			//		var data_item = DataSource.At( i );

			//		var view_item = item.GetComponent< ICollectionViewItem >();
			//		view_item.OnUpdate( data_item );

			//	} else {

			//		item.SetActive( false );

			//	}

			//	++i;
			//}
			RuntimeRebuildViewItemLayout();

		}

		public void OnValueChanged( Vector2 v ) {
//			Debug.Log( v );
		}

		// Use this for initialization
		protected override void Start () {

			ItemUpdateAll();

		}
		
		// Update is called once per frame
		void Update () {

		}
	}

}
