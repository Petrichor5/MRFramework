using UnityEngine;
using UnityEngine.Rendering;

public partial class WBP_Counter_TestSV
{
	[HideInInspector] public UnityEngine.UI.Button CloseButton;

	private void Awake()
	{
		CanvasGroup = transform.Find("UIContent").GetComponent<CanvasGroup>();

		// 组件事件绑定
		AddButtonClickListener(CloseButton, OnCloseButtonClick);
	}
}
