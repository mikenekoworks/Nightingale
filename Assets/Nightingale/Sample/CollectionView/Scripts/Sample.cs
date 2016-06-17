using UnityEngine;
using System.Collections;

public class Sample : MonoBehaviour {

	public Nightingale.CollectionView CollectionView;
	public SampleDataSource DataSource;


	public void Awake() {
		DataSource = new SampleDataSource();

		for ( int i = 0; i < 8; ++i ) {

			SampleData new_data = new SampleData();
			new_data.Id = i;
			new_data.Rare = 3;
			new_data.Name = "アイテム" + i;

			DataSource.Add( new_data );
		}

		CollectionView.DataSource = DataSource;


	}

}
