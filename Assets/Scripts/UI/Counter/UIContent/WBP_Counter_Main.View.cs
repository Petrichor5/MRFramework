using UnityEngine;
using UnityEngine.Rendering;

public partial class WBP_Counter_Main
{
	[HideInInspector] public UnityEngine.UI.Button CloseButton;
	[HideInInspector] public UnityEngine.UI.Text Count;
	[HideInInspector] public UnityEngine.UI.Button Increase;
	[HideInInspector] public UnityEngine.UI.Button Decrease;

	private void Awake()
	{
		UIContent = transform.Find("UIContent");
		UIMask = transform.Find("UIMask").GetComponent<CanvasGroup>();
		CanvasGroup = UIContent.GetComponent<CanvasGroup>();
		SortingGroup = transform.GetComponent<SortingGroup>();

		// 组件事件绑定
		AddButtonClickListener(CloseButton, OnCloseButtonClick);
		AddButtonClickListener(Increase, OnIncreaseClick);
		AddButtonClickListener(Decrease, OnDecreaseClick);
	}
}
