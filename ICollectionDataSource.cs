using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Nightingale {

	public interface ICollectionDataSource {
		int Count {
			get;
		}
		System.Object At( int index );
	}

}
