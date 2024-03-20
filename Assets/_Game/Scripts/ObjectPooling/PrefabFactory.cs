using UnityEngine;

namespace _game.Utility.ObjectPooling
{
	public class PrefabFactory<T> : IFactory<T> where T : MonoBehaviour {
		
		GameObject _prefab;
		private Transform _parent;
		private GameObject _spawnedObject;

		public PrefabFactory(GameObject prefab, Transform parent) {
			_prefab = prefab;
			_parent = parent;
		}

		public T Spawn()
		{
			_spawnedObject = InstantiateGameObject();
			return _spawnedObject.GetComponent<T>();
		}

		private GameObject InstantiateGameObject()
		{
			return Object.Instantiate(_prefab, _parent);
		}
	}	
}

