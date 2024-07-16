using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitCompleted : MonoBehaviour
{
    [SerializeField] private List<FruitHandler> slotPrefabs;
    [SerializeField] private List<FruitSelector> slotPiece;
    [SerializeField] private Transform slotParent, pieceParent;

    private readonly HashSet<int> _spawnedIndices = new HashSet<int>();
    private HashSet<int> placedIndices = new HashSet<int>();

    public float delay;

    void Start()
    {
        Spawn();
    }

    void Spawn(List<int> specificIndices = null)
    {
        if (specificIndices == null || specificIndices.Count == 0)
        {
            specificIndices = Enumerable.Range(0, slotPrefabs.Count)
                .Where(i => !placedIndices.Contains(i))
                .OrderBy(i => Random.value)
                .Take(3)
                .ToList();
        }

        int countToIterate = Math.Min(Math.Min(slotParent.childCount, pieceParent.childCount), specificIndices.Count);
        
        for (int i = 0; i < countToIterate; i++)
        {
            int index = specificIndices[i];

            if (index >= slotPrefabs.Count || index >= slotPiece.Count)
            {
                Debug.LogError($"Index {index} is out of bounds for _slotPrefabs or _slotPiece.");
                continue;
            }

            var spawnSlot = Instantiate(slotPrefabs[index], slotParent.GetChild(i).position, Quaternion.identity, slotParent.GetChild(i));
            var spawnPiece = Instantiate(slotPiece[index], pieceParent.GetChild(i).position, Quaternion.identity, pieceParent.GetChild(i));
            
            if (index == 3)
            {
                var spriteRenderer = spawnSlot.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = true;
                }
            }
            
            spawnPiece.Init(spawnSlot);
            _spawnedIndices.Add(index);
            
        }
    }
 
    public void OnFruitPlaced()
    {
        bool allPlaced = slotPiece.All(selector => selector.placed);
        if (allPlaced)
        {
            Debug.Log("All fruits placed!");
           
        }
    }

    public void UpdateGameState()
    {
        bool allPlaced = slotPiece.All(selector => selector.placed);
        if (allPlaced)
        {
            Debug.Log("All fruits placed!");

        }
    }

    public IEnumerator ResetGamePrepareNextStage()
    {
        yield return new WaitForSeconds(delay);
        List<int> availableIndices = Enumerable.Range(0, slotPrefabs.Count)
            .Where(i => !_spawnedIndices.Contains(i) && !placedIndices.Contains(i))
            .ToList();
        if (availableIndices.Count == 0)
        {
            Debug.Log("All fruits have been spawned, preparing next stage...");
            _spawnedIndices.Clear();
            placedIndices.Clear(); 
            availableIndices = Enumerable.Range(0, slotPrefabs.Count).ToList();
        }
        var selectedIndices = availableIndices.OrderBy(i => Random.value).Take(3).ToList();
        Spawn(selectedIndices);
        
        foreach (var fruitSelector in slotPiece)
        {
            if (fruitSelector.transform.parent != null)
            {
                fruitSelector.transform.parent.position = fruitSelector.originalParentPosition;
            }
        }
        
    }
}