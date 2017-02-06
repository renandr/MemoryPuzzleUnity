using DG.Tweening;
using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary> 
    /// A basic class for all tween animation components.
    /// </summary>
    public abstract class AAnimationComponent : MonoBehaviour
    {
        [Tooltip("the animation will automatically start playing on awake.")]
        [SerializeField]
        private bool playOnAwake;

        [Tooltip("the duration of the animation time.")]
        [SerializeField]
        private float duration;

        protected Tween Tween { get; set; }

        #region Request
        public TweenAnimationDelegate PlayAnimation;
        public TweenAnimationDelegate SkipAnimation;
        public TweenAnimationDelegate CompleteAnimation;
        #endregion

        #region Response
        public event TweenAnimationDelegate OnAnimationCompleted;
        #endregion

        /// <summary>
        /// Awake is called when the script Instance is being loaded.
        /// Awake is used to initialize any variables or game state before the game starts.
        /// Awake is called only once during the lifetime of the script Instance. 
        /// Awake is called after all objects are initialized so you can safely speak to other objects or query them using eg. GameObject.FindWithTag. 
        /// Each GameObject's Awake is called in a random order between objects .
        /// Because of this, you should use Awake to set up references between scripts, and use Start to pass any information back and forth. 
        /// Awake is always called before any Start functions. This allows you to order initialization of scripts. Awake can not act as a coroutine.
        /// Note for C#: use Awake instead of the constructor for initialization, as the serialized state of the component is undefined at construction time. 
        /// Awake is called once, just like the constructor.
        /// </summary>
        protected virtual void Awake()
        {
            //Initzialize the settings for DoTween
            DOTween.Init();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        /// Like the Awake function, Start is called exactly once in the lifetime of the script.
        /// However, Awake is called when the script object is initialised, regardless of whether or not the script is enabled.
        /// Start may not be called on the same frame as Awake if the script is not enabled at initialisation time.
        /// The Awake function is called on all objects in the scene before any object's Start function is called. 
        /// This fact is useful in cases where object A's initialisation code needs to rely on object B's already being initialised; 
        /// B's initialisation should be done in Awake while A's should be done in Start.
        /// Where objects are instantiated during gameplay, their Awake function will naturally be called after the Start functions of scene objects have already completed.
        /// </summary>
        protected virtual void Start()
        {
            //play the animation once direct after setup all values
            if (playOnAwake)
            {
                Play();
            }
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            PlayAnimation += Play;
            SkipAnimation += Skip;
            CompleteAnimation += Complete;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled () or inactive.
        /// This is also called when the object is destroyed and can be used for any cleanup code. 
        /// When scripts are reloaded after compilation has finished, OnDisable will be called, followed by an OnEnable after the script has been loaded.
        ///  </summary>
        protected virtual void OnDisable()
        {
            PlayAnimation -= Play;
            SkipAnimation -= Skip;
            CompleteAnimation -= Complete;
        }

        /// <summary>
        /// Play the animation.
        ///</summary>
        protected abstract void Play();

        /// <summary>
        /// Skip the animation after playing.
        ///</summary>
        protected abstract void Skip();

        /// <summary>
        /// Complete the animation after playing.
        ///</summary>
        protected abstract void Complete();

        /// <summary>
        /// Called when animation is completed
        /// need to be called in the OnComplete method from the current tween
        ///</summary>
        protected virtual void OnComplete()
        {
            ResetTweenSpeed();
            if (OnAnimationCompleted != null)
            {
                OnAnimationCompleted();
            }
        }

        /// <summary> 
        /// Reset the animation speed it used the timeScale of the tween
        /// 1 = the normal speed
        /// </summary>
        private void ResetTweenSpeed()
        {
            Tween.timeScale = 1;
        }

        /// <summary>
        /// Animation is playing?
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return Tween.IsPlaying();
            }
        }

        /// <summary> 
        ///  Gets the duration of the animation.
        /// </summary>
        public float AnimationDuration
        {
            get { return duration; }
            set { duration = value; }
        }
    }
}
