using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using GodMorgon.Models;

/**
 * Création d'un database pour toutes les cartes
 * Elle se load en récupérant toutes les cartes(scriptables objects) du dossier Resources/Cards
 * Elle permet de récupérer les classes dérivées de la classe mère BasicCard(de type ScriptableObjects), et récup les variables propres à ces classes dérivées
 * Voir dans PlayerGUI.cs comment appeler la fonction GetCard 
 */

public class CardsDataBase 
{
    static private Dictionary<int, BasicCard> cardsDict = new Dictionary<int, BasicCard>();
    static private bool isDatabaseLoaded = false;


    static public void LoadDatabase()
    {
        if (isDatabaseLoaded) return;

        LoadDatabaseForce();
    }

    static public void LoadDatabaseForce()
    {
        cardsDict.Clear();
        BasicCard[] resources = Resources.LoadAll<BasicCard>("Cards"); // Load all cards from the Resources/Cards folder
        foreach (BasicCard card in resources)
        {
            cardsDict[card.id] = card;
        }
        Debug.Log("Cards database is load !");
        isDatabaseLoaded = true;
    }

    static public void ClearDatabase() // Clear database to free up memory
    {
        isDatabaseLoaded = false;
        cardsDict.Clear();
    }

    public static T GetCard<T>(int id) where T : BasicCard
    {
        if (!isDatabaseLoaded)
            LoadDatabaseForce();

        BasicCard card;
        if (!cardsDict.TryGetValue(id, out card))
        {
            return null;
        }

        Type baseType = typeof(T);
        Type cardType = card.GetType();

        if (baseType.IsAssignableFrom(cardType))
        {
            return ScriptableObject.Instantiate(card) as T;
        }

        return null;
    }
}
