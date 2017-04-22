using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Nightingale {
	[System.Serializable]
	public class ObjectPool {

		[SerializeField]
		private List<GameObject> Available;

		[SerializeField]
		private List<GameObject> Use;

		public GameObject Origin;

		public int Count
		{
			get
			{
				return Available.Count + Use.Count;
			}
		}

		public int AvailableCount
		{
			get
			{
				return Available.Count;
			}
		}

		public int UseCount
		{
			get
			{
				return Use.Count;
			}
		}

		public IEnumerator UseItems() {
			return Use.GetEnumerator();
		}

		public void Clear() {

			ReleaseAll();

			int empty_count = Available.Count;

			for ( int i = 0; i < empty_count; ++i ) {
				GameObject item = Available[ 0 ];
				item.transform.SetParent( null );
#if UNITY_EDITOR
				GameObject.DestroyImmediate( item );
#else
				GameObject.Destroy( item );
#endif
				Available.RemoveAt( 0 );
			}

		}

		public void Initialize( GameObject origin_object, int size ) {

			if ( origin_object == null ) {
				Debug.LogWarning( "Origin is null!" );
				return;
			}

			Origin = origin_object;
			Clear();

			for ( int i = 0; i < size; ++i ) {
				Available.Add( GameObject.Instantiate( Origin ) );
			}

		}


		public GameObject Get() {

			if ( Available.Count == 0 ) {

				Debug.LogWarning( "Object No Stock!" );

				return null;
			}

			GameObject obj = Available[ 0 ];

			Available.RemoveAt( 0 );
			Use.Add( obj );

			return obj;

		}

		public void Release( GameObject obj ) {
			obj.SetActive( false );

			Use.Remove( obj );
			Available.Add( obj );
		}

		public void ReleaseAll() {

			Available.AddRange( Use );
			Use.Clear();

		}

	}
}