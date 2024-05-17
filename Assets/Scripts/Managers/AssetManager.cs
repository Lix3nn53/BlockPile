using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class AssetManager : MonoBehaviour
{
    // Singleton
    public static AssetManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Assets
    public GameObjectPool GridSlotPool;
    public GameObjectPool BlockPilePool;
    public GameObjectPool BlockPool;
}
