using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public class LixGameObjectPool : LixObjectPool<GameObject>
  {
    private GameObject _prefab;
    public LixGameObjectPool(GameObject prefab, int defaultCapacity, int maxSize, bool collectionCheck = true) : base(defaultCapacity, maxSize, false, collectionCheck)
    {
      _prefab = prefab;

      base.Prewarm();

      Debug.Log("Created LixObjectPool pool for " + typeof(GameObject) + " with " + Pool.CountAll + " objects");
    }

    public override GameObject CreatePooledObject()
    {
      GameObject instance = GameObject.Instantiate(_prefab);
      instance.gameObject.SetActive(false);

      if (instance.GetComponent<LixPooledGameObject>() == null)
      {
        // Add the script to the targetGameObject
        instance.AddComponent(typeof(LixPooledGameObject));
      }

      return instance;
    }

    public override void OnTakeFromPool(GameObject instance)
    {
      // Instance.gameObject.SetActive(true);
      LixPooledGameObject pooledGameObject = instance.GetComponent<LixPooledGameObject>();
      pooledGameObject.GameObjectPool = this;
    }

    public override void OnReturnToPool(GameObject Instance)
    {
      Instance.gameObject.SetActive(false);
    }

    public override void OnDestroyObject(GameObject Instance)
    {
      GameObject.Destroy(Instance.gameObject);
    }
  }
}