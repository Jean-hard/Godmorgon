using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public void OnUse()
    {
        Destroy(gameObject);
    }

    public void MovingEffect(int orientionChoice)
    {
        TilemapManager.Instance.DirectionForPlayer(orientionChoice);
        //////////faire les liaisons pour les cartes
    }
}
