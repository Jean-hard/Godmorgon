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

    private bool isHandUpdated;

    // Start is called before the first frame update
    void Start()
    {
        if (!isHandUpdated)
        {
            //HandSetup();
            isHandUpdated = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
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
    */
}
