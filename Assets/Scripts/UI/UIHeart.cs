using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeart : MonoBehaviour
{
    [SerializeField] Sprite heartSpriteFull;
    [SerializeField] Sprite heartSpriteEmpty;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetHeartFull()
    {
        image.sprite = heartSpriteFull;
    }

    public void SetHeartEmpty()
    {
        image.sprite = heartSpriteEmpty;
    }
}
