using UnityEngine;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{

    public float min = 0;
    public float max = 100;
    public float current = 100;
    public Transform sliderBar;
    public Text textField;
    public string unit = "m/s";

    public Color defaultColor = Color.white;
    public Color overShootColor = Color.yellow;
    // Use this for initialization

    public void SetMin(float _min) { min = _min; }
    public void SetMax(float _max) { max = _max; }
    public void SetValue(float val)
    {
        current = val > min ? val : min; // lower Clamp
        UpdateSlider();
    }
    private void UpdateSlider()
    {
        // set slider
        Vector3 scale = sliderBar.localScale;
        scale.x = current / (max-min) >0 ? current / (max-min):0;
        sliderBar.localScale = scale;

        if (current < max)
            sliderBar.GetComponent<Image>().color = defaultColor;
        else
            sliderBar.GetComponent<Image>().color = overShootColor;

        // set text
        if(textField!=null) textField.text = ""+current.ToString("F0") + " "+ unit;    
    }
}
