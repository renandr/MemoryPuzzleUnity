using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGS.ScreenManagement
{
    public abstract class AScreenController : MonoBehaviour
    {
        [SerializeField]
        private IScreen parentScreen;

        protected IScreen ParentScreen
        {
            get
            {
                return parentScreen;
            }
        }

        private void Awake()
        {
            parentScreen = GetComponent<IScreen>();
            Dictionary<string, Action<GameObject>> assets = CreationAssetList;
            if (assets != null && assets.Count > 0)
            {
                foreach (KeyValuePair<string, Action<GameObject>> ass in assets)
                {
                    parentScreen.AddCreationAsset(ass.Key, ass.Value);
                }
            }
            ScreenControllerAwake();
        }

        protected virtual void ScreenControllerAwake()
        {
            
        }

        public virtual Dictionary<string, Action<GameObject>> CreationAssetList
        {
            get
            {
                return null;
            }
        }

        protected void OnEnable()
        {
            parentScreen.ScreenIsCreated += OnScreenCreated;//probably not gonna work?
            parentScreen.ScreenOpened += OnScreenOpen;
            parentScreen.PropertiesFetched += OnScreenProperties;

            parentScreen.CloseAnimationFinished += OnCloseAnimationFinished;

            parentScreen.ScreenClosed += OnScreenClose;

            parentScreen.OpenAnimationFinished += OnOpenAnimationFinished;

            parentScreen.ScreenDestroyed += OnScreenDestroy;
        }

        protected void OnDisable()
        {
            parentScreen.ScreenIsCreated = null;
            parentScreen.ScreenOpened -= OnScreenOpen;
            parentScreen.PropertiesFetched -= OnScreenProperties;

            parentScreen.CloseAnimationFinished -= OnCloseAnimationFinished;

            parentScreen.ScreenClosed -= OnScreenClose;

            parentScreen.OpenAnimationFinished -= OnOpenAnimationFinished;

            parentScreen.ScreenDestroyed -= OnScreenDestroy;
        }

        protected virtual void OnScreenProperties()
        {
            
        }


        protected virtual void OnScreenCreated()
        {
        }

        protected virtual void OnScreenOpen()
        {
        }

        protected virtual void OnOpenAnimationFinished(IScreen t)
        {
        }

        protected virtual void OnCloseAnimationFinished(IScreen t)
        {
        }
        protected virtual void OnScreenClose()
        {
        }
        protected virtual void OnScreenDestroy()
        {
        }

    }
}