using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarIndicator : MonoBehaviour
{
    public int healthBarIndex = 0;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void turnOnSprite()
    {
        image.color = Color.white;
    }

    public void turnOffSprite()
    {
        image.color = Color.black;
    }
}
