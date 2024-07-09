using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FruitCompleted : MonoBehaviour
{
    [SerializeField] private List<FruitHandler> _slotPrefabs;
    [SerializeField] private List<FruitSelector> _slotPiece;
    [SerializeField] private FruitSelector _fruitPiece;
    [SerializeField] private Transform _slotParent, _pieceParent;

    void Start()
    {
        Spawn();
    }

    void Spawn() {
        var randomSpawn = _slotPrefabs.OrderBy(s => Random.value).Take(3).ToList(); 

        for (int i = 0; i < randomSpawn.Count; i++)
        {
            var spawnSlot = Instantiate(randomSpawn[i], _slotParent.GetChild(i).position, Quaternion.identity);

            var spawnPiece = Instantiate(_fruitPiece, _pieceParent.GetChild(i).position, Quaternion.identity);
            spawnPiece.Init(spawnSlot);
        }
    }
}
