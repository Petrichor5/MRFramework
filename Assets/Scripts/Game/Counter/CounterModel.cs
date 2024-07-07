public class CounterModel
{
    private int mCount;

    public CounterModel()
    {
        mCount = 0;
    }

    public void OnDestroy()
    {
        
    }

    public int GetCount()
    {
        return mCount;
    }

    public void Increase()
    {
        mCount++;

    }

    public void Decrease()
    {
        mCount--;
    }
}
