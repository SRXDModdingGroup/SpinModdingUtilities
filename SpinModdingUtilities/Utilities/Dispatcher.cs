using System;
using System.Collections;
using UnityEngine;

namespace SMU.Utilities {
    /// <summary>
    /// Utility class for running coroutines from non-Monobehaviours and queueing actions to execute on the next frame.
    /// </summary>
    public class Dispatcher : MonoBehaviour {
        private static Dispatcher instance;
        private static Dispatcher Instance {
            get {
                if (instance == null)
                    instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();

                return instance;
            }
        }

        private Action nextFrame;

        private void Update() {
            if (nextFrame == null)
                return;
            
            nextFrame.Invoke();
            nextFrame = null;
        }

        /// <summary>
        /// Queues an action to be executed on the next available frame
        /// </summary>
        /// <param name="action">The action to queue</param>
        public static void QueueForNextFrame(Action action) => Instance.nextFrame += action;

        /// <summary>
        /// Starts a coroutine, without the need for the calling class to inherit from Monobehaviour.
        /// </summary>
        /// <param name="routine">The Coroutine to start.</param>
        /// <returns>The started Coroutine.</returns>
        public new static Coroutine StartCoroutine(IEnumerator routine) => ((MonoBehaviour)Instance).StartCoroutine(routine);
    }
}
