using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;

namespace Lix.Core
{
  public class PooledGameObject : MonoBehaviour
  {
    [HideInInspector] public GameObjectPool GameObjectPool; // GameObjectPool is set in GameObjectPool.OnTakeFromPool

    private void OnDisable()
    {
      DelayedReturnToPool();
    }

    private async void DelayedReturnToPool()
    {
      await UniTask.Delay(1000);

      if (GameObjectPool != null)
      {
        GameObjectPool.Pool.Release(gameObject);
      }
    }
  }
}