using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace SeriousGaming.Tools.CI.Editor
{

    public static class FastBuilderCommand
    {

        public static string buildPath = "D:/Builds/DS_FIC/CurrentBuild/";
        public static string outputFilePrefix = "FIC - ";

        const string WINDOWS_FOLDER = "Plateforme - Windows/";
        const string STANDALONE_EXTENSION = ".exe";
        

        /**
         * Allows the FastBuilder to be launched from Command line.
         * 
         * This method start the "heavy" build: all platforms with all difficulty levels:
         *  - platform / Windows
         *  - platform / SCORM
         *  - level / Easy
         *  - level / Expert
         */
        public static void LaunchFullBuild()
        {
            LaunchBuild(true);
        }

        /**
         * Prepares the conbtent and request a Build.
         */
        public static void LaunchBuild (bool buildWindows)
        {
            // Get current build target settings
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;

            // Gather all scenes
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            List<string> scenes = new List<string>();

            for (int i = 0; i < sceneCount; i++) {
                string scenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                scenes.Add(scenePath);
            }

            // -----------------------------------
            // Optimize plateform switching


            // Then we build it first
            BuildTargetSelection(buildTarget, scenes);
            
        }

        /**
         * Triggers a easy/hard mode BUILD for a given platform.
         */
        static void BuildTargetSelection(BuildTarget target, List<string> scenes)
        {
                BuildProject(scenes, target);
        }

        /**
         * Starts the BUILD with the given parameters.
         */
        static void BuildProject(List<string> generalScenes, BuildTarget target)
        {
            Debug.Log("BUILD >>> Building for " + target.ToString());

            // Update the Plateform
            BuildTargetGroup group = BuildTargetGroup.Standalone;
            EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);

            // Prepare file name and path
            string fileName = WINDOWS_FOLDER
                + outputFilePrefix
                + " - "
                + Application.version;

          
            // Merge scenes
            List<string> merged = new List<string>(generalScenes);

            // Convert the shit
            string[] _mergedScenes = merged.ToArray();

            // File Extention
            fileName += STANDALONE_EXTENSION;

            // Go !
            BuildReport report = BuildPipeline.BuildPlayer(_mergedScenes, buildPath + fileName, target, BuildOptions.None);
            Debug.Log("BUILD >>> Building : " + report.summary.result.ToString());
        }

    }

}