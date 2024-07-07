using UnityEngine.Events;
using UnityEngine.UI;

public static class SliderExtends
{
    public static void SetMaxValue(this Slider widget, float value)
    {
        widget.maxValue = value;
    }
    
    public static void SetValue(this Slider widget, float value)
    {
        widget.value = value;
    }
    
    public static void SetMinValue(this Slider widget, float value)
    {
        widget.minValue = value;
    }
    
    public static void AddSliderListener(this Slider widget, UnityAction<float> callback)
    {
        widget.onValueChanged.AddListener(callback);
    }
    
    public static void RemoveSliderListener(this Slider widget, UnityAction<float> callback)
    {
        widget.onValueChanged.RemoveListener(callback);
    }
    
    public static void RemoveAllSliderListeners(this Slider widget)
    {
        widget.onValueChanged.RemoveAllListeners();
    }
}