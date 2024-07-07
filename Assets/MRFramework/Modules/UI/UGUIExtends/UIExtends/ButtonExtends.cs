using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public static class ButtonExtends
{
    public static void AddClickListener(this Button widget, UnityAction action)
    {
        // 清除之前所有的监听器，防止重复添加
        //widget.onClick.RemoveAllListeners();
        //widget.onClick.AddListener(() =>
        //{
        //    widget.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
        //    {
        //        widget.transform.DOScale(1f, 0.1f).SetEase(Ease.OutQuad);
        //        action();
        //    });
        //});
    }
}