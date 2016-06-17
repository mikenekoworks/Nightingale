using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SampleDataSource : Nightingale.ICollectionDataSource {

	List< SampleData > Collection;

	public SampleDataSource() {
		Collection = new List<SampleData>();
	}

	public void Add( SampleData new_data ) {
		Collection.Add( new_data );
	}


	public int Count {
		get {
			return Collection.Count;
		}

	}

	public object At( int index ) {
		return Collection[ index ];
	}

}
