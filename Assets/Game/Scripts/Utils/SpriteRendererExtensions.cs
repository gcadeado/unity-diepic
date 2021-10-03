using System.Collections;
using UnityEngine;

public static class SpriteRendererExtensions
{
    public static void SetColor(this SpriteRenderer s, Color c)
    {
        s.color = c;
    }

    public static IEnumerator SetColorAnimation(this SpriteRenderer s, Color startColor, Color endColor, float duration)
    {
        s.color = startColor;
        float timer = 0f;

        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            s.color = Color.Lerp(startColor, endColor, timer / duration);
        }

        s.color = endColor; // Enforce final color
    }
}