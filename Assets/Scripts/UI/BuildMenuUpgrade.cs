using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuUpgrade : MonoBehaviour
{
    public Sprite flower1;
    public Sprite flower2;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // access SpriteRenderer that is attached to game object
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null)
            spriteRenderer.sprite = flower1;
  
                

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ChangeSprite();
    }

    void ChangeSprite()
    {
        if (spriteRenderer.sprite == flower1)
            spriteRenderer.sprite = flower2;
        else
            spriteRenderer.sprite = flower1;
    }
 

    private void OnMouseDown()
    {
        
    }
}