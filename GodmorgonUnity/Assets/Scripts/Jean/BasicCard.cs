using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Classe mère des types de carte (spell, stuff, move, ...)
 * Elle contient des variables communes à chaque type de carte
 * Elle permet que toutes les classes filles soient de scriptable object
 */
public class BasicCard : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite template;
    public Sprite artwork;
}
