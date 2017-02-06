using UnityEngine;

namespace GGS.Legends.UI
{
    public abstract class DummyProvider<TView> : MonoBehaviour
    {
        protected void OnEnable()
        {
            var view = GetComponent<TView>();
            if (view != null)
            {
                SetViewContent(view);
            }
        }

        abstract protected void SetViewContent(TView view);
    }
}