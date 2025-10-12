using UnityEngine;
using System.Collections;

public static class InterfaceUtils
{
    public static IEnumerator AnimateAppear(CanvasGroup group, UnityEngine.Transform target, float duration = 0.25f, float delay = 0f, float targetScaleMagnitude = 1f)
    {
        if (!group.gameObject.activeSelf)
        {
            group.gameObject.SetActive(true);
        }

        Vector3 targetScale = Vector3.one * targetScaleMagnitude;
        float time = 0f;

        group.alpha = 0f;
        target.localScale = Vector3.one * 0.8f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / duration;

            group.alpha = Mathf.Lerp(0f, 1f, normalizedTime);
            target.localScale = Vector3.Lerp(Vector3.one * 0.8f, targetScale, normalizedTime);

            yield return null;
        }

        group.alpha = 1f;
        target.localScale = targetScale;
    }

    public static IEnumerator AnimateDisappear(CanvasGroup group, UnityEngine.Transform target, float duration = 0.25f)
    {
        Vector3 initialScale = target.localScale;
        float time = 0f;

        group.alpha = 1f;
        target.localScale = initialScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / duration;

            group.alpha = Mathf.Lerp(1f, 0, normalizedTime);
            target.localScale = Vector3.Lerp(initialScale, Vector3.one * 0.8f, normalizedTime);

            yield return null;
        }

        group.alpha = 0f;
        target.localScale = initialScale;

        group.gameObject.SetActive(false);
    }

    public static IEnumerator AnimateWidthChange(RectTransform target, float targetWidth, float duration = 0.25f, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        float startWidth = target.rect.width;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / duration;
            float newWidth = Mathf.Lerp(startWidth, targetWidth, normalizedTime);

            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

            yield return null;
        }

        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
    }

    public static IEnumerator AnimateHorizontalMovement(RectTransform target, float horizontalDisplacement, float duration = 0.25f)
    {
        Vector2 startPos = target.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(horizontalDisplacement, 0f);

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / duration;

            float x = Mathf.Lerp(startPos.x, endPos.x, normalizedTime);
            target.anchoredPosition = new Vector2(x, startPos.y);

            yield return null;
        }

        target.anchoredPosition = endPos;
    }
}