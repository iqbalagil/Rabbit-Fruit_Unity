using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class FruitCompleted : MonoBehaviour
{
    [SerializeField] private List<GameObject> slotPrefabs;
    [SerializeField] private List<FruitSelector> slotPiece;
    [SerializeField] private Transform slotParent, pieceParent;

    private readonly HashSet<int> _spawnedIndices = new HashSet<int>();
    private int _spawnCount = 0;
    public float delay;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        _spawnCount++;
        if (_spawnCount >= 4)
        {
            Debug.Log("Spawn limit reached.");
            return;
        }

        List<int> specificIndices = PickFruits();
        int countToIterate = Math.Min(slotParent.childCount, Math.Min(pieceParent.childCount, specificIndices.Count));

        for (int i = 0; i < countToIterate; i++)
        {
            int index = specificIndices[i];
            if (index >= slotPrefabs.Count || index >= slotPiece.Count)
            {
                Debug.LogError($"Index {index} is out of bounds for slotPrefabs or slotPiece.");
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

            SpriteRenderer sr = spawnSlot.GetComponent<SpriteRenderer>();
            spawnPiece.Init(sr);
            _spawnedIndices.Add(index);
        }
        
    }

    public void OnFruitPlaced()
    {
        if (slotPiece.All(selector => selector.placed))
        {
            Debug.Log("All fruits placed!");
        }
    }

    List<int> PickFruits()
    {
        if (slotPrefabs.Count - _spawnedIndices.Count < 3) _spawnedIndices.Clear();

        return Enumerable.Range(0, slotPrefabs.Count)
            .Where(i => !_spawnedIndices.Contains(i))
            .OrderBy(i => UnityEngine.Random.value)
            .Take(3)
            .ToList();
    }

    public IEnumerator ResetGamePrepareNextStage()
    {
        yield return new WaitForSeconds(delay);
        Spawn();

        foreach (var fruitSelector in slotPiece)
        {
            if (fruitSelector.transform.parent != null)
            {
                fruitSelector.transform.parent.position = fruitSelector.originalParentPosition;
            }
        }
    }
}