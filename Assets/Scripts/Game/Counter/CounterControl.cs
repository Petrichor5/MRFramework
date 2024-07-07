using MRFramework;
using System.Collections.Generic;

public class CounterControl : AbstractController
{
    private CounterModel mModel;

    private CounterEvent mEvent;

    protected override void OnInit()
    {
        mModel = new CounterModel();

        mEvent = new CounterEvent();
    }

    public void OnDestroy()
    {
        mModel.OnDestroy();
    }

    public CounterModel GetModel()
    {
        return mModel;
    }

    public int GetCount()
    {
        return mModel.GetCount();
    }

    public void Increase()
    {
        mModel.Increase();

        GameModule.EventMgr.TriggerEventListener(mEvent.OnUpdateCounterInfo);
    }

    public void Decrease()
    {
        mModel.Decrease();

        GameModule.EventMgr.TriggerEventListener(mEvent.OnUpdateCounterInfo);
    }
}
