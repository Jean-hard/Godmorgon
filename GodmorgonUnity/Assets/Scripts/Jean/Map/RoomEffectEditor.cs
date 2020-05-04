﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.SceneManagement;

public class RoomEffectEditor : EditorWindow
{
    #region Exemples
    //Exemple
    /*
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    */
    #endregion

    public RoomEffect effect = RoomEffect.EMPTY;
    public RoomEffect selectedEffect;

    private RoomEffectManager _roomEffectManager = null;

    // The variable to control where the scrollview 'looks' into its child elements.
    Vector2 scrollPosition;

    private static GUIStyle _selectedButton = null;
    private static GUIStyle selectedButton
    {
        get
        {
            if (_selectedButton == null)
            {
                _selectedButton = new GUIStyle(GUI.skin.button);    //Nouveau skin de button
                _selectedButton.normal.background = _selectedButton.active.background;  //On active le background de button appuyé
                _selectedButton.normal.textColor = _selectedButton.active.textColor;    //On active la couleur de texte de button appuyé
            }
            return _selectedButton; //On applique le nouveau skin de button appuyé
        }
    }

    //Ajoute le RoomEffectEditor au menu Window
    [MenuItem("Window/Room Effect Editor")]
    static void Init()
    {
        //Récupère la window existante, et si elle n'existe pas, on en créé une nouvelle
        RoomEffectEditor window = (RoomEffectEditor)EditorWindow.GetWindow(typeof(RoomEffectEditor));
        window.Show();
    }

    //Fonction d'affichage sur la window
    void OnGUI()
    {
        if (null == _roomEffectManager)
        {
            _roomEffectManager = GetRoomEffectManagerInScene();
        }

        if (null == _roomEffectManager)
        {
            //No Room Effect Manager in Scene
            GUILayout.Label("Create a room effect manager on scene to continue.", EditorStyles.boldLabel);
            return;
        }

        GUILayout.Label("Room Grid", EditorStyles.boldLabel);

        int newSizeX = EditorGUILayout.IntField("Size X", _roomEffectManager.sizeX);  //Champ int correspondant au nombre de room en largeur
        int newSizeY = EditorGUILayout.IntField("Size Y", _roomEffectManager.sizeY);  //Champ int correspondant au nombre de room en longueur
        if (_roomEffectManager.sizeX != newSizeX || _roomEffectManager.sizeY != newSizeY)
        {
            _roomEffectManager.sizeX = newSizeX;
            _roomEffectManager.sizeY = newSizeY;

            RoomData[] roomDataArr = _roomEffectManager.roomsDataArr;
            Array.Resize(ref roomDataArr, _roomEffectManager.sizeX * _roomEffectManager.sizeY);
            for (int i = 0; i < roomDataArr.Length; ++i)
            {
                if (roomDataArr[i] == null)
                    roomDataArr[i] = new RoomData();


                roomDataArr[i].x = i%newSizeX;
                roomDataArr[i].y = i / newSizeX;
            }
            _roomEffectManager.roomsDataArr = roomDataArr;
        }

        //Affiche en ligne un button par effet de room, button qu'on devra sélectionner pour set un effet à une room
        EditorGUILayout.BeginHorizontal();
        foreach (RoomEffect effect in Enum.GetValues(typeof(RoomEffect)))
        {
            //Button créé directement dans le if, et ce if gère le click du button
            if (GUILayout.Button(effect.ToString(), (effect == selectedEffect ? selectedButton : GUI.skin.button))) //Si on clique sur un button de choix d'effet
            {
                selectedEffect = effect;    //On set selectedEffect à l'effet du button cliqué
            }
        }
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        //Affiche le tableau de room avec une room = un button
        EditorGUILayout.BeginHorizontal(GUILayout.Width(_roomEffectManager.sizeX*50));
        for (int x = 0; x < _roomEffectManager.sizeX; x++)
        {
            EditorGUILayout.BeginVertical(GUILayout.Height(_roomEffectManager.sizeY*50));
            for (int y = 0; y < _roomEffectManager.sizeY; y++)
            {
                RoomData roomData = _GetRoomData(_roomEffectManager.roomsDataArr, x, y);
                
                if (GUILayout.Button(roomData.roomEffect.ToString(), GUILayout.Width(50), GUILayout.Height(50))) //Créé le button
                {
                    _roomEffectManager.SetRoomEffect(selectedEffect, new Vector2Int(x,y));
                    roomData.roomEffect = selectedEffect;
                    Debug.Log(roomData.roomEffect + " en " + x + "/" + y);
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();


        if(GUILayout.Button("Generate map view")) //Si on clique sur un button de choix d'effet
        {
            _roomEffectManager.GenerateRoomsView();    //On set selectedEffect à l'effet du button cliqué
        }

        #region Exemple
        //Exemples
        /*
        myString = EditorGUILayout.TextField("Text Field", myString);
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        
        EditorGUILayout.EndToggleGroup();
        */
        #endregion
    }

    private RoomData _GetRoomData(RoomData[] datasArr, int x, int y)
    {
        foreach (RoomData roomData in datasArr)
        {
            if (roomData.x == x && roomData.y == y)
            {
                return roomData;
            }
        }

        return null;
    }

    public RoomEffectManager GetRoomEffectManagerInScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] rootGameObjects = scene.GetRootGameObjects();

        foreach (GameObject rootGameObject in rootGameObjects)
        {
            RoomEffectManager roomEffectManager = rootGameObject.GetComponentInChildren<RoomEffectManager>();
            if (null != roomEffectManager)
            {
                return roomEffectManager;
            }
        }

        return null;
    }

}
