using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SampleDataSource : MonoBehaviour, Nightingale.ICollectionDataSource {

	List<SampleData> SampleDataCollection = new List<SampleData>();

	public void Awake() {

		for ( int i = 0; i < 8; ++i ) {

			SampleData new_data = new SampleData();
			new_data.Id = i;
			new_data.Rare = Random.Range( 1, 5 );
			new_data.Name = "アイテム" + i;
			new_data.Category = ( TypeCategory )Random.Range( 0, 3 );

			SampleDataCollection.Add( new_data );
		}
	}

	public void Add( SampleData new_data ) {
		SampleDataCollection.Add( new_data );
	}

	public int Count
	{
		get
		{
			return SampleDataCollection.Count;
		}

	}

	public object At( int index ) {
		return SampleDataCollection[ index ];
	}
}
