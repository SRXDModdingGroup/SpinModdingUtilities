using System;

namespace SMU.Events; 

/// <summary>
/// Utility class for handling events.
/// </summary>
public static class EventHelper
{
    /// <summary>
    /// Invoke an action, catching errors that occur along the way without causing the rest to fail.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    /// <param name="args">The arguments to be passed.</param>
    public static void InvokeAll<T1, T2, T3>(this Action<T1, T2, T3> action, params object[] args)
    {
        Delegate[] actions = action?.GetInvocationList();
        if (actions == null) return;
        foreach (Delegate invoc in actions)
        {
            string name = String.Empty;
            try
            {
                name = invoc?.Method.DeclaringType.Assembly.FullName;
                invoc?.DynamicInvoke(args);
            }
            catch (Exception ex)
            {
                Plugin.LogError($"Caught Exception when executing event: {ex.Message}\n In Assembly: {name}");
                Plugin.LogError(ex.StackTrace);
            }
        }
    }

    /// <summary>
    /// Invoke an action, catching errors that occur along the way without causing the rest to fail.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    /// <param name="args">The arguments to be passed.</param>
    public static void InvokeAll<T1, T2>(this Action<T1, T2> action, params object[] args)
    {
        Delegate[] actions = action?.GetInvocationList();
        if (actions == null) return;
        foreach (Delegate invoc in actions)
        {
            string name = String.Empty;
            try
            {
                name = invoc?.Method.DeclaringType.Assembly.FullName;
                invoc?.DynamicInvoke(args);
            }
            catch (Exception ex)
            {
                Plugin.LogError($"Caught Exception when executing event: {ex.Message}\n In Assembly: {name}");
                Plugin.LogError(ex.StackTrace);
            }
        }
    }

    /// <summary>
    /// Invoke an action, catching errors that occur along the way without causing the rest to fail.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    /// <param name="args">The arguments to be passed.</param>
    public static void InvokeAll<T>(this Action<T> action, params object[] args)
    {
        Delegate[] actions = action?.GetInvocationList();
        if (actions == null) return;
        foreach (Delegate invoc in actions)
        {
            string name = String.Empty;
            try
            {
                name = invoc?.Method.DeclaringType.Assembly.FullName;
                invoc?.DynamicInvoke(args);
            }
            catch (Exception ex)
            {
                Plugin.LogError($"Caught Exception when executing event: {ex.Message}\n In Assembly: {name}");
                Plugin.LogError(ex.StackTrace);
            }
        }
    }

    /// <summary>
    /// Invoke an action, catching errors that occur along the way without causing the rest to fail.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    /// <param name="args">The arguments to be passed.</param>
    public static void InvokeAll(this Action action, params object[] args)
    {
        Delegate[] actions = action?.GetInvocationList();
        if (actions == null) return;
        foreach (Delegate invoc in actions)
        {
            string name = String.Empty;
            try
            {
                name = invoc?.Method.DeclaringType.Assembly.FullName;
                invoc?.DynamicInvoke(args);
            }
            catch (Exception ex)
            {
                Plugin.LogError($"Caught Exception when executing event: {ex.Message}\n In Assembly: {name}");
                Plugin.LogError(ex.StackTrace);
            }
        }
    }
}