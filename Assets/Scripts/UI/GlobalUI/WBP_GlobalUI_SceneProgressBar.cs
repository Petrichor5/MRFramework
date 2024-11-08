using MRFramework;

public partial class WBP_GlobalUI_SceneProgressBar : MainPanel
{
	public void UpdateProgress(float value)
	{
		Slider_Progress.value = value;
	}

	#region UI组件事件


	#endregion
}
