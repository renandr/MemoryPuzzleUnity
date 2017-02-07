using GGS.CakeBox.Utils;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIListElementHider : MonoBehaviour
{
    [SerializeField] 
    private Vector2 offset;
    [Tooltip("If empty, will use root canvas as bounds")]
    [SerializeField]
    private RectTransform boundsRectTransform;

    public event GenericEvent<UIListElementHider> ElementExitedView;

    private RectTransform rectTransform;
    private Canvas elementCanvas;
    private Vector3[] canvasCorners;
    private Vector3[] elementCorners;
    
    private bool canvasEnabled;
    private bool setup;

	private void Setup()
	{
	    rectTransform = (RectTransform) transform;
	    elementCanvas = gameObject.GetComponent<Canvas>();
	    if (elementCanvas == null)
	    {
	        elementCanvas = gameObject.AddComponent<Canvas>();
	    }
        canvasCorners = new Vector3[4];
        elementCorners = new Vector3[4];
	    if (boundsRectTransform == null)
	    {
            boundsRectTransform = transform.root.GetComponent<RectTransform>();
	    }
        boundsRectTransform.GetWorldCorners(canvasCorners);
	    rectTransform.GetWorldCorners(elementCorners);
	    Render(true);
	    setup = true;
	}

    /// <summary>
    /// Setup is done at the Update loop because if we do it on Awake and the object
    /// is dynamic, it will probably not be reparented yet (which breaks our fetching for the root
    /// canvas).
    /// </summary>
    private void Update () 
    {
        if (!setup)
        {
            Setup();
        }
        if (rectTransform.hasChanged)
        {
            rectTransform.GetWorldCorners(elementCorners);
            Verify();
        }
    }

    private void Verify()
    {
        Render(!(elementCorners[0].x > canvasCorners[2].x + offset.x || elementCorners[2].x - offset.x < canvasCorners[0].x
             || elementCorners[0].y > canvasCorners[2].y - offset.y || elementCorners[2].y + offset.y < canvasCorners[0].y));
    }

    private void Render(bool visible)
    {
        if (canvasEnabled != visible)
        {
            elementCanvas.pixelPerfect = visible;
            canvasEnabled = visible;
            if (!visible)
            {
                ElementExitedView.Fire(this);
            }
        }
    }

}
