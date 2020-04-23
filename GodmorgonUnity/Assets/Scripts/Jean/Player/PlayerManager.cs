using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

using GodMorgon.StateMachine;
using GodMorgon.Player;
using GodMorgon.Enemy;

public class PlayerManager : MonoBehaviour
{
    [Header("General Settings")]
    public Text healthText;
    public Text blockText;

    [Header("Movement Settings")]
    public float playerSpeed = 1f;
    public AnimationCurve playerMoveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    private bool playerCanMove = false;
    private bool playerHasMoved = false;
    private int spotIndex;
    private List<Spot> playerPathArray;

    Vector3Int endPos;

    // nombre de tiles parcourues pour 1 move
    private int nbTilesToMove = 3;

    [System.NonSerialized]
    public bool isFirstInRoom = true;

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
        UpdateBlockText();
    }

    // Update is called once per frame
    void Update()
    {
        LaunchMoveMechanic();
    }

    /**
     * Détermine la liste de tiles (=path) à parcourir jusqu'à l'endroit où la carte mouvement est déposée
     */
    public void MovePlayer(int nbMoves)
    {
        //Transpose la position de la souris au moment du drop de carte en position sur la grid, ce qui donne donc la tile sur laquelle on a droppé la carte
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
        endPos = TilesManager.Instance.walkableTilemap.WorldToCell(mouseWorldPos);

        List<Spot> _roadPath = TilesManager.Instance.roadPath;

        Vector3 playerPos = this.transform.position;
        Vector3Int playerCellPos = TilesManager.Instance.walkableTilemap.WorldToCell(playerPos);  //Position du player en format cell

        playerPathArray = new List<Spot>();

        spotIndex = 0;

        if (_roadPath != null && _roadPath.Count > 0) //reset le roadpath
            _roadPath.Clear();

        //création du path, prenant en compte la position des tiles, le point de départ, le point d'arrivée, et la longueur en tiles du path
        //roadPath est une liste de spots = une liste de positions de tiles
        _roadPath = TilesManager.Instance.astar.CreatePath(TilesManager.Instance.spots, new Vector2Int(playerCellPos.x, playerCellPos.y), new Vector2Int(endPos.x, endPos.y), nbTilesToMove); //* nbMoves

        bool isEnemyOnPath = false;

        foreach (Spot tile in _roadPath)
        {
            bool isEnemyOnTile = false;

            //on ajoute les tiles par lesquelles il va devoir passer sauf celle où il y a un enemy
            if (null != EnemyManager.Instance.GetEnemyViewByPosition(new Vector3Int(tile.X, tile.Y, 0)))
            {
                EnemyManager.Instance.GetEnemyViewByPosition(new Vector3Int(tile.X, tile.Y, 0)).enemyData.inPlayersRoom = true;
                isEnemyOnTile = true;
                isEnemyOnPath = true;
                isFirstInRoom = false;
            }

            if(!isEnemyOnTile)
                playerPathArray.Add(tile);     //on ajoute les spots par lesquels le player va devoir passer sauf celui où il est

            spotIndex++;
        }

        //Si on ETAIT le premier arrivé dans la room, alors un ennemi présent dans la room doit se recentrer au milieu de la room
        if (isFirstInRoom)
            EnemyManager.Instance.RecenterEnemiesAfterPlayerMove();

        //Si on n'a pas d'ennemi sur le chemin, on est le premier arrivé dans la room
        if (!isEnemyOnPath)
        {
            isFirstInRoom = true;
            foreach (EnemyView enemy in EnemyManager.Instance.GetEnemiesInPlayersRoom())
            {
                enemy.enemyData.inPlayersRoom = false;
            }
        }

        playerPathArray.Reverse(); //on inverse la liste pour la parcourir de la tile la plus proche à la plus éloignée
        playerPathArray.RemoveAt(0);

        /*    
        foreach (Spot spot in playerPathArray)
        {
            Debug.Log(spot.X + " " + spot.Y);
        }*/

        playerCanMove = true;  //on autorise le player à bouger

        spotIndex = 0;    
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

        //la prochaine position est le spot parmi la liste de spots
        Vector3 nextPos = TilesManager.Instance.walkableTilemap.CellToWorld(new Vector3Int(playerPathArray[spotIndex].X, playerPathArray[spotIndex].Y, 0))
            + new Vector3(0, 0.3f, 0);   //on ajoute 0.x pour que le player passe bien au milieu de la tile, la position de la tile étant en bas du losange             


        if (Vector3.Distance(this.transform.position, nextPos) < 0.001f)
        {
            //si on arrive à la tile finale où le player peut se rendre
            if (spotIndex == playerPathArray.Count - 1)
            {

                playerCanMove = false;
                playerHasMoved = true;
                spotIndex = 0;
                StartCoroutine(WaitForRingMasterTurn());
            }
            else if (spotIndex < playerPathArray.Count - 1)
            {
                spotIndex++;    //on passe à la tile suivante tant qu'on a pas atteint la dernière
            }
        }
        else
        {
            float ratio = (float)spotIndex / (playerPathArray.Count - 1);   //ratio varie entre 0 et 1, 0 pour le spot le plus proche et 1 pour le spot final
            ratio = playerMoveCurve.Evaluate(ratio);     //on le lie à notre curve pour le modifier dans l'inspector à notre guise
            float speed = playerSpeed * ratio;   //on le lie à la vitesse pour que la curve ait un impact sur la vitesse du player
            this.transform.position = Vector2.MoveTowards(this.transform.position, nextPos, speed * Time.deltaTime);
        }
    }

    /**
    * Renvoie true si le mouvement des ennemis est terminé
    */
    public bool PlayerMoveDone()
    {
        if (playerHasMoved)
            return true;
        else
            return false;
    }

    /**
     * Return player's cell position
     */
    public Vector3Int GetPlayerPosition()
    {
        return TilesManager.Instance.walkableTilemap.WorldToCell(transform.position);
    }

    // WIP
    IEnumerator WaitForRingMasterTurn()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Ringmaster turn");
        //GameEngine.Instance.SetState(StateMachine.STATE.RINGMASTER_TURN);
        playerHasMoved = false;
    }

    /**
     * Inflige des damages au player
     */
    public void TakeDamage(int damage)
    {
        //considérer le shield du player
        PlayerData.Instance.TakeDamage(damage);

        UpdateHealthText();
        UpdateBlockText();
        Debug.Log("Update player's life ");
    }

    /**
     * Add black defense to player
     */
    public void AddBlock(int blockValue)
    {
        PlayerData.Instance.AddBlock(blockValue);
        UpdateBlockText();
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
}
