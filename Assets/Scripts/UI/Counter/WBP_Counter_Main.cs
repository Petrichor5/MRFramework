using MRFramework;
using UnityEngine.SocialPlatforms;

public partial class WBP_Counter_Main : MainPanel
{
    private CounterControl mCounterControl = GameArch.Instance.GetController<CounterControl>();

    public override void OnAwake()
	{
		// 添加事件
        CounterEvent counterEvent = new CounterEvent();
        AddEventListener(counterEvent.OnUpdateCounterInfo, RefreshInfo);

        RefreshInfo();
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

	// 刷新信息
	public void RefreshInfo()
	{
		int num = mCounterControl.GetCount();
        Count.SetText(num.ToString());
	}

	#region UI组件事件

	// 添加按钮
	public void OnIncreaseClick()
	{
        mCounterControl.Increase();
    }

	// 减少按钮
	public void OnDecreaseClick()
	{
        mCounterControl.Decrease();
    }

    public void OnCloseButtonClick()
    {
        GameModule.UIMgr.ClosePanel<WBP_Counter_Main>();
    }

    #endregion
}
