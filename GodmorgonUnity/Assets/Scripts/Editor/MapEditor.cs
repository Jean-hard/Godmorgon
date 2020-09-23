using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

public class MapEditor : EditorWindow
{
    private MapGenerator _mapGenerator = null;

    private int _tempMapSizeX = 0;
    private int _tempMapSizeY = 0;

    private bool showMapSettings = false;

    //Ajoute le RoomEffectEditor au menu Window
    [MenuItem("Window/Map Editor")]
    static void Init()
    {
        //Récupère la window existante, et si elle n'existe pas, on en créer une nouvelle
        MapEditor window = (MapEditor)EditorWindow.GetWindow(typeof(MapEditor));
        window.Show();
    }

    void OnGUI()
    {
        if (null == _mapGenerator)
        {
            _mapGenerator = GetMapInScene();
        }

        if (null == _mapGenerator)
        {
            //No Map in Scene
            GUILayout.Label("Create a MapGenerator on scene to continue.", EditorStyles.boldLabel);
            return;
        }

        showMapSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showMapSettings, "Modify map size");

        if (showMapSettings)
        {
            Vector2Int mapSize = EditorGUILayout.Vector2IntField("Enter values", new Vector2Int(_tempMapSizeX, _tempMapSizeY));

            if (mapSize.x != _tempMapSizeX || mapSize.y != _tempMapSizeY)
            {
                _tempMapSizeX = mapSize.x;
                _tempMapSizeY = mapSize.y;
            }

            if (GUILayout.Button("Update map size")) //Si on clique sur le bouton generate
            {
                if (_mapGenerator.map.mapSize.x != mapSize.x || _mapGenerator.map.mapSize.y != mapSize.y)
                {
                    _mapGenerator.map.mapSize.x = mapSize.x;
                    _mapGenerator.map.mapSize.y = mapSize.y;

                    _tempMapSizeX = _mapGenerator.map.mapSize.x;
                    _tempMapSizeY = _mapGenerator.map.mapSize.y;

                    _mapGenerator.GenerateMap(); //On affiche le nouveau tableau sur la room tilemap
                }
            }
        }
        
    }

    public MapGenerator GetMapInScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] rootGameObjects = scene.GetRootGameObjects();

        foreach (GameObject rootGameObject in rootGameObjects)
        {
            MapGenerator map = rootGameObject.GetComponentInChildren<MapGenerator>();
            if (null != map)
            {
                return map;
            }
        }

        return null;
    }
}

#endif
