#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    [MenuItem("ValhallaVerse/Build/Build Server (Linux)")]
    public static void BuildLinuxServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Offline.unity","Assets/Scenes/Lobby.unity" };
        buildPlayerOptions.locationPathName = "Builds/Linux/Server/Server.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;
        buildPlayerOptions.subtarget = (int)( StandaloneBuildSubtarget.Server);
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    [MenuItem("ValhallaVerse/Build/Build Server (OSX)")]
    public static void BuildOSXServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Offline.unity","Assets/Scenes/Lobby.unity" };
        buildPlayerOptions.locationPathName = "Builds/OSX/Server/Server.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneOSX;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;
        buildPlayerOptions.subtarget = (int)(StandaloneBuildSubtarget.Server);
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
#endif