using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

using GodMorgon.StateMachine;
using GodMorgon.Player;
using GodMorgon.Enemy;
using GodMorgon.VisualEffect;
using System;
using GodMorgon.Models;
using GodMorgon.Sound;

public class PlayerManager : MonoBehaviour
{
    [Header("Player View")]
    public List<Sprite> spriteList = new List<Sprite>();
    private SpriteRenderer spriteRenderer;

    [Header("UI Settings")]
    public Image healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI blockText;
    public TextMeshProUGUI goldValueText;
    public TextMeshProUGUI tokenText;

    [Header("Movement Settings")]
    public float playerSpeed = 1f;
    public AnimationCurve playerMoveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    private bool playerCanMove = false;
    private bool playerHasMoved = false;
    [NonSerialized]
    public bool isMoving = false;
    private int spotIndex;
    private List<Spot> playerPathList;

    //DONNEES RECUP DE LA CARTE MOVE
    private CardEffectData[] _cardEffectDatas;

    private int nbMoveIterationCounter = 0; //nb d'iterations de move effectuées
    private bool accessibleShown = false;
    private bool canLaunchOtherMove = false;

    [NonSerialized]
    public int multiplier = 1;

    private Vector3Int endPos;
    [NonSerialized]
    public Vector3Int supposedPos; //Position supposée du joueur = centre room lorsqu'il est dans la room d'un ennemi (utilisée pour les déplacements suite à ce cas)

    // nombre de tiles parcourues pour 1 move
    private int nbRoomsToMove = 1;

    [NonSerialized]
    public bool isFirstInRoom = true;

    //all visual effect for the player
    [Header("Visual Effect")]
    public ParticleSystemScript playerHit = null;
    public ParticleSystemScript playerShield = null;
    public ParticleSystemScript playerPowerUp = null;
    public ParticleSystemScript playerKillerInstinct = null;
    public ParticleSystemScript playerCounter = null;
    public ParticleSystemScript playerFastShoes = null;
    public List<ParticleSystemScript> wheelParticules = new List<ParticleSystemScript>();

    #region Singleton Pattern
    private static PlayerManager _instance;

    public static PlayerManager Instance { get { return _instance; } }
    #endregion

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

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthText();
        //UpdateBlockText();
        UpdateGoldText();
        UpdateTokenText();

        SetHealthMax(PlayerData.Instance.health + PlayerData.Instance.defense);

        nbMoveIterationCounter = 0;
        supposedPos = GetPlayerCellPosition();
        if (this.gameObject.GetComponentInChildren<SpriteRenderer>() != null)
            spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        else Debug.Log("Sprite of player not found");
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(supposedPos);
        }

        LaunchMoveMechanic();

        if(null != _cardEffectDatas)    //Si on a un card effect data
        {
            if (nbMoveIterationCounter < nbRoomsToMove * multiplier && canLaunchOtherMove) //S'il nous reste des moves à faire et qu'on est autorisé à le faire
            {
                ShowNewAccessible();    //On affiche les effets sur les nouvelles tiles disponibles

                //On reset les particules avant d'attribuer les nouvelles
                foreach (ParticleSystemScript particule in wheelParticules)
                {
                    particule.stopParticle();
                }

                if (Input.GetMouseButtonDown(0))    //Lors du click
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
                    Vector3Int dropCellPosition = TilesManager.Instance.walkableTilemap.WorldToCell(mousePos);      //Recup la position de la souris au moment du click

                    if (TilesManager.Instance.accessibleTiles.Contains(dropCellPosition))   //Si la tile clickée fait partie de celles accessibles
                    {
                        canLaunchOtherMove = false; //On ne peut pas relancer un move
                        accessibleShown = false;    
                        TilesManager.Instance.HideAccessibleTiles();    //Cache les effets de tiles accessibles
                        
                        MovePlayer();    //Lance le move du joueur                        
                    }
                }
            }
        }
    }

    /**
     * Récup les données de la carte de type MOVE
     */
    public void UpdateMoveDatas(CardEffectData[] cardEffectData)
    {
        _cardEffectDatas = cardEffectData;
    }

    /**
     * Renvoie les données de la carte move jouée
     */
    public CardEffectData[] GetCardEffectData()
    {
        if (_cardEffectDatas != null)
            return _cardEffectDatas;
        else
            return null;
    }

    /**
     * Update le multiplier si le trust de la carte move est activé
     */
    public void UpdateMultiplier(int valueToAddToMultiplier)
    {
        multiplier = valueToAddToMultiplier;
    }

    /**
     * Détermine la liste de tiles (=path) à parcourir jusqu'à l'endroit où la carte mouvement est déposée
     */
    public void MovePlayer()
    {
        nbRoomsToMove = BuffManager.Instance.getModifiedMove(_cardEffectDatas[0].nbMoves);  //Update le nombre de rooms à parcourir, qui changera en fct du nb sur la carte et si un fast shoes a été joué

        GameManager.Instance.DownPanelBlock(true);  //Block le down panel pour que le joueur ne puisse pas jouer de carte pendant le mouvement

        //Transpose la position de la souris au moment du drop de carte en position sur la grid, ce qui donne donc la tile sur laquelle on a droppé la carte
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
        endPos = TilesManager.Instance.walkableTilemap.WorldToCell(mouseWorldPos);

        Vector3Int dropRoomCellPosition = new Vector3Int(0, 0, 0);
        if (null != TilesManager.Instance.roomTilemap)  //Si le TilesManager possède bien la roomTilemap
            dropRoomCellPosition = TilesManager.Instance.roomTilemap.WorldToCell(endPos);

        List<Spot> _roadPath = TilesManager.Instance.roadPath;

        Vector3 playerPos = this.transform.position;
        Vector3Int playerCellPos = TilesManager.Instance.walkableTilemap.WorldToCell(playerPos);  //Position du player en format cell

        playerPathList = new List<Spot>();

        spotIndex = 0;

        if (_roadPath != null && _roadPath.Count > 0) //reset le roadpath
            _roadPath.Clear();


        //création du path, prenant en compte la position des tiles, le point de départ, le point d'arrivée, et la longueur en tiles du path
        //roadPath est une liste de spots = une liste de positions de tiles
        _roadPath = TilesManager.Instance.astar.CreatePath(TilesManager.Instance.spots, new Vector2Int(playerCellPos.x, playerCellPos.y), new Vector2Int(endPos.x, endPos.y), 5); //* nbMoves

        bool isEnemyOnPath = false;

        if (_roadPath == null) return;

        //on ajoute les tiles par lesquelles il va devoir passer sauf celle où il y a un enemy
        if (null != EnemyManager.Instance.GetEnemyViewByPosition(new Vector3Int(_roadPath[0].X, _roadPath[0].Y, 0)))
        {
            EnemyManager.Instance.GetEnemyViewByPosition(new Vector3Int(_roadPath[0].X, _roadPath[0].Y, 0)).enemyData.inPlayersRoom = true;
            isEnemyOnPath = true;
            isFirstInRoom = false;

            supposedPos = new Vector3Int(_roadPath[0].X, _roadPath[0].Y, 0);    //La position supposée est celle de l'ennemi sur le path
            _roadPath.Remove(_roadPath[0]);
        }

        playerPathList = _roadPath;

        //Si on ETAIT le premier arrivé dans la room, alors un ennemi présent dans la room doit se recentrer au milieu de la room
        if (isFirstInRoom)
            EnemyManager.Instance.RecenterEnemiesAfterPlayerMove();


        playerPathList.Reverse(); //on inverse la liste pour la parcourir de la tile la plus proche à la plus éloignée
        playerPathList.RemoveAt(0);    

        //Si on n'a pas d'ennemi sur le chemin, on est le premier arrivé dans la room
        if (!isEnemyOnPath)
        {
            supposedPos = new Vector3Int(playerPathList[playerPathList.Count - 1].X, playerPathList[playerPathList.Count - 1].Y, 0); //position supposée = dernière tile du path

            isFirstInRoom = true;
            foreach (EnemyView enemy in EnemyManager.Instance.GetEnemiesInPlayersRoom())
            {
                enemy.enemyData.inPlayersRoom = false;
            }
        }

        //On reset les particules avant d'attribuer les nouvelles
        foreach(ParticleSystemScript particule in wheelParticules)
        {
            particule.stopParticle();
        }

        //On update le sprite du player en fonction de sa direction
        if (playerPathList[0].Y > playerCellPos.y)
        {
            UpdatePlayerSprite("haut_gauche");
            wheelParticules[1].launchParticle();
        } 
        else if (playerPathList[0].X > playerCellPos.x)
        {
            UpdatePlayerSprite("haut_droite");
            wheelParticules[2].launchParticle();
        }
        else if (playerPathList[0].X < playerCellPos.x)
        {
            UpdatePlayerSprite("bas_gauche");
            wheelParticules[3].launchParticle();
        }
        else if (playerPathList[0].Y < playerCellPos.y)
        {
            UpdatePlayerSprite("bas_droite");
            wheelParticules[0].launchParticle();
        }

        /*    
        foreach (Spot spot in playerPathArray)
        {
            Debug.Log(spot.X + " " + spot.Y);
        }*/

        playerCanMove = true;  //on autorise le player à bouger

        spotIndex = 0;

        nbMoveIterationCounter++;   //On ajoute un move au compteur

        //SFX player  move
        MusicManager.Instance.PlayPlayerMove();
    }

    /**
     * Lance le mouvement du player
     */
    private void LaunchMoveMechanic()
    {
        if(!playerCanMove)
        {
            return;
        }

        isMoving = true;

        //la prochaine position est le spot parmi la liste de spots
        Vector3 nextPos = TilesManager.Instance.walkableTilemap.CellToWorld(new Vector3Int(playerPathList[spotIndex].X, playerPathList[spotIndex].Y, 0))
            + new Vector3(0, 0.3f, 0);   //on ajoute 0.x pour que le player passe bien au milieu de la tile, la position de la tile étant en bas du losange             


        if (Vector3.Distance(this.transform.position, nextPos) < 0.001f)
        {
            //si on arrive à la tile finale où le player peut se rendre
            if (spotIndex == playerPathList.Count - 1)
            {
                playerCanMove = false;
                isMoving = false;
                spotIndex = 0;
                
                StartCoroutine(LaunchActionsInNewRoom());  //Attends avant de permettre un autre move (pour ralentir le rythme)
                
            }
            else if (spotIndex < playerPathList.Count - 1)
            {
                spotIndex++;    //on passe à la tile suivante tant qu'on a pas atteint la dernière
            }
        }
        else
        {
            float ratio = (float)spotIndex / (playerPathList.Count - 1);   //ratio varie entre 0 et 1, 0 pour le spot le plus proche et 1 pour le spot final
            ratio = playerMoveCurve.Evaluate(ratio);     //on le lie à notre curve pour le modifier dans l'inspector à notre guise
            float speed = playerSpeed * ratio;   //on le lie à la vitesse pour que la curve ait un impact sur la vitesse du player
            this.transform.position = Vector2.MoveTowards(this.transform.position, nextPos, speed * Time.deltaTime);
        }
    }

    /**
     * Une fois arrivé à destination, active la suite des évènements après qq secondes
     */
    IEnumerator LaunchActionsInNewRoom()
    {
        bool enemySlayed = false;

        if(_cardEffectDatas[0].noBrakes)    //Si la carte était une No brakes, on lance l'attaque sur les ennemis avant le reste des actions
        {
            enemySlayed = RoomAttack(); //Lance l'attaque de room et renvoie true si un ennemi est mort lors de cette attaque
            yield return new WaitForSeconds(1f);
        }

        RoomEffectManager.Instance.LaunchRoomEffect(GetPlayerRoomPosition());   //Lance l'effet de room sur laquelle on vient d'arriver
        
        

        yield return new WaitForSeconds(2f);
        canLaunchOtherMove = true;  //On permet le lancement d'un autre move
        if (nbMoveIterationCounter >= nbRoomsToMove * multiplier)  //Si on a atteint le nombre de moves possibles de la carte
        {
            canLaunchOtherMove = false;
            nbMoveIterationCounter = 0;
            multiplier = 1;
            if (RoomEffectManager.Instance.currentRoom.roomEffect == RoomEffect.CHEST)
            {
                Debug.Log("dans chest room alors lance draft");
                StartCoroutine(LaunchDraft());
            }
            else
            {
                if(enemySlayed)
                {
                    while (GameManager.Instance.draftPanelActivated)
                        yield return null;

                    playerHasMoved = true;
                }
                else
                    playerHasMoved = true;  //Le joueur a terminé l'effet de la carte move
            
            }
        }
        /*
        else if (nbMoveIterationCounter >= nbRoomsToMove * multiplier)   //Si c'est notre dernier move et qu'un enemy a été tué
        {
            canLaunchOtherMove = false;
            nbMoveIterationCounter = 0;
            multiplier = 1;
            Debug.Log("dans on a tué un ennemy avec brakes alors lance draft");
            //StartCoroutine(LaunchDraft());  //Lance le draft d'après No Brakes car enemy tué
        }*/
    }

    /**
     * Ouvre le draft si un ennemi est buté avec le no brakes
     */
    IEnumerator LaunchDraft()
    {
        GameManager.Instance.DraftPanelActivation(true);

        while(GameManager.Instance.draftPanelActivated)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        playerHasMoved = true;  //Le joueur a terminé l'effet de la carte move
    }

    /**
     * Renvoie true lorsque le joueur est en mouvement
     */
    public bool IsPlayerMoving()
    {
        return isMoving;
    }

    /**
    * Renvoie true si le mouvement du player est terminé
    */
    public bool PlayerMoveDone()
    {
        if (!playerHasMoved) return false;

        foreach(ParticleSystemScript particule in wheelParticules)
        {
            particule.stopParticle();
        }

        playerHasMoved = false;
        return true;
    }

    /**
     * Affiche les nouvelles tiles accessibles lorsqu'il reste des move à utiliser de la carte "mouvement" jouée
     */
    private void ShowNewAccessible()
    {
        if(!accessibleShown)
        {
            accessibleShown = true;
            TilesManager.Instance.UpdateAccessibleTilesList();
            TilesManager.Instance.ShowAccessibleTiles();            
        }
    }

    /**
     * Return player's world position
     */
    public Vector3 GetPlayerWorldPosition()
    {
        return transform.position;
    }

    /**
     * Return player's cell position
     */
    public Vector3Int GetPlayerCellPosition()
    {
        return TilesManager.Instance.walkableTilemap.WorldToCell(transform.position);
    }

    /**
     * Return player's room position
     */
    public Vector3Int GetPlayerRoomPosition()
    {
        return TilesManager.Instance.roomTilemap.WorldToCell(transform.position);
    }

    /**
     * Attaque les ennemis dans la room avec les damages de la carte jouée
     */
    public bool RoomAttack()
    {
        bool enemySlayed = false;

        List<EnemyView> closeEnemiesList = EnemyManager.Instance.GetEnemiesInPlayersRoom(); //On récup la list des ennemis présents dans la nouvelle room du player
        foreach(EnemyView enemy in closeEnemiesList)    //Pour chaque ennemi de cette room
        {
            enemy.OnDamage();   //On lance l'effet visuel du hit sur les ennemis
            enemy.enemyData.TakeDamage(_cardEffectDatas[0].damagePoint, true);    //On applique les damages de la carte sur les ennemis
            if (enemy.enemyData.killedByPlayer)
                enemySlayed = true;
        }

        return enemySlayed;
    }

    /**
     * Inflige des damages au player
     */
    public void TakeDamage(int damage)
    {
        //considérer le shield du player
        PlayerData.Instance.TakeDamage(damage, false);

        UpdateHealthText();
        //UpdateBlockText();
        //Debug.Log("Update player's life ");

        //launch player hit effect
        OnDamage();
    }

    /**
     * Inflige des dégat à l'ennemie lorsque le player est attaqué
     */
    public int Counter()
    {
        return BuffManager.Instance.counterDamage;
    }

    /**
     * Add block defense to player
     */
    public void AddBlock(int blockValue)
    {
        PlayerData.Instance.AddBlock(blockValue);
        //UpdateBlockText();
        UpdateHealthBar(PlayerData.Instance.health + PlayerData.Instance.defense);
        UpdateHealthText();
    }

    /**
     * Add Gold to player
     */
    public void AddGold(int goldValue)
    {
        PlayerData.Instance.AddGold(goldValue);

        UpdateGoldText();
    }

    /**
     * Add Gold to player
     */
    public void AddToken()
    {
        PlayerData.Instance.AddToken();

        UpdateTokenText();
    }

    /**
     * Remove 1 token to player
     */
    public void TakeOffToken()
    {
        PlayerData.Instance.TakeOffOneToken();

        UpdateTokenText();
    }

    /**
     * Update Health Text
     */
    public void UpdateHealthBar(int currentHealthPoints)
    {
        healthBar.GetComponent<PlayerHealthBar>().UpdateHealthBar(currentHealthPoints);
    }

    /**
     * Set la valeur max pour la health bar
     */
    public void SetHealthMax(int value)
    {
        healthBar.GetComponent<PlayerHealthBar>().SetMaxHealth(value);
    }

    /**
     * Update Health Text
     */
    private void UpdateHealthText()
    {
        healthText.text = PlayerData.Instance.health.ToString();
    }

    /**
     * Update Block Text
     */
    private void UpdateBlockText()
    {
        blockText.text = PlayerData.Instance.defense.ToString();
    }

    /**
     * Update Gold Text
     */
    public void UpdateGoldText()
    {
        goldValueText.text = PlayerData.Instance.goldValue.ToString();
    }

    /**
     * Update Token Text
     */
    private void UpdateTokenText()
    {
        tokenText.text = PlayerData.Instance.token.ToString();
    }

    /**
     * annule tous les bonus de stats du player
     * annule aussi les effets visuel de ces bonus
     */
    public void ResetBonus()
    {
        PlayerData.Instance.ResetStat();
        StopVisualEffect();
    }

    /**
     * Actualise le sprite du player en fonction de sa direction
     */
    public void UpdatePlayerSprite(string direction)
    {
        switch(direction)
        {
            case "haut_gauche":
                spriteRenderer.sprite = spriteList[0];
                break;
            case "haut_droite":
                spriteRenderer.sprite = spriteList[1];
                break;
            case "bas_gauche":
                spriteRenderer.sprite = spriteList[2];
                break;
            case "bas_droite":
                spriteRenderer.sprite = spriteList[3];
                break;
        }
    }


    #region Visual effect

    //launch player hit effect
    public void OnDamage()
    {
        playerHit.launchParticle();

        //SFX player hit
        MusicManager.Instance.PlayPlayerHit();
    }

    //launch player Shield effect
    public void OnShield()
    {
        playerShield.launchParticle();
    }

    //launch player PowerUp effect
    public void OnPowerUp()
    {
        playerPowerUp.launchParticle();
    }

    public void OnKillerInstinct()
    {
        playerKillerInstinct.launchParticle();
    }

    public void OnPlayerCounter()
    {
        playerCounter.launchParticle();
    }

    public void OnPlayerFastShoes()
    {
        playerFastShoes.launchParticle();
    }

    public void StopVisualEffect()
    {
        playerKillerInstinct.stopParticle();
        playerFastShoes.stopParticle();
        playerCounter.stopParticle();
    }
    #endregion
}
