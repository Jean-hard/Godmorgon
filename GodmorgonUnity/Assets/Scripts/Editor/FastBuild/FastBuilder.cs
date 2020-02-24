/*
 */

using UnityEditor;
using UnityEngine;

namespace SeriousGaming.Tools.CI.Editor
{

    /**
     * FAST Builder is a script to create a simple BUILD interface.
     * 
     * The aim of the FastBuilder is to leverage the build process by preventing to
     * have a lot of operations to manually perform to get a consistent build:
     *  - select the correct scenes according to the game mode (hard/easy)
     *  - set the correct internal difficulty (PRAGMA)
     *  - SCORMify consistenly the stuff.
     */
    public class FastBuilder : ScriptableWizard
    {
        // App version for DISPLAY ONLY
        public string version = "n/a";

        [MenuItem("Tools/Fast Builder")]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<FastBuilder>("My Real Fast Builder", "Build");
        }

        void OnEnable()
        {
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            version = Application.version;
        }

        void OnWizardCreate()
        {
            FastBuilderCommand.LaunchBuild(true);
        }


    }
}