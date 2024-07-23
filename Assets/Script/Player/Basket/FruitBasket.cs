// using System.Collections;
// using UnityEngine;
//
// public class FruitBasket : MonoBehaviour
// {
//     private FruitSelector _selector;
//     private PlayerBehavior _playerBehavior;
//     public GameObject[] slotbasket;
//     private int filledSlots = 0;
//     public float delay;
//
//     private void Start()
//     {
//         StartCoroutine(Initialiaze());
//     }
//
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.gameObject.CompareTag("fruit") && filledSlots < slotbasket.Length)
//         {
//             PlaceFruitInBasket(other.gameObject, filledSlots);
//             filledSlots++;
//             // _selector.anim.enabled = false;
//
//             if (filledSlots == slotbasket.Length)
//             {
//                 PlayerTuang();
//             }
//         }
//     }
//
//     void PlaceFruitInBasket(GameObject fruit, int slotIndex)
//     {
//         fruit.transform.position = slotbasket[slotIndex].transform.position;
//         fruit.transform.parent = slotbasket[slotIndex].transform;
//     }
//
//     public void UpdateFruitPosition(int slotIndex)
//     {
//         if (slotIndex < slotbasket.Length && slotbasket[slotIndex].transform.childCount > 0)
//         {
//             Transform fruit = slotbasket[slotIndex].transform.GetChild(0);
//             fruit.position = slotbasket[slotIndex].transform.position;
//         }
//     }
//
//     void PlayerTuang()
//     {
//         _playerBehavior.anime.SetBool(_playerBehavior.playerTuangAnimation, true);
//     }
//
//     private IEnumerator Initialiaze()
//     {
//         yield return new WaitForSeconds(0.1f);
//         _selector = FindObjectOfType<FruitSelector>();
//     }
// }