using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public class GameObjectPool : ObjectPoolMono<GameObject>
  {
    [SerializeField] private GameObject _prefab;

    public override GameObject CreatePooledObject()
    {
      GameObject instance = Instantiate(_prefab);
      instance.gameObject.SetActive(false);

      if (instance.GetComponent<PooledGameObject>() == null)
      {
        // Add the script to the targetGameObject
        instance.AddComponent(typeof(PooledGameObject));
      }

      return instance;
    }

    public override void OnTakeFromPool(GameObject instance)
    {
      // Instance.gameObject.SetActive(true);
      PooledGameObject pooledGameObject = instance.GetComponent<PooledGameObject>();
      pooledGameObject.GameObjectPool = this;
    }

    public override void OnReturnToPool(GameObject Instance)
    {
      Instance.gameObject.SetActive(false);
    }

    public override void OnDestroyObject(GameObject Instance)
    {
      Destroy(Instance.gameObject);
    }
  }
}