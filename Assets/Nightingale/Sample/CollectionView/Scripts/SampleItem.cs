using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SampleItem : UIBehaviour, Nightingale.ICollectionViewItem {

	// Use this for initialization
	public void OnUpdate( object item_data ) {
		Debug.Log( "Id: " + ( (SampleData)item_data ).Id );
	}

}
