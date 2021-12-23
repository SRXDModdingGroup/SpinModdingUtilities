# Spin Modding Utilities (SMU) (Previously Slime Modding Utilities)
A utility mod originally for Nickelodeon All Star Brawl, forked for Spin Rhythm XD. This mod does nothing on its own, but has a bunch of nifty features that modders can make use of.

-----

### SMU.Events

Contains handy extension methods for safely firing off events, without the worry of an exception stopping the chain.

```cs
public static void InvokeAll(this Action action, params object[] args)
```

### SMU.Constants

Contains methods to translate character and stage IDs into their human-readable names, and vice versa.

```cs
public static bool TryGetIdFromName(string name, out string id);
public static string GetIdFromName(string name);
public static bool TryGetNameFromId(string id, out string name);
public static string GetNameFromId(string id);
public static List<string> GetCharacterIds();
public static List<string> GetStageIds();
```

### SMU.Reflection

Contains handy extension methods to help get and set private fields and properties, invoke private methods, and copy Unity components.

```cs
public static U GetField<T, U>(this T obj, string fieldName);
public static U GetProperty<T, U>(this T obj, string propertyName);
public static void SetField<T, U>(this T obj, string fieldName, U value);
public static void SetProperty<T, U>(this T obj, string propertyName, U value);
public static T InvokeMethod<T>(this object obj, string methodName, params object[] args);
public static Component CopyComponent(this Component original, Type overridingType,
                                      GameObject destination, Type originalTypeOverride = null);
```

### SMU.Utilities

Contains extra utility classes with miscellaneous functionality

- ImageHelper
  - Utility class for loading external images from various locations; creates Texture2D and Sprite objects
  ```cs
  public static Texture2D LoadTextureRaw(byte[] bytes);
  public static Texture2D LoadTextureFromFile(string path);
  public static Texture2D LoadTextureFromResources(string resourcePath);
  public static Sprite LoadSpriteRaw(byte[] image, float pixelsPerUnit = 100.0f);
  public static Sprite LoadSpriteFromTexture(Texture2D texture, float pixelsPerUnit = 100.0f);
  public static Sprite LoadSpriteFromFile(string path, float pixelsPerUnit = 100.0f);
  public static Sprite LoadSpriteFromResources(string resourcePath, float pixelsPerUnit = 100.0f);
  ```
- SharedCoroutineStarter
  - Provides a way to start coroutines from any class, without having to derive from MonoBehaviour 
  ```cs
  public static new Coroutine StartCoroutine(IEnumerator routine);
  ```
