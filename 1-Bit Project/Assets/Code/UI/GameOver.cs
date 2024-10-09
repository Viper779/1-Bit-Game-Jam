using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Sprite[] image;
    [SerializeField] private int imageNumber;
    public Image imageComponent;
    public int currentFrame;

    // Start is called before the first frame update
    void Start()
    {
        if (imageComponent == null)
        {
            imageComponent = GetComponent<Image>();
        }
        if (imageComponent != null)
        {
            imageComponent.color = Color.white;
            if (image.Length > 0)
            {
                imageComponent.sprite = image[0];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (WaveBasedEnemySpawner.winCond)
        {
            imageComponent.sprite = image[1];
        }
    }
}
