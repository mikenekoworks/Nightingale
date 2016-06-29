using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SampleItem : UIBehaviour, Nightingale.ICollectionViewItem {

	public Text Rarity;
	public Image Icon;

	// Use this for initialization
	public void OnUpdate( object item_data ) {

		SampleData sample_data = ( SampleData )item_data;

		//Debug.Log( "Id: " + sample_data.Id );

		Rarity.text = "★" + sample_data.Rare;
		switch ( sample_data.Category ) {
		case TypeCategory.Weapon:
			if ( sample_data.Rare > 3 ) {
				Icon.sprite = Resources.Load<Sprite>( "Items/icon002" );
			} else {
				Icon.sprite = Resources.Load<Sprite>( "Items/icon001" );
			}

			break;
		case TypeCategory.Armor:
			Icon.sprite = Resources.Load<Sprite>( "Items/icon011" );
			break;
		case TypeCategory.Accessory:
			Icon.sprite = Resources.Load<Sprite>( "Items/icon015" );
			break;
		case TypeCategory.Potion:
			Icon.sprite = Resources.Load<Sprite>( "Items/icon030" );
			break;
		}
	}

}
