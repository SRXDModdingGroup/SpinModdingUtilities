using System.Collections.Generic;
using System.IO;
using BepInEx;
using UnityEngine;

namespace SMU {
    /// <summary>
    /// Utility for loading AssetBundles
    /// </summary>
    public static class AssetBundleUtility {
        private static string bundlesPath = Path.Combine(Paths.BepInExRootPath, "assetBundles");
        private static Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();

        /// <summary>
        /// Unloads an asset bundle with the given name in the BepInEx\assetBundles folder
        /// </summary>
        /// <param name="name">The name of the asset bundle</param>
        /// <remarks>Asset bundle must reside in a folder named *name*_bundle</remarks>
        public static void UnloadAssetBundle(string name) => UnloadAssetBundle(bundlesPath, name);

        /// <summary>
        /// Unloads an asset bundle with the given name in a given folder
        /// </summary>
        /// <param name="directory">The folder which contains the asset bundle's folder</param>
        /// <param name="name">The name of the asset bundle</param>
        /// <remarks>Asset bundle must reside in a folder named *name*_bundle</remarks>
        public static void UnloadAssetBundle(string directory, string name) {
            if (!TryGetBundlePath(directory, name, out string path) || !assetBundles.TryGetValue(path, out var bundle))
                return;
            
            bundle.Unload(true);
            assetBundles.Remove(path);
        }

        /// <summary>
        /// Attempts to get an asset bundle with the given name in the BepInEx\assetBundles folder
        /// </summary>
        /// <param name="name">The name of the asset bundle</param>
        /// <param name="bundle">The loaded asset bundle</param>
        /// <returns>True if the asset bundle was found</returns>
        /// <remarks>Asset bundle must reside in a folder named *name*_bundle</remarks>
        public static bool TryGetAssetBundle(string name, out AssetBundle bundle) => TryGetAssetBundle(bundlesPath, name, out bundle);
        
        /// <summary>
        /// Attempts to get an asset bundle with the given name in a given folder
        /// </summary>
        /// <param name="directory">The folder which contains the asset bundle's folder</param>
        /// <param name="name">The name of the asset bundle</param>
        /// <param name="bundle">The loaded asset bundle</param>
        /// <returns>True if the asset bundle was found</returns>
        /// <remarks>Asset bundle must reside in a folder named *name*_bundle</remarks>
        public static bool TryGetAssetBundle(string directory, string name, out AssetBundle bundle) {
            if (assetBundles.TryGetValue(name, out bundle))
                return true;

            if (!TryGetBundlePath(directory, name, out string path))
                return false;

            bundle = AssetBundle.LoadFromFile(path);
            assetBundles.Add(name, bundle);

            return true;
        }

        private static bool TryGetBundlePath(string directory, string name, out string path) {
            path = Path.Combine(directory, name);

            return File.Exists(path);
        }
    }
}