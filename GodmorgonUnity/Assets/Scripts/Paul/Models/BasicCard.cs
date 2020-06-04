using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{

    /**
     * Classe mère des types de carte (attack, defense, move, ...)
     * Elle contient des variables communes à chaque type de carte
     * Elle permet que toutes les classes filles soient de scriptable object
     */
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/BasicCard")]
    public class BasicCard : ScriptableObject
    {
        public enum CARDTYPE
        {
            MOVE,
            ATTACK,
            DEFENSE,
            POWER_UP,
            SPELL,
            SIGHT,
            CURSE,
        }

        public CARDTYPE cardType = CARDTYPE.MOVE;
        public int id;
        public new string name;

        [TextArea]
        public string description;
        
        public int actionCost = 1;
        public int price = 0;

        public Sprite template;
        public Sprite artwork;

        public CardEffectData[] effectsData;

        #region GET_DATA

        //retourne les dégats de base de la carte
        public int GetRealDamage()
        {
            int damageData = 0;
            foreach (CardEffectData effect in effectsData)
            {
                damageData += effect.damagePoint;
                //check pour les autres effets
                if (effect.shiver)
                    damageData = damageData * 2;
                else if (effect.trust && BuffManager.Instance.IsTrustValidate(effect.trustNb))
                    damageData = damageData * 2;
            }

            return damageData;
        }

        //retourne les blocks de base de la carte
        public int GetRealBlock()
        {
            int blockData = 0;
            foreach (CardEffectData effect in effectsData)
            {
                blockData += effect.nbBlock;
                if (effect.shiver)
                    blockData = blockData * 2;
                else if (effect.trust && BuffManager.Instance.IsTrustValidate(effect.trustNb))
                    blockData = blockData * 2;
            }
            return blockData;
        }

        //retourne les mouvements de base de la carte
        public int GetRealMove()
        {
            int moveData = 0;
            foreach (CardEffectData effect in effectsData)
            {
                moveData += effect.nbMoves;
                if (effect.shiver)
                    moveData = moveData * 2;
                else if (effect.trust && BuffManager.Instance.IsTrustValidate(effect.trustNb))
                    moveData = moveData * 2;
            }
            return moveData;
        }

        //retourne le heal de base de la carte
        public int GetRealHeal()
        {
            int healData = 0;
            foreach (CardEffectData effect in effectsData)
            {
                healData += effect.nbHeal;
                if (effect.shiver)
                    healData = healData * 2;
                else if (effect.trust && BuffManager.Instance.IsTrustValidate(effect.trustNb))
                    healData = healData * 2;
            }
            return healData;
        }

        //retourne le nombre de carte à piocher de base de la carte
        public int GetRealNbDraw()
        {
            int nbDrawData = 0;
            foreach (CardEffectData effect in effectsData)
            {
                nbDrawData += effect.nbCardToDraw;
                if (effect.shiver)
                    nbDrawData = nbDrawData * 2;
                else if (effect.trust && BuffManager.Instance.IsTrustValidate(effect.trustNb))
                    nbDrawData = nbDrawData * 2;
            }
            return nbDrawData;
        }
        #endregion
    }
}
