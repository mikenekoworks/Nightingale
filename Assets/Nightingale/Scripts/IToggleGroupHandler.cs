using UnityEngine;
using System.Collections;

namespace Nightingale {
	public interface IToggleGroupHandler {
		void OnGroupNotify( bool value );
		bool GetState();
	}
}
