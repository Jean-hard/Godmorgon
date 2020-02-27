using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardDisplay : MonoBehaviour
{
    public BasicCard card;

    public int cardId;

    public Text nameText;
    public Text descriptionText;

    public Image artworkImage;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = card.name;
        descriptionText.text = card.description;
        artworkImage.sprite = card.artwork;
        cardId = card.id;
    }

}
