using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GGS.CakeBox.Utils
{
    public static class UnityEventHook
    {
        private sealed class UnityHook : MonoBehaviour
        {
            private void OnApplicationPause(bool paused)
            {
                if (paused)
                    OnPause();
                else
                    OnResume();
            }

            private void OnApplicationQuit()
            {
                OnQuit();
            }

            private void OnApplicationFocus(bool focus)
            {
                if (focus)
                    OnFocus();
                else
                    OnUnFocus();
            }
        }

        //
        // Static 
        //

        private static readonly UnityHook unity;
        private static float pausedTimeStamp;

        //
        // Events
        // 

        /// <summary>Fired when the application gets paused like in background mode. Paramter is returning the current realtime since startup.</summary>
        public static event Action<float> Paused;

        /// <summary>Fired when the application resumed after it was paused. Paramter is returning the calcualted paused duration.</summary>
        public static event Action<float> Resumed;

        /// <summary>Fired if the application gets back the focus. For eaxmaple after closing an onscreen keyboard.</summary>
        public static event Action Focused;

        /// <summary>Fired if the application lost the focus. This happens for example when an onscreen keyboard is shown.</summary>
        public static event Action LostFocus;

        /// <summary>This event is fired when the application is quitting.</summary>
        public static event Action Quitting;

        static UnityEventHook()
        {
            unity = new GameObject("UnityEventHook").AddComponent<UnityHook>();
            Object.DontDestroyOnLoad(unity.gameObject);
        }

        private static void OnPause()
        {
            pausedTimeStamp = Time.realtimeSinceStartup;
            if (Paused != null)
                Paused(pausedTimeStamp);
        }

        private static void OnResume()
        {
            var elapsedTime = Time.realtimeSinceStartup - pausedTimeStamp;
            if (Resumed != null)
                Resumed(elapsedTime);
        }

        private static void OnQuit()
        {
            if (Quitting != null)
                Quitting();
        }

        private static void OnFocus()
        {
            if (Focused != null)
                Focused();
        }

        private static void OnUnFocus()
        {
            if (LostFocus != null)
                LostFocus();
        }

        public static Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return unity.StartCoroutine(coroutine);
        }

        public static void StopCoroutine(Coroutine coroutine)
        {
            unity.StopCoroutine(coroutine);
        }

        public static void StopCoroutine(IEnumerator coroutine)
        {
            unity.StopCoroutine(coroutine);
        }

        public static void StopAllCoroutines()
        {
            unity.StopAllCoroutines();
        }
    }
}