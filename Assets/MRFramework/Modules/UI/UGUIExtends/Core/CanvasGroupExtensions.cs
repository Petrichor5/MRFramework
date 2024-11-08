using DG.Tweening;
using UnityEngine;

namespace MRFramework
{
    public static class CanvasGroupExtensions
    {
        public static Tweener DOFade(this CanvasGroup canvasGroup, float endValue, float duration)
        {
            return DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, endValue, duration);
        }
    }
}