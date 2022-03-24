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

            string path = Path.Combine(directory, $"{name}_bundle\\{name}");

            if (!File.Exists(name))
                return false;
            
            bundle = AssetBundle.LoadFromFile(path);
            assetBundles.Add(name, bundle);

            return true;
        }
    }
}