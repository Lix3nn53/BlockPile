using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public class LixGameObjectPoolList : MonoBehaviour
  {
    private Dictionary<Guid, LixGameObjectPool> _poolDictionary = new();


    public Guid CreateNewPool(GameObject prefab, int defaultCapacity, int maxSize)
    {
      LixGameObjectPool pool = new LixGameObjectPool(prefab, defaultCapacity, maxSize);

      Guid id = Guid.NewGuid();

      _poolDictionary.Add(id, pool);

      return id;
    }

    public LixGameObjectPool GetPool(Guid id)
    {
      return _poolDictionary[id];
    }

    private void OnDestroy()
    {
      foreach (KeyValuePair<Guid, LixGameObjectPool> kvp in _poolDictionary)
      {
        kvp.Value.Pool.Clear();
      }
    }
  }
}