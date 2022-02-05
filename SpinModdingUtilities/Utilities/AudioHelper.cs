using System.IO;
using UnityEngine;

namespace SMU.Utilities
{
    /// <summary>
    /// Utility class for loading AudioClip objects.
    /// </summary>
    public static class AudioHelper
    {
        /// <summary>
        /// Load an audio file, given a file path.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <returns>An AudioClip, or null if the file cannot be found/loaded.</returns>
        public static AudioClip LoadClipFromFile(string path)
        {
#pragma warning disable 0618
            using (WWW www = new WWW(BepInEx.Utility.ConvertToWWWFormat(path)))
#pragma warning restore 0618
            {
                if (!File.Exists(path)) return null;
                try
                {
                    AudioClip clip = www.GetAudioClip();
                    while (clip.loadState != AudioDataLoadState.Loaded) { }
                    return clip;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
