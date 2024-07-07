using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MRFramework;

public partial class WBP_Counter_MainUI : MainPanel
{
	public override void OnAwake()
	{
        WBP_Counter_TestSV.Close();
    }

	public override void OnOpen()
	{

	}

	public override void OnClose()
	{

	}

	public override void OnDispose()
	{

	}

	#region UI组件事件

	public void OnOpenMainClick()
	{
		GameModule.UIMgr.OpenPanel<WBP_Counter_Main>();
	}

	public void OnButton_OpenSVClick()
	{
		WBP_Counter_TestSV.Open();
    }

	#endregion
}
