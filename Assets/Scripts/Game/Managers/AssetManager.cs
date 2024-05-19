using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;
using AYellowpaper.SerializedCollections;

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
    [SerializeField] private SerializedDictionary<BlockColorType, string> ColorCodes = new SerializedDictionary<BlockColorType, string>();
    public Color GetColor(BlockColorType color)
    {
        Color result = Color.white;

        if (ColorCodes.ContainsKey(color))
        {
            ColorUtility.TryParseHtmlString("#" + ColorCodes[color], out result);
        }

        return result;
    }
}
