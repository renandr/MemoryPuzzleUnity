using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameArea : MonoBehaviour {
    

    void Start () {
        var rec = GetComponent<RectTransform>();
        Debug.Log("transform position " + rec.transform.position);
        Debug.Log("transform local position"+rec.transform.localPosition);
        Debug.Log("local position" + rec.localPosition);
        Debug.Log("size delta" + rec.sizeDelta);
    }

    
}
