using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public class LixPooledGameObject : MonoBehaviour
  {
    [HideInInspector] public LixGameObjectPool GameObjectPool; // GameObjectPool is set in GameObjectPool.OnTakeFromPool

    private void OnDisable()
    {
      if (GameObjectPool != null)
      {
        GameObjectPool.Pool.Release(gameObject);
      }
    }
  }
}