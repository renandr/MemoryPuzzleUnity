using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GGS.CakeBox.Logging;
using GGS.GameLocks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// Abstract class with methods for handling the game objects for the user interfaces
    /// </summary>
    public abstract class ALayer : MonoBehaviour
    {
        [Header("ALayer")]
        [SerializeField]
        protected bool shouldAddChildOnTop;

        [SerializeField]
        protected bool cachesDefault = false;

        public string Id;

        /// <summary>
        /// Dictionary holding the screens that are cached in stacks, for pooling
        /// </summary>
        protected readonly Dictionary<string, Stack<IScreen>> CachedScreenViews = new Dictionary<string, Stack<IScreen>>();

        /// <summary>
        /// Dictionary holding the screens that are active
        /// </summary>
        protected readonly Dictionary<ScreenIdentifier, IScreen> ActiveViews = new Dictionary<ScreenIdentifier, IScreen>();

        private string isLoadingWhat;
        private bool isAnimatingOpen;
        private bool isAnimatingClose;

        private bool loadTesting;

        protected void Awake()
        {
            ScreenSystem.RegisterLayer(Id);
        }
        protected void Start()
        {
            ScreenSystem.LoadTestEnabled+=EnableLoadTest;
            ScreenSystem.PreCacheScreenRequested += PreCache;
            ScreenSystem.LayerRestrictionChanged += OnRestrictionChange;
            LayerStart();
        }

        protected abstract void LayerStart();

        protected void OnDestroy()
        {
            ScreenSystem.LoadTestEnabled -= EnableLoadTest;
            ScreenSystem.PreCacheScreenRequested -= PreCache;
            ScreenSystem.LayerRestrictionChanged -= OnRestrictionChange;
            OnPanelDestroy();
        }

        protected abstract void OnPanelDestroy();

        private void OnRestrictionChange(string layer, bool openingEnabled)
        {
            if (layer == Id)
            {
                OnScreenOpeningRestrictionChange(openingEnabled);
            }
        }

        protected virtual void OnScreenOpeningRestrictionChange(bool openingEnabled)
        {
            
        }

        private void PreCache(string screenAssetName, string layerId)
        {
            if (layerId != Id)
            {
                return;
            }

            foreach (ScreenIdentifier identifier in ActiveViews.Keys)
            {
                if (identifier.AssetId == screenAssetName)
                {
                    return;
                }
            }

            Stack<IScreen> list;
            if (CachedScreenViews.TryGetValue(screenAssetName, out list))
            {
                return;
            }
            //AssetProvider.Instance.GetAsset(screenAssetName, delegate (GameObject screenGO)
            ScreenSystem.LoadAssetBundle(screenAssetName, delegate (GameObject screenGO)
            {
                var screen = screenGO.GetComponent<IScreen>();
                if (screen == null)
                {
                    throw new Exception("Can't find View script for " + screenAssetName + ". Does the Class inherit from IScreenView?");
                }

                if (ShouldViewBeCached(screen))
                {
                    screenGO = Instantiate(screenGO);
                    var instanceScreen = screenGO.GetComponent<IScreen>();
                    instanceScreen.ScreenId = new ScreenIdentifier(screenAssetName);
                    instanceScreen.gameObject.transform.SetParent(transform, false);
                    CacheScreen(instanceScreen);
                }
                PrintChildren("Pre cached " + screenAssetName);
            });
        }

        public void EnableLoadTest()
        {
            loadTesting = true;
        }

        struct ScreenQueueEntry
        {
            public OpenScreenVO ViewVO;
            public Action<IScreen, OpenScreenVO> Callback;
        }

        private readonly Queue<ScreenQueueEntry> loadQueue = new Queue<ScreenQueueEntry>();

        /// <summary>
        /// Screen was requested, but that will only happen when it's its time in the queue (regardles if it's cached)
        /// </summary>
        public virtual void ShowScreen(OpenScreenVO screenVO, Action<IScreen, OpenScreenVO> callback = null, bool forceEnqueue = false)
        {
            loadQueue.Enqueue(new ScreenQueueEntry
            {
                ViewVO = screenVO,
                Callback = callback
            });

            if (loadQueue.Count == 1 && !forceEnqueue && !IsLoading)
            {
                DoLoadScreen(screenVO, callback);
            }
        }

        private bool IsLoading 
        {
            get
            {
                return !string.IsNullOrEmpty(isLoadingWhat);
            }
        }

        /// <summary>
        /// Checks if the screen should be loaded from AB or from cache
        /// </summary>
        /// <param name="screenVO"></param>
        /// <param name="callback"></param>
        private void DoLoadScreen(OpenScreenVO screenVO, Action<IScreen, OpenScreenVO> callback = null)
        {
            IScreen screen;
            if (ActiveViews.TryGetValue(screenVO.Id, out screen))
            {
                throw new Exception("Trying to add screen with the same id " + screenVO.Id);
            }
            Stack<IScreen> list;

            if ((CachedScreenViews.TryGetValue(screenVO.Id.AssetId, out list) && list.Count > 0) && !loadTesting)
            {
                screen = list.Pop();

                ActiveViews[screenVO.Id] = screen;

                loadQueue.Dequeue();
                OpenScreen(screen, screenVO, false, callback);
            }
            else
            {
                isLoadingWhat = screenVO.Id.ToString();
                LockSystem.CustomLock(CustomLockReason.LoadingAsset, screenVO.Id.AssetId, true);
                //AssetProvider.Instance.GetAsset(screenVO.Id.AssetId, delegate(GameObject screenGO)
                ScreenSystem.LoadAssetBundle(screenVO.Id.AssetId, delegate (GameObject screenGO)
                {
                    ScreenQueueEntry entry = loadQueue.Dequeue();
                    if (entry.ViewVO.Id == screenVO.Id)
                    {
                        OnScreenLoaded(screenGO, screenVO, callback);
                    }
                    else
                    {
                        GGLog.LogError("Problem with the screen loading queue, " + entry.ViewVO.Id + " is different than " + screenVO.Id, ScreenSystem.LogType);
                    }

                });
            }
        }

        /// <summary>
        /// Moves the load queue
        /// </summary>
        public void TryOpenNextQueuedScreen()
        {
            if (HasMoreScreensToLoad && !IsLoading)
            {
                ScreenQueueEntry next = loadQueue.Peek();
                DoLoadScreen(next.ViewVO, next.Callback);
            }
        }

        public bool HasMoreScreensToLoad
        {
            get
            {
                return (loadQueue != null && loadQueue.Count != 0);
            }
        }
        /// <summary>
        /// Sets up the screen after it's loaded, only called if it wasn't cached
        /// </summary>
        protected void OnScreenLoaded(GameObject screenGO, OpenScreenVO screenVO, Action<IScreen, OpenScreenVO> callback = null)
        {
            screenGO = Instantiate(screenGO);

            var screen = screenGO.GetComponent<IScreen>();
            if (screen == null)
            {
                throw new Exception("Can't find View script for " + screenVO.Id.AssetId + ". Does the Class inherit from IScreenView?");
            }

            if (ActiveViews.ContainsKey(screenVO.Id))
            {
                GGLog.LogError(this + " Layer already contains a screen with id: " + screenVO.Id, ScreenSystem.LogType);
            }
            ActiveViews.Add(screenVO.Id, screen);
            LockSystem.CustomLock(CustomLockReason.LoadingAsset, screenVO.Id.AssetId, false);
            OpenScreen(screen, screenVO, true, callback);
            PrintChildren("Loaded "+screenVO.Id.AssetId);
        }

        /// <summary>
        /// Do the proper set up. Happens regardless if the screen was cached or loaded
        /// </summary>
        private void OpenScreen(IScreen screen, OpenScreenVO screenVO, bool wasLoaded, Action<IScreen, OpenScreenVO> callback = null)
        {
            //Setting the GO as active should always happen. In case anyone forgets to set it as active when working with the prefab
            //Hiding and disabling panels should only be done by using the open and close signals
            screen.gameObject.SetActive(true);
            screen.ScreenId = screenVO.Id;
            screen.InstanceIdentifier = screenVO.InstanceIdentifier;

            SetName(screen);

            //precached screens 
            if (wasLoaded || !screen.IsCreated)
            {
                screen.ScreenIsCreated = delegate
                {
                    OnScreenOpened(screen, screenVO, callback);
                };
            }
            else
            {
                OnScreenOpened(screen, screenVO, callback);
            }
        }

        /// <summary>
        /// Screen is finally created, and its creation assets too
        /// </summary>
        protected void OnScreenOpened(IScreen screen, OpenScreenVO screenVO, Action<IScreen, OpenScreenVO> callback = null)
        {
            screen.gameObject.transform.SetParent(transform, false);
            SetSiblingIndex(screenVO, screen);
            screen.ScreenIsCreated = null;

            if(callback != null)
            {
                callback(screen, screenVO);
            }

            screen.NotifyOpen();
            ScreenSystem.NotifyScreenActive(Id, screenVO.Id);

            if (!HasMoreScreensToLoad && !(screenVO.PreventAnimation))
            {
                screen.OpenAnimationFinished += OnOpenAnimationFinished;
                LockSystem.CustomLock(CustomLockReason.AnimatingUIOpen, screen.ScreenId.ToString(), true);
                isAnimatingOpen = true;
                screen.PlayOpenAnimation();
            }
            screenVO.PreventAnimation = false;
            isLoadingWhat = "";
            TryOpenNextQueuedScreen();
        }

        protected virtual void OnOpenAnimationFinished(IScreen screen)
        {
            screen.OpenAnimationFinished -= OnOpenAnimationFinished;
            LockSystem.CustomLock(CustomLockReason.AnimatingUIOpen, screen.ScreenId.ToString(), false);
            isAnimatingOpen = false;
        }


        /// <summary>
        /// Start the animation and triggers the screen to close
        /// </summary>
        /// <param name="screenVO"></param>
        public virtual void CloseScreen(OpenScreenVO screenVO, bool animate = true)
        {
            IScreen screen;
            if (ActiveViews.TryGetValue(screenVO.Id, out screen))
            {
                if (IsLoading && screenVO.Id.ToString() == isLoadingWhat)
                {
                    GGLog.LogWarning("OMG closing a dialog that was still loading! " + isLoadingWhat, ScreenSystem.LogType);
                    //Well this is of awkward, just remove everything and pretend nothing happened....
                    screen.ScreenIsCreated = null;
                    ActiveViews.Remove(screen.ScreenId);
                    Destroy(screen.gameObject);
                    isLoadingWhat = "";
                    TryOpenNextQueuedScreen();
                }
                else if (animate)
                {
                    screen.CloseAnimationFinished += OnCloseAnimationFinished;
                    LockSystem.CustomLock(CustomLockReason.AnimatingUIClose, screen.ScreenId.ToString(), true);
                    isAnimatingClose = true;
                    screen.PlayCloseAnimation();
                }
                else
                {
                    OnCloseScreen(screen.ScreenId);
                }
            }
            else
            {
                GGLog.LogWarning("Tried to close screen that is not in this layer. (" + screenVO.Id + ")" + " isLoading["+isLoadingWhat+"]" + " isOpening[" + isAnimatingOpen + "]" + " isClosing[" + isAnimatingClose + "]", ScreenSystem.LogType);
            }
        }

        /// <summary>
        /// Callback for when the animation ends
        /// </summary>
        protected virtual void OnCloseScreen(ScreenIdentifier screenId)
        {
            IScreen screen;
            if (ActiveViews.TryGetValue(screenId, out screen))
            {
                ActiveViews.Remove(screen.ScreenId);
                screen.NotifyClose();
                RemoveScreen(screen);
            }
            else
            {
                GGLog.LogWarning("Tried to close screen that is not in this layer. (" + screenId + ")" + " isLoading[" + isLoadingWhat + "]" + " isOpening[" + isAnimatingOpen + "]" + " isClosing[" + isAnimatingClose + "]", ScreenSystem.LogType);
            }
        }

        public virtual void CloseEverything()
        {
            List<IScreen> list = ActiveViews.Values.ToList();
            foreach (IScreen screen in list)
            {
                OnCloseScreen(screen.ScreenId);
            }
            ActiveViews.Clear();
        }

        protected virtual void OnCloseAnimationFinished(IScreen screen)
        {
            screen.CloseAnimationFinished -= OnCloseAnimationFinished;
            LockSystem.CustomLock(CustomLockReason.AnimatingUIClose, screen.ScreenId.ToString(), false);
            isAnimatingClose = false;
            OnCloseScreen(screen.ScreenId);
        }

        /// <summary>
        /// Makes the screen not "active", might destroy it or cache it
        /// </summary>
        private void RemoveScreen(IScreen screen)
        {
            LockSystem.CustomLock(CustomLockReason.AnimatingUIClose, screen.ScreenId.ToString(), false);
            LockSystem.CustomLock(CustomLockReason.AnimatingUIOpen, screen.ScreenId.ToString(), false);
            isAnimatingClose = false;
            isAnimatingOpen = false;
            if (ShouldViewBeCached(screen))
            {
                CacheScreen(screen);
            }
            else
            {
                Destroy(screen.gameObject);
            }

        }

        private void CacheScreen(IScreen screen)
        {
            screen.gameObject.SetActive(false);
            //screen.transform.SetAsLastSibling();

            Stack<IScreen> target;
            if (!CachedScreenViews.TryGetValue(screen.ScreenId.AssetId, out target))
            {
                target = new Stack<IScreen>();
                CachedScreenViews[screen.ScreenId.AssetId] = target;
            }

            target.Push(screen);

            PrintChildren("Cached " + screen.ScreenId.AssetId);
        }

        /// <summary>
        /// Reusable method to load a prefab from the ui folder with the given name
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected GameObject LoadPrefab(string asset)
        {
            string assetPath = "UI/" + asset;
            Object loadedPrefab = Resources.Load(assetPath);

            if (loadedPrefab == null)
            {
                throw new NullReferenceException("Can't find prefab for " + assetPath);
            }

            return Instantiate(loadedPrefab) as GameObject;
        }

        /// <summary>
        /// Changes the game object's name
        /// Only called inside the editor since it decreases performance
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="view"></param>
        [Conditional("UNITY_EDITOR")]
        protected void SetName(IScreen view)
        {
            string newName = view.ScreenId.ToString();
            if (ShouldViewBeCached(view))
            {
                newName += "_[cached]";
            }

            view.gameObject.name = newName;
        }

        [Conditional("DEBUG")]
        protected virtual void PrintChildren(string str = "")
        {
            var builder = new StringBuilder(gameObject.name + ": " + str + "\n");
            if (ActiveViews.Count > 0)
            {
                builder.AppendFormat("Active screens in :{0} \n", ActiveViews.Count);
                foreach (ScreenIdentifier child in ActiveViews.Keys)
                {
                    builder.AppendFormat("- {0}\n", child.AssetId);
                }
            }

            builder.AppendFormat("Cached screens :{0} \n", CachedScreenViews.Count);
            foreach (KeyValuePair<string, Stack<IScreen>> cachedChild in CachedScreenViews)
            {
                builder.AppendFormat("- {0} ({1})\n", cachedChild.Key, cachedChild.Value.Count);
            }
            GGLog.Log(builder.ToString(), ScreenSystem.LogType);
        }

        private bool ShouldViewBeCached(IScreen screen)
        {
            // while load testing everyhing will be cached but not reused.
            if (loadTesting)
            {
                return true;
            }

            if (screen.IsUncacheable)
            {
                return false;
            }

            bool cacheScreen;
            switch (screen.CachingBehavior)
            {
                case CachingBehavior.Default:
                    cacheScreen = cachesDefault;
                    break;

                case CachingBehavior.ForceCaching:
                    cacheScreen = true;
                    break;

                case CachingBehavior.ForceNotCaching:
                    cacheScreen = false;
                    break;

                default:
                    cacheScreen = false;
                    break;
            }

            return cacheScreen;
        }

        /// <summary>
        /// Adjusts position in the layer due to animations
        /// </summary>
        private void SetSiblingIndex(OpenScreenVO screenVO, IScreen screen)
        {
            // note: what is in the hierarchy lower means in front.
            if(!shouldAddChildOnTop)
            {
                if (screenVO.SetAsSecondSibling)
                {
                    screen.gameObject.transform.SetSiblingIndex(1);
                    screenVO.SetAsSecondSibling = false;   
                }
                else
                {
                    screen.gameObject.transform.SetSiblingIndex(0);
                }
            }
            else
            {
                if (screenVO.SetAsSecondSibling)
                {
                    if (transform.childCount >= 2)
                    {
                        screen.gameObject.transform.SetSiblingIndex(transform.childCount - 2);
                    }
                    screenVO.SetAsSecondSibling = false;
                }
                else
                {
                    screen.gameObject.transform.SetAsLastSibling();
                }
            }
        }
    }
}