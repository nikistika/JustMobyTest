using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Meta.ObjectPool
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly int _size;
        private readonly Transform _container;
        private readonly bool _autoExpand;
        private List<T> _objects;
        private IInstantiator _instantiator;

        public IEnumerable<T> Objects => _objects;

        public ObjectPool(T prefab, int size, Transform container, bool autoExpand, IInstantiator instantiator)
        {
            _prefab = prefab;
            _size = size;
            _container = container;
            _autoExpand = autoExpand;
            _instantiator = instantiator;
            
            CreatePool();
        }

        private void CreatePool()
        {
            _objects = new List<T>();
            for (int i = 0; i < _size; i++)
            {
                CreateObject();
            }
        }

        private T CreateObject(bool isActive = false)
        {
            T obj = _instantiator.InstantiatePrefabForComponent<T>(_prefab, _container);
            obj.gameObject.SetActive(isActive);
            _objects.Add(obj);
            return obj;
        }

        public T GetObject()
        {
            if (HasFreeObject(out T obj))
            {
                obj.gameObject.SetActive(true);
                return obj;
            }

            if (_autoExpand)
            {
                CreateObject(true);
            }

            return null;
        }

        private bool HasFreeObject(out T monoObj)
        {
            foreach (var obj in _objects)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    monoObj = obj;
                    return true;
                }
            }

            monoObj = null;
            return false;
        }
    }
}