using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class ToggleWidgetButton : MonoBehaviour
{
    [SerializeField]
    private GameObject widget;

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    private CanvasGroup widgetGroup;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => ToggleWidget());
        widgetGroup = widget.GetComponent<CanvasGroup>();
    }

    public void ToggleWidget()
    {
        bool isActive = widget.activeSelf;

        if (!isActive)
        {
            StartCoroutine(InterfaceUtils.AnimateAppear(widgetGroup, widget.transform, 0.15f, 0.175f));
        }
        else
        {
            StartCoroutine(InterfaceUtils.AnimateDisappear(widgetGroup, widget.transform, 0.075f));
        }

        if (!isActive)
        {
            buttons.ForEach(i => i.interactable = false);
        }
        else 
        {
            buttons.ForEach(i => i.interactable = true);
        }
    }
}
