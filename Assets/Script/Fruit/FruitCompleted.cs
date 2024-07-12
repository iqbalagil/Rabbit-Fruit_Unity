using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitCompleted : MonoBehaviour
{

    [SerializeField] private List<FruitHandler> _slotPrefabs;
    [SerializeField] private List<FruitSelector> _slotPiece;
    [SerializeField] private Transform _slotParent, _pieceParent;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        var indices = Enumerable.Range(0, _slotPrefabs.Count).OrderBy(i => Random.value).ToList();

        var selectedIndices = indices.Take(3).ToList();

        for (int i = 0; i < selectedIndices.Count; i++)
        {
            int index = selectedIndices[i];

            var spawnSlot = Instantiate(_slotPrefabs[index], _slotParent.GetChild(i).position, Quaternion.identity, _slotParent.GetChild(i));

            var spawnPiece = Instantiate(_slotPiece[index], _pieceParent.GetChild(i).position, Quaternion.identity, _pieceParent.GetChild(i));

            spawnPiece.Init(spawnSlot);
        }
    }

    

    
}
