using System;
using System.Collections.Generic;
using GGS.GameLocks;
using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// This is placed in and managed by layers directly.
    /// Never nest them. Use ScreenControllers for that.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup)), DisallowMultipleComponent]
    public abstract class AScreen<TProperties, TModel> : MonoBehaviour, IScreen //TODO screen registry system: ,ITutorialRegistrableView
        where TProperties : IScreenPropertiesVO
        where TModel : IScreenModelReader
    {
        [Header("AScreen")]
        [SerializeField]
        protected ScreenAnimation screenAnimation = new ScreenAnimation();

        [SerializeField]
        protected CachingBehavior cachingBehavior = CachingBehavior.ForceNotCaching;


        public TModel ModelReader { protected get; set; }

        public ScreenIdentifier ScreenId { get; set; }

        public int InstanceIdentifier { get; set; }

        protected TProperties PreviousProperties { get; private set; }

        protected TProperties CurrentProperties { get; private set; }

        public CachingBehavior CachingBehavior {
            get
            {
                return cachingBehavior;
            }
        }


        public GenericEvent ScreenIsCreated { get; set; }

        public event GenericEvent ScreenOpened;

        public event GenericEvent PropertiesFetched;

        public event GenericEvent<IScreen> CloseAnimationFinished;

        public event GenericEvent ScreenClosed;

        public event GenericEvent<IScreen> OpenAnimationFinished;

        public event GenericEvent ScreenDestroyed;


        public bool IsOpen { get; private set; }
        public bool IsCreated { get; private set; }

        /// <summary>
        /// Defines if the view is currently playing an open or close animation
        /// </summary>
        public bool IsAnimating { get; private set; }

        /// <summary>
        /// Is the view as uncacheable marked. This is only for views that have a
        /// special mediator binding and reusing it would beak something.
        /// </summary>
        public bool IsUncacheable { get; private set; }

        protected CanvasGroup canvasGroup;

        private readonly Dictionary<string, Action<GameObject>> CreationAssetList = new Dictionary<string, Action<GameObject>>();


        protected virtual void Awake()
        {
            screenAnimation.StartAnimationFinished += OnStartScreenAnimationFinished;
            screenAnimation.EndAnimationFinished += OnEndScreenAnimationFinished;
        }

        protected void Start()
        {
            ScreenStart();
            LoadCreationAssets();
        }

        public void AddCreationAsset(string assetName, Action<GameObject> callbackAction)
        {
            CreationAssetList.Add(assetName, callbackAction);

        }

        protected virtual void ScreenStart()
        {

        }

        /// <summary>
        /// Override this for loading assets differently
        /// </summary>
        protected virtual void LoadCreationAssets()
        {
            if (CreationAssetList.Count <=0)
            {
                ScreenAssetsLoaded();
            }
            else foreach (KeyValuePair<string, Action<GameObject>> ass in CreationAssetList)
            {
                LoadComponent(ass.Key, delegate (GameObject obj)
                {
                    CreationAssetList.Remove(ass.Key);
                    ass.Value(obj);
                    if (CreationAssetList.Count == 0)
                    {
                        ScreenAssetsLoaded();
                    }
                });
            }
        }

        protected void ScreenAssetsLoaded()
        {
            IsCreated = true;
            OnCreate();
            ScreenIsCreated.Fire();
        }

        /// <summary>
        /// The asset is created and its creation assets too
        /// </summary>
        protected virtual void OnCreate()
        {
        }

        public void NotifyOpen()
        {
            IsOpen = true;
            if (NeedsProperties && !IsDialog)
            {
                ScreenSystem.PanelContentUpdated+=OnContentUpdated;
            }
            OnOpen();
            CheckForProperties();
            ScreenOpened.Fire();
        }

        /// <summary>
        /// The screen is open, can be for the first time or not
        /// </summary>
        protected abstract void OnOpen();

        private void CheckForProperties()
        {
            if (NeedsProperties)
            {
                PreviousProperties = CurrentProperties;
                CurrentProperties = ModelReader.GetProperties<TProperties>(ScreenId, InstanceIdentifier);
                OnProperties();
                PropertiesFetched.Fire();

            }
        }

        public bool NeedsProperties
        {
            get
            {
                return typeof(TProperties) != typeof(NullScreenProperties);
            }
        }

        /// <summary>
        /// Happens right after OnOpen, and maybe afterwards via signals
        /// </summary>
        protected abstract void OnProperties();

        private void OnContentUpdated(ScreenIdentifier id)
        {
            if (ScreenId == id)
            {
                CheckForProperties();
            }
        }

        /// <summary>
        /// Plays the start animation. If there is none, the finished
        /// is directly triggered.
        /// </summary>
        public void PlayOpenAnimation()
        {
            IsAnimating = true;
            screenAnimation.PlayStartAnimation(this);
        }

        private void OnStartScreenAnimationFinished()
        {
            IsAnimating = false;
            OpenAnimationFinished.Fire(this);
        }


        public void NotifyClose()
        {
            IsOpen = false;
            ClosingView();
            if (NeedsProperties && !IsDialog)
            {
                ScreenSystem.PanelContentUpdated -= OnContentUpdated;
            }

            OnClose();
            ScreenClosed.Fire();
        }

        protected virtual void ClosingView()
        {
        }

        /// <summary>
        /// The screen is closed, but might be cached
        /// </summary>
        protected abstract void OnClose();

        /// <summary>
        /// Plays the end animation. If there is none the finished
        /// is directly triggered.
        /// </summary>
        public void PlayCloseAnimation()
        {
            IsAnimating = true;
            screenAnimation.PlayEndAnimation(this);
        }

        private void OnEndScreenAnimationFinished()
        {
            IsAnimating = false;
            CloseAnimationFinished.Fire(this);
        }



        protected void OnDestroy()
        {
            OnScreenDestroy();
            //CreationAssetsLoaded = null;
            ScreenDestroyed.Fire();
        }

        /// <summary>
        /// The asset is being destroyed
        /// </summary>
        protected virtual void OnScreenDestroy()
        {
        }

        
        public bool IsDialog
        {
            get
            {
                return typeof(TModel) == typeof(IDialogModelReader);
            }
        }


        public void OnCallOutActivated(ACallOutComponent callOut)
        {
            ScreenSystem.OpenPanel(new OpenPanelVO(
                new ScreenIdentifier(callOut.CallOutPanelId),
                new CallOutScreenProperties
                {
                    CallOutComponent = callOut
                },
                CommonLayerIds.CallOuts));
        }

        /// <summary>
        /// Marks the view as uncachable. Caching will be avoided.
        /// </summary>
        public void MarkAsUncachable()
        {
            IsUncacheable = true;
        }


        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }


        /// <summary>
        /// Gets the screen animation. You bet it does!
        /// </summary>
        public ScreenAnimation ScreenAnimation {
            get
            {
                return screenAnimation;
            }
        }

        public CanvasGroup CanvasGroup {
            get
            {
                if (!canvasGroup)
                {
                    canvasGroup = GetComponent<CanvasGroup>();
                    if (!canvasGroup)
                    {
                        throw new Exception(name + ".prefab has no canvas group");
                    }
                }
                return canvasGroup;
            }
            set
            {
                canvasGroup = value;
            }
        }

        protected void LoadComponent(string asset, Action<GameObject> callback = null)
        {
            LockSystem.CustomLock(CustomLockReason.LoadingAsset, asset, true);
            //AssetProvider.Instance.GetAsset(asset, delegate (GameObject componentGO)
            
            ScreenSystem.LoadAssetBundle(asset, delegate (GameObject componentGO)
            {
                OnComponentLoaded(asset, componentGO, callback);
            });
        }

        protected void OnComponentLoaded(string asset, GameObject componentGO, Action<GameObject> callback)
        {
            componentGO = Instantiate(componentGO);
            componentGO.gameObject.transform.SetParent(transform, false);

            if (callback != null)
            {
                callback(componentGO.gameObject);
            }

            LockSystem.CustomLock(CustomLockReason.LoadingAsset, asset, false);
        }

        protected void LoadComponent<T>(string asset, Action<T> callback = null) where T : MonoBehaviour
        {
            LoadComponent(asset, delegate (GameObject componentGO)
            {
                T comp = componentGO.GetComponent<T>();
                if (comp == null)
                {
                    throw new Exception("Can't find script for " + asset);
                }
                componentGO.gameObject.transform.SetParent(transform, false);

                if (callback != null)
                {
                    callback(comp);
                }
            });
        }

    }
}