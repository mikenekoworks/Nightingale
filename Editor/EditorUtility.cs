using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Nightingale {

	public static class EditorUtility {

		public static GameObject CreateNewUIObject( string name, GameObject parent, Vector2 size ) {

			var new_obj = new GameObject();
			var rect_trans = new_obj.AddComponent<RectTransform>();
			rect_trans.sizeDelta = size;

			// 選択されているオブジェクトがあるならそれを親にする。
			if ( parent != null ) {
				new_obj.transform.SetParent( parent.transform );
			}

			new_obj.transform.localPosition = Vector3.zero;
			new_obj.name = GameObjectUtility.GetUniqueNameForSibling( new_obj.transform.parent, name );

			return new_obj;

		}

		public static GameObject CreateNewUIObject( string name, GameObject parent ) {
			return CreateNewUIObject( name, parent, new Vector2( 100.0f, 100.0f ) );
		}


		// DefaultControls.csからコピー
		public static void SetLayerRecursively( GameObject go, int layer ) {
			go.layer = layer;
			Transform t = go.transform;
			for ( int i = 0; i < t.childCount; i++ ) {
				SetLayerRecursively( t.GetChild( i ).gameObject, layer );
			}
		}

	}
}	// end of namespace

