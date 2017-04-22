using UnityEngine;
using System.Collections;

namespace Nightingale {

	interface ICollectionViewItem {
		void OnUpdate( object item_data );
	}

}