using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MRFramework;

public partial class WBP_Main_Main : PanelBase
{
	public override void OnFirstOpen()
	{
		base.OnFirstOpen();
		InitWidgets();
	}

	public void InitWidgets()
	{
		AddButtonClickListener(Button_Start, OnStartClick);

	}

	public override void OnOpen()
	{
		base.OnOpen();
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public override void OnDispose()
	{
		base.OnDispose();
	}

	#region UI组件事件

	public void OnStartClick()
	{
	}

	#endregion
}
