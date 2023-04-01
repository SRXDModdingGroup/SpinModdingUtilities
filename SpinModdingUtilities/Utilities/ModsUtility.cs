using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace SMU.Utilities; 

/// <summary>
/// Utility class for accessing mod files
/// </summary>
public static class ModsUtility {
    private static Dictionary<string, AssetBundle> assetBundles = new();

    /// <summary>
    /// Unloads all asset bundles for a given mod
    /// </summary>
    /// <param name="modName">The name of the mod which contains the asset bundles</param>
    public static void UnloadAssetBundles(string modName) {
        foreach (string path in Directory.GetFiles(Path.Combine(Paths.PluginPath, modName, "assetBundles"))) {
            if (!assetBundles.TryGetValue(path, out var bundle))
                continue;
            
            bundle.Unload(true);
            assetBundles.Remove(path);
        }
    }

    /// <summary>
    /// Unloads an asset bundle with the given name for a given mod
    /// </summary>
    /// <param name="modName">The name of the mod which contains the asset bundle</param>
    /// <param name="bundleName">The name of the asset bundle</param>
    public static void UnloadAssetBundle(string modName, string bundleName) {
        if (!TryGetBundlePath(modName, bundleName, out string path) || !assetBundles.TryGetValue(path, out var bundle))
            return;
            
        bundle.Unload(true);
        assetBundles.Remove(path);
    }

    /// <summary>
    /// Attempts to get an asset bundle with the given name for a given mod
    /// </summary>
    /// <param name="modName">The name of the mod which contains the asset bundle</param>
    /// <param name="bundleName">The name of the asset bundle</param>
    /// <param name="bundle">The loaded asset bundle</param>
    /// <returns>True if the asset bundle was found</returns>
    public static bool TryGetAssetBundle(string modName, string bundleName, out AssetBundle bundle) {
        if (!TryGetBundlePath(modName, bundleName, out string path)) {
            bundle = null;
            
            return false;
        }
            
        if (assetBundles.TryGetValue(path, out bundle))
            return true;

        bundle = AssetBundle.LoadFromFile(path);
        assetBundles.Add(path, bundle);

        return true;
    }

    /// <summary>
    /// Gets the directory path for the mod with a given name
    /// </summary>
    /// <param name="modName">The name of the mod</param>
    /// <returns>The full path to the mod directory</returns>
    public static string GetModDirectory(string modName) => Path.Combine(Paths.PluginPath, modName);

    /// <summary>
    /// Gets the path for a file from a mod with a given name
    /// </summary>
    /// <param name="modName">The name of the mod</param>
    /// <param name="paths">An array of parts of the file path</param>
    /// <returns>The full path to the file</returns>
    public static string GetModFile(string modName, params string[] paths) => Path.Combine(Paths.PluginPath, modName, Path.Combine(paths));

    /// <summary>
    /// Loads all asset bundles for a given mod
    /// </summary>
    /// <param name="modName">The name of the mod which contains the asset bundles</param>
    public static Dictionary<string, AssetBundle> LoadAssetBundles(string modName) {
        var foundBundles = new Dictionary<string, AssetBundle>();
        
        foreach (string path in Directory.GetFiles(Path.Combine(Paths.PluginPath, modName, "assetBundles"))) {
            if (Path.HasExtension(path))
                continue;

            if (!assetBundles.TryGetValue(path, out var bundle)) {
                bundle = AssetBundle.LoadFromFile(path);

                if (bundle == null)
                    continue;

                assetBundles.Add(path, bundle);
            }
            
            foundBundles.Add(Path.GetFileName(path), bundle);
        }

        return foundBundles;
    }

    private static bool TryGetBundlePath(string modName, string name, out string path) {
        path = Path.Combine(Paths.PluginPath, modName, "assetBundles", name);

        return !Path.HasExtension(path) && File.Exists(path);
    }
}