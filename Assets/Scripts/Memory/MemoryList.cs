using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MemoryList : MonoBehaviour
{
    public MemoryListItem memoryItemPrefab;

    private List<MemoryListItem> items = new();

    public float itemHeight = 140f;

    public void Add(MemoryEntry entry)
    {
        var position = new Vector2(0, -itemHeight * items.Count);
        var memoryItem = Instantiate(memoryItemPrefab, this.transform);
        (memoryItem.transform as RectTransform).anchoredPosition = position;
        memoryItem.Init(entry);
        items.Add(memoryItem);
    }
}
