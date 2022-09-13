using System;
using System.Collections;
using UnityEngine;

namespace SMU.Utilities; 

/// <summary>
/// Utility class for generating hash numbers that do not vary in different program contexts. Use this to generate unique identifiers for objects
/// </summary>
public static class HashUtility {
    private const uint HASH_BIAS = 2166136261u;
    private const int HASH_COEFF = 486187739;

    /// <summary>
    /// Combines the hash numbers for multiple objects until a single hash
    /// </summary>
    /// <param name="objects">The objects whose hash numbers should be combined</param>
    /// <returns>A combined hash number</returns>
    public static int Combine(params object[] objects) {
        unchecked {
            int hash = (int) HASH_BIAS;

            foreach (object o in objects)
                hash = hash * HASH_COEFF ^ GetStableHash(o);

            return hash;
        }
    }

    /// <summary>
    /// Generates a hash number for a boolean
    /// </summary>
    /// <param name="b">The boolean to hash</param>
    /// <returns>The hash number</returns>
    public static int GetStableHash(bool b) => b ? 1 : 0;
    /// <summary>
    /// Generates a hash number for a character
    /// </summary>
    /// <param name="c">The character to hash</param>
    /// <returns>The hash number</returns>
    public static int GetStableHash(char c) => c | c << 16;
    /// <summary>
    /// Generates a hash number for a float
    /// </summary>
    /// <param name="f">The float to hash</param>
    /// <returns>The hash number</returns>
    public static unsafe int GetStableHash(float f) {
        if (f == 0.0)
            return 0;

        return *(int*) &f;
    }
    /// <summary>
    /// Generates a hash number for a string
    /// </summary>
    /// <param name="s">The string to hash</param>
    /// <returns>The hash number</returns>
    public static int GetStableHash(string s) {
        unchecked {
            int hash = (int) HASH_BIAS;

            foreach (char c in s)
                hash = hash * HASH_COEFF ^ GetStableHash(c);

            return hash;
        }
    }
    /// <summary>
    /// Generates a hash number for a color
    /// </summary>
    /// <param name="c">The color to hash</param>
    /// <returns>The hash number</returns>
    public static int GetStableHash(Color c) {
        unchecked {
            int hash = (int) HASH_BIAS;

            hash = hash * HASH_COEFF ^ GetStableHash(c.a);
            hash = hash * HASH_COEFF ^ GetStableHash(c.r);
            hash = hash * HASH_COEFF ^ GetStableHash(c.g);
            hash = hash * HASH_COEFF ^ GetStableHash(c.b);

            return hash;
        }
    }
    /// <summary>
    /// Generates a hash number for an IEnumerable
    /// </summary>
    /// <remarks>The contents of the enumerable must be of type bool, char, int, float, string, Color, IHashable, or IEnumerable</remarks>
    /// <param name="e">The enumerable to hash</param>
    /// <returns>The hash number</returns>
    public static int GetStableHash(IEnumerable e) {
        unchecked {
            int hash = (int) HASH_BIAS;

            foreach (object o in e)
                hash = hash * HASH_COEFF ^ GetStableHash(o);

            return hash;
        }
    }
    /// <summary>
    /// Generates a hash number for an object
    /// </summary>
    /// <remarks>The object must be of type bool, char, int, float, string, Color, IHashable, Array, or IEnumerable</remarks>
    /// <param name="o">The object to hash</param>
    /// <returns>The hash number</returns>
    public static int GetStableHash(object o) => o switch {
        bool b => GetStableHash(b),
        char c => GetStableHash(c),
        int i => i,
        float f => GetStableHash(f),
        string s => GetStableHash(s),
        Color c => GetStableHash(c),
        IHashable h => h.GetStableHash(),
        IEnumerable e => GetStableHash(e),
        _ => throw new ArgumentException("GetStableHash can only be called on objects of type bool, char, int, float, string, Color, IHashable, or IEnumerable")
    };
}