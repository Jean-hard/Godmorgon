using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField]
    private GameObject handLayoutGroup;

    [SerializeField]
    private List<GameObject> cardDeck;

    private static DeckManager _instance;

    public static DeckManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    //call to draw new card from deck
    public void NewCard()
    {
        if (handLayoutGroup.transform.childCount > 3)
            Debug.Log("nombre de carte en main maximal");
        else
        {
            int randCardInDeck = Random.Range(0, 3);

            GameObject newCard = Instantiate(cardDeck[randCardInDeck]);
            newCard.transform.SetParent(handLayoutGroup.transform);
        }
    }
}
