using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitCompleted : MonoBehaviour
{
    [SerializeField] private List<FruitHandler> _slotPrefabs;
    [SerializeField] private List<FruitSelector> _slotPiece;
    [SerializeField] private Transform _slotParent, _pieceParent;

    private HashSet<int> _spawnedIndices = new HashSet<int>();
    private HashSet<int> _placedIndices = new HashSet<int>();

    void Start()
    {
        Spawn();
    }

    void Spawn(List<int> specificIndices = null)
    {
        if (specificIndices == null || specificIndices.Count == 0)
        {
            specificIndices = Enumerable.Range(0, _slotPrefabs.Count)
                .Where(i => !_placedIndices.Contains(i))
                .OrderBy(i => Random.value)
                .Take(3)
                .ToList();
        }

        int countToIterate = Math.Min(Math.Min(_slotParent.childCount, _pieceParent.childCount), specificIndices.Count);

        for (int i = 0; i < countToIterate; i++)
        {
            int index = specificIndices[i];
            if (index >= _slotPrefabs.Count || index >= _slotPiece.Count)
            {
                Debug.LogError($"Index {index} is out of bounds for _slotPrefabs or _slotPiece.");
                continue;
            }

            var spawnSlot = Instantiate(_slotPrefabs[index], _slotParent.GetChild(i).position, Quaternion.identity, _slotParent.GetChild(i));
            var spawnPiece = Instantiate(_slotPiece[index], _pieceParent.GetChild(i).position, Quaternion.identity, _pieceParent.GetChild(i));
            spawnPiece.Init(spawnSlot);
            _spawnedIndices.Add(index);
            
        }
    }

    public void onFruitPlaced()
    {
        bool allPlaced = _slotPiece.All(selector => selector._placed);
        if (allPlaced)
        {
            Debug.Log("All fruits placed!");
           
        }
    }

    public void UpdateGameState()
    {
        bool allPlaced = _slotPiece.All(selector => selector._placed);
        if (allPlaced)
        {
            Debug.Log("All fruits placed!");

        }
    }

    public void ResetGamePrepareNextStage()
    {
        List<int> availableIndices = Enumerable.Range(0, _slotPrefabs.Count)
            .Where(i => !_spawnedIndices.Contains(i) && !_placedIndices.Contains(i))
            .ToList();
        if (availableIndices.Count == 0)
        {
            Debug.Log("All fruits have been spawned, preparing next stage...");
            _spawnedIndices.Clear();
            _placedIndices.Clear(); 
            availableIndices = Enumerable.Range(0, _slotPrefabs.Count).ToList();
        }
        var selectedIndices = availableIndices.OrderBy(i => Random.value).Take(3).ToList();
        Spawn(selectedIndices);
        
        foreach (var fruitSelector in _slotPiece)
        {
            if (fruitSelector.transform.parent != null)
            {
                fruitSelector.transform.parent.position = fruitSelector._originalParentPosition;
            }
        }
        
    }
}