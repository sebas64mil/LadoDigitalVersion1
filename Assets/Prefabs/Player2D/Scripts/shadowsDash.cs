using UnityEngine;
using System.Collections.Generic;

public class shadowsDash : MonoBehaviour
{
    public static shadowsDash shadow;
    public GameObject sombra;
    public List<GameObject> sombras = new List<GameObject>();
    private float cronometro;
    public float speed;
    private PlayerController2d player;

    void Awake()
    {
        shadow = this;
        player = GetComponent<PlayerController2d>();
    }

    public GameObject GetShadows()
    {
        for (int i = 0; i < sombras.Count; i++)
        {
            if (!sombras[i].activeInHierarchy)
            {
                sombras[i].SetActive(true);
                sombras[i].transform.position = transform.position;
                sombras[i].transform.rotation = transform.rotation;

                SpriteRenderer sr = sombras[i].GetComponent<SpriteRenderer>();
                sr.sprite = GetComponent<SpriteRenderer>().sprite;
                sr.color = player.dashColor; 

                // Actualizamos el color del script SolidDashColor también
                SolidDashColor solid = sombras[i].GetComponent<SolidDashColor>();
                if (solid != null)
                    solid.dashColor = player.dashColor;

                return sombras[i];
            }
        }

        // Si no hay sombras disponibles, se crea una nueva
        GameObject go = Instantiate(sombra, transform.position, transform.rotation);
        SpriteRenderer goSR = go.GetComponent<SpriteRenderer>();
        goSR.sprite = GetComponent<SpriteRenderer>().sprite;
        goSR.color = player.dashColor;

        // También actualizamos el SolidDashColor
        SolidDashColor goSolid = go.GetComponent<SolidDashColor>();
        if (goSolid != null)
            goSolid.dashColor = player.dashColor;

        sombras.Add(go);
        return go;
    }

    public void Sombras_Skill()
    {
        cronometro += Time.deltaTime * speed;
        if (cronometro > 1)
        {
            GetShadows();
            cronometro = 0;
        }
    }
}
