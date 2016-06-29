using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Nightingale {

	[RequireComponent( typeof( RectTransform ) )]
	public class CollectionView : UIBehaviour {


		public ScrollRect ViewTarget;

		public ICollectionDataSource DataSource;
		public GameObject DataSourceObject;

		public int RowPosition;

		public int Row;
		public int Column;

		public Vector2 CellSize;

		public GameObject ViewItemResource;

		[SerializeField]
		public ObjectPool Pool;

		/*!
			@brief アイテム単体の初期化
		*/
		public void SetupItem( GameObject obj, Transform parent ) {

			if ( parent != null ) {
				obj.transform.SetParent( parent );
			}
			obj.transform.localPosition = Vector3.zero;
			obj.SetActive( false );
		}

		/*!
			@brief 指定した数のアイテムを生成して初期化
		*/
		public void InitializeViewItems( Transform parent, int count ) {

			Pool.Initialize( ViewItemResource, count );

			for ( int i = 0; i < count; ++i ) {
				SetupItem( Pool.Get(), parent );
			}

		}

		/*!
			@brief 全てのアイテムを削除する。
			*/
		public void DeleteAllViewItem() {

			Pool.Clear();

		}

		/*!
			@brief エディター時のレイアウト調整を行う
			
			*/
		public void EditorRebuildViewItemLayout() {

			Debug.Log( "RebuildViewItemLayout" );

			RemoveAllViewItem();

			Pool.ReleaseAll();

			// ひとまず縦方向だけを考慮して・・・。
			var size_delta = ViewTarget.content.sizeDelta;
			size_delta.y = Row * CellSize.y;
			// スクロールバーのサイズを考慮するのは後にしよう
			if ( ViewTarget.horizontalScrollbar != null ) {
			}
			if ( ViewTarget.verticalScrollbar != null ) {
			}
			ViewTarget.content.sizeDelta = size_delta;

			for ( int y = 0; y < Row; ++y ) {
				for ( int x = 0; x < Column; ++x ) {

					if ( Pool.AvailableCount == 0 ) {
						break;
					}

					var go = Pool.Get();
					go.SetActive( true );
					var rt = go.GetComponent<RectTransform>();

					rt.transform.localPosition = new Vector3(
						x * CellSize.x + rt.pivot.x * CellSize.x,
						-( y * CellSize.y + rt.pivot.y * CellSize.y ),
						0.0f
					);

				}
			}

		}

		public void RuntimeRebuildViewItemLayout() {

			Debug.Log( "RebuildViewItemLayout" );

			RemoveAllViewItem();

			Pool.ReleaseAll();

			// ひとまず縦方向だけを考慮して・・・。
			var size_delta = ViewTarget.content.sizeDelta;
			size_delta.y = Row * CellSize.y;
			// スクロールバーのサイズを考慮するのは後にしよう
			if ( ViewTarget.horizontalScrollbar != null ) {
			}
			if ( ViewTarget.verticalScrollbar != null ) {
			}
			ViewTarget.content.sizeDelta = size_delta;

			int data_count = DataSource.Count;
			int row_count = Mathf.CeilToInt( ( float )data_count / Column );

			int data_index = 0;

			for ( int y = 0; y < row_count; ++y ) {

				int col_count = ( data_count - ( Column * y ) >= Column ) ? Column : data_count % Column;

				for ( int x = 0; x < col_count; ++x ) {

					if ( Pool.AvailableCount == 0 ) {
						break;
					}

					var go = Pool.Get();
					go.SetActive( true );
					var rt = go.GetComponent<RectTransform>();

					rt.transform.localPosition = new Vector3(
						x * CellSize.x + rt.pivot.x * CellSize.x,
						-( y * CellSize.y + rt.pivot.y * CellSize.y ),
						0.0f
					);

					System.Object item_data = DataSource.At( data_index );
					var vi = go.GetComponent<ICollectionViewItem>();
					vi.OnUpdate( item_data );

					++data_index;
				}
			}
		}

		/*!
			@brief 使用中のアイテムを全部未使用にする。
		*/
		void RemoveAllViewItem() {
			var item = Pool.UseItems();
			while ( item.MoveNext() ) {
				GameObject go = ( GameObject )item.Current;
				go.SetActive( false );
			}
		}

		GameObject PopViewItem() {
			return null;
		}
		void PushViewItem( GameObject obj ) {
		}

		//void ItemUpdateAll() {

		//	if ( DataSource == null ) {
		//		return;
		//	}

		//	int top_position = RowPosition * Column;

		//	int data_count = DataSource.Count;
		//	System.Object obj = DataSource.At( 0 );


		//	//			RuntimeRebuildViewItemLayout();

		//}

		/*!
			@brief ICollectionDataSourceを指定した手動初期化
		*/
		public void SetupDataSource( ICollectionDataSource data_source ) {

			DataSource = data_source;

//			ItemUpdateAll();
		}

		public void OnValueChanged( Vector2 v ) {
			//			Debug.Log( v );
		}

		// Use this for initialization
		protected override void Start() {

			// DataSourceオブジェクトが指定されている時は、コンポーネントを取得して自動で初期化をする。
			if ( DataSourceObject != null ) {
				ICollectionDataSource data_source = DataSourceObject.GetComponent<ICollectionDataSource>();
				DataSource = data_source;

				RemoveAllViewItem();
				RuntimeRebuildViewItemLayout();

//				ItemUpdateAll();
			}

		}

		// Update is called once per frame
		void Update() {

		}
	}

}
