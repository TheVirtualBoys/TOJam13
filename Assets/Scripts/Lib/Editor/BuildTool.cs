using UnityEditor;
using UnityEngine;

public class BuildTool {

	#region Build Tools

	/// <summary>
	/// Called when Unity is building the project. actually creates the mobile build.
	/// </summary>
	[MenuItem("XMG/Build/CreateBuild")]
	public static void CreateBuild() {
		string outDir = "_XCODEDIR_";
		BuildOptions buildOptions = BuildOptions.None;

		// Call the PreBuild function on the build utility class.
		// Each project must implement a static class with the name BuildUtility and the function PreBuild.
		BuildUtility.PreBuild();
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		for (int i = 0; i < scenes.Length; ++i) {
			// TODO: Remove any debug scenes if this is a release build.
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}

		// We need IL2CPP turned on for 64 bit compatability if we're building for iPhone.
		if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS ||
		    EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ||
			EditorUserBuildSettings.activeBuildTarget == BuildTarget.tvOS) {
			buildOptions |= BuildOptions.Il2CPP;
		}

		EditorUserBuildSettings.SetBuildLocation(EditorUserBuildSettings.activeBuildTarget, outDir);
		BuildPipeline.BuildPlayer(scenes, outDir, EditorUserBuildSettings.activeBuildTarget, buildOptions);
	}

	#endregion

}
