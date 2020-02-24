using System.IO;
using UnityEditor;
using UnityEngine;

namespace SeriousGaming.Tools.CI.Editor
{

    /**
     * Tools to provide PreProcess action in Continuous Integration Environment.
     * 
     * CI has to go in a clear preformatted environment and some actions must be performed
     * to get the whole shebang set up.
     * Those methods can be called from anywhere they are required, ie. batch command or 
     * internal scripts and wizards to get the shit done.
     */
    public static class QualityPrepareCommand
    {
        /**
         * Produces the Windows Visual Studio solution and project files.
         * 
         * This action is required before to start a Quality Insurance process using
         * SonarQube quality tools. Those are going to compile the project to identify
         * code smells and failures.
         * GIT does not store SLN and CSPROJ files are they are system dependant and
         * may vary from dev environment to another.
         * It should be called on a fresh GIT pull to create solution files and let
         * Sonar do its job.
         */
        public static void PrepareSonarFiles ()
        {
            Debug.Log("### QualityPrepareCommand:PrepareSonarFiles - Started...");
            // We actually ask Unity to create the CSPROJ and SLN files.
            bool success = EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
            Debug.Log("### QualityPrepareCommand:PrepareSonarFiles - " + (success ? "Done" : "FAILED") + ".");

            // Check solution ONE has performed
            bool success1 = success && File.Exists(GetSolutionFilename());
            Debug.Log("### QualityPrepareCommand:PrepareSonarFiles - " + (success1 ? "OK" : "FAILED"));
            // Unsupported Version
            /*Debug.Log("### QualityPrepareCommand:PrepareSonarFiles - Started V2...");
            System.Type T = System.Type.GetType("UnityEditor.SyncVS,UnityEditor");
            System.Reflection.MethodInfo SyncSolution = T.GetMethod("SyncSolution", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            SyncSolution.Invoke(null, null);
            Debug.Log("### QualityPrepareCommand:PrepareSonarFiles - Ended V2...");*/
            // ---

            EditorApplication.Exit(success1 ? 0 : 1);
        }

        /**
         * Retrieves Application path and name.
         */
        private static string GetSolutionFilename ()
        {
            string dataPath = Application.dataPath;
            // Solution one:
            // <code>
            // string[] path = dataPath.Split('/');
            // string AppName = path[path.Length - 2];
            // </code>
            string solutionPath = dataPath + "/../" + Application.productName + ".sln";
            Debug.Log("### QualityPrepareCommand:GetSolutionFilename - Looking for " + solutionPath);
            return solutionPath;
        }
    }

}