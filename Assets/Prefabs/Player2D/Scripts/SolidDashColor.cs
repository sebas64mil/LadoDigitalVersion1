using System.Xml.Serialization;
using UnityEngine;

public class SolidDashColor : MonoBehaviour
{

    private SpriteRenderer myRenderer;
    private Shader myMaterial;
    public Color dashColor;
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myMaterial = Shader.Find("GUI/Text Shader");
    }


    void ColorSprite() { 
       myRenderer.material.shader = myMaterial;
        myRenderer.color = dashColor;
    }

    public void Finish() { 
    
    gameObject.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
       ColorSprite();
    }
}
