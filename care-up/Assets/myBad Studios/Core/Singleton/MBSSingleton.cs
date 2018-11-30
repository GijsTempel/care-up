using UnityEngine;

namespace MBS {
	public class MBSSingleton<T> : MonoBehaviour where T : Component {

		static T _instance;
		static public T Instance {
			get {
				if (null == _instance)
				{
					T[] instances = FindObjectsOfType<T>();
					if (null != instances && instances.Length > 0)
						_instance = instances[0];
					if (instances.Length > 1)
						for( int i = 1; i < instances.Length; i++)
							Destroy(instances[i]);
				}
                return _instance;
			}
		}

		virtual public void OnDestroy()
		{
			_instance = null;
		}
	}
}