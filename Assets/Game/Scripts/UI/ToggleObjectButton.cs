using UnityEngine;
using UnityEngine.UI;

public class ToggleObjectButton : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject parent;

    private CanvasGroup targetGroup;
    private CanvasGroup parentGroup;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => ToggleObject());

        targetGroup = target.GetComponent<CanvasGroup>();
        
        if (parent)
        {
            parentGroup = parent.GetComponent<CanvasGroup>();
        }
    }

    private void ToggleObject()
    {
        bool isActive = target.activeSelf;

        if (!isActive)
        {
            StartCoroutine(InterfaceUtils.AnimateAppear(targetGroup, target.transform, 0.2f));
        }
        else
        {
            StartCoroutine(InterfaceUtils.AnimateDisappear(targetGroup, target.transform, 0.15f));
        }

        if (parent)
        {
            isActive = parent.activeSelf;

            if (!isActive)
            {
                StartCoroutine(InterfaceUtils.AnimateAppear(parentGroup, target.transform, 0.2f));
            }
            else
            {
                StartCoroutine(InterfaceUtils.AnimateDisappear(parentGroup, target.transform, 0.15f));
            }
        }
    }
}
