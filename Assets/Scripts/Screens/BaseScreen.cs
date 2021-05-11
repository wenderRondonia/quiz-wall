using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScreen<T> : Singleton<T>  where T : MonoBehaviour
{

    public GameObject panel;
    public bool IsShowing { get { return panel.activeInHierarchy; } }
    public virtual void Show()
    {
        FadePanel.FadeIn(panel);
    }

    public virtual void Hide()
    {
        FadePanel.FadeOut(panel);
    }
}
