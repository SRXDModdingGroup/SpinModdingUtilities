using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMU.Utilities; 

/// <summary>
/// Utility class for running coroutines from non-Monobehaviours and queueing actions to execute on the next frame.
/// </summary>
public sealed class Dispatcher : MonoBehaviour {
    private static Dispatcher instance;
    private static Dispatcher Instance {
        get {
            CheckForInstance();

            return instance;
        }
    }

    private static List<Action> nextFrame = new();

    private void Update() {
        lock (nextFrame) {
            if (nextFrame.Count == 0)
                return;

            foreach (var action in nextFrame) {
                try {
                    action();
                }
                catch (Exception e) {
                    Plugin.LogError(e.Message);
                    Plugin.LogError(e.StackTrace);
                }
            }

            nextFrame.Clear();
        }
    }

    /// <summary>
    /// Queues an action to be executed on the next available frame
    /// </summary>
    /// <param name="action">The action to queue</param>
    public static void QueueForNextFrame(Action action) {
        CheckForInstance();
            
        lock (nextFrame)
            nextFrame.Add(action);
    }

    /// <summary>
    /// Starts a coroutine, without the need for the calling class to inherit from Monobehaviour.
    /// </summary>
    /// <param name="routine">The Coroutine to start.</param>
    /// <returns>The started Coroutine.</returns>
    public new static Coroutine StartCoroutine(IEnumerator routine) => ((MonoBehaviour)Instance).StartCoroutine(routine);

    private static void CheckForInstance() {
        if (instance == null)
            instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
    }
}