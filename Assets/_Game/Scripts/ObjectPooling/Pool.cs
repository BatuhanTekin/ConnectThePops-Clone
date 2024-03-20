using System.Collections;
using System.Collections.Generic;

namespace _game.Utility.ObjectPooling
{
    public class Pool<T> : IEnumerable where T : IPoolingObject
    {
        private List<T> _list = new List<T>();
        IFactory<T> factory;
        private T _poolObject;

        public Pool(IFactory<T> factory) : this(factory, 3)
        {
        }

        public Pool(IFactory<T> factory, int poolSize)
        {
            this.factory = factory;

            for (int i = 0; i < poolSize; i++)
            {
                Spawn();
            }
        }

        public T GetObject()
        {
            foreach (var poolObject in _list)
            {
                if (poolObject.IsDisabled())
                {
                    poolObject.SetEnable();
                    return poolObject;
                }
            }

            _poolObject = Spawn();
            _poolObject.SetEnable();
            return _poolObject;
        }

        public void ReturnPool(T poolObject)
        {
            poolObject.SetDisable();
        }

        private T Spawn()
        {
            _poolObject = factory.Spawn();
            _poolObject.OnSpawn();
            _poolObject.SetDisable();
            
            _list.Add(_poolObject);
            return _poolObject;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public List<T> GetPool()
        {
            return _list;
        }

        public bool IsExist(T target)
        {
            foreach (var member in _list)
            {
                if (ReferenceEquals(member, target))
                {
                    return true;
                }
            }

            return false;
        }
    }
}