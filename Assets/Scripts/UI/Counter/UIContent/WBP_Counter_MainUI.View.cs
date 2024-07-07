using UnityEngine;
using UnityEngine.Rendering;

public partial class WBP_Counter_MainUI
{
	[HideInInspector] public WBP_Counter_TestSV WBP_Counter_TestSV;
	[HideInInspector] public UnityEngine.UI.Button OpenMain;
	[HideInInspector] public UnityEngine.UI.Button Button_OpenSV;

	private void Awake()
	{
		UIContent = transform.Find("UIContent");
		UIMask = transform.Find("UIMask").GetComponent<CanvasGroup>();
		CanvasGroup = UIContent.GetComponent<CanvasGroup>();
		SortingGroup = transform.GetComponent<SortingGroup>();

		// 组件事件绑定
		AddButtonClickListener(OpenMain, OnOpenMainClick);
		AddButtonClickListener(Button_OpenSV, OnButton_OpenSVClick);
	}
}
