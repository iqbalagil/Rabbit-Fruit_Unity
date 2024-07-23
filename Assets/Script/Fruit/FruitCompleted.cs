using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitCompleted : MonoBehaviour
{
    [SerializeField] private List<FruitHandler> slotPrefabs;
    [SerializeField] private List<FruitSelector> slotPiece;
    [SerializeField] private Transform slotParent, pieceParent;

    private readonly HashSet<int> _spawnedIndices = new HashSet<int>();
    private HashSet<int> placedIndices = new HashSet<int>();

    public float delay;

    private static int _totalPlacedFruits = 0;

    public static event Action OnThreeFruitsPlaced;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        List<int> specificIndices = PickFruits();
        int countToIterate = Math.Min(Math.Min(slotParent.childCount, pieceParent.childCount), specificIndices.Count);

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

            spawnPiece.Init(spawnSlot);
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

    public void UpdateGameState()
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
            _totalPlacedFruits++;
        }
    }

    public static void IncrementPlacedFruitsCount()
    {
        _totalPlacedFruits++;
        if (_totalPlacedFruits % 3 == 0)
        {
            OnThreeFruitsPlaced?.Invoke();
        }
    }
}