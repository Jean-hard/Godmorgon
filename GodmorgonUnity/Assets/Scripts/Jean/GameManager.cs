using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerGUI playerGUI;

    [SerializeField]
    private GameObject hand;

    private bool isCardSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isCardSelected)
            HandSetup();
    }

    private void HandSetup()
    {
        Transform[] cardsInHand = hand.gameObject.GetComponentsInChildren<Transform>();   // tableau contenant les cartes en main --> TODO: les récup du gameengine
        foreach (Transform _card in cardsInHand)
        {
            if (_card.GetComponent<Button>())
            {
                Button btn = _card.GetComponent<Button>();
                string btnClickedName = _card.name;
                btn.onClick.AddListener(delegate { SelectCard(btnClickedName); });
            }
        }
    }

    

    //Sélectionne la carte sur laquelle on a cliqué
    public void SelectCard(string btnClickedName) 
    {
        Debug.Log("Card selected : " + btnClickedName);
        isCardSelected = true;
    }

}
