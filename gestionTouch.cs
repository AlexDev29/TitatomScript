using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gestionTouch : MonoBehaviour
{
    //Les GameObject qui vont suivre la positions des doigts sur l'écran
    GameObject click1;
    GameObject click2;

    public bool clickChange = false;   //switch d'objet click si le joueur up le touch(0) en premier

    Vector2 iniPos;
    Vector2 startPos;
    Vector2 currentPos;

    public int nbCartouche; //les nombre de cartouche antiFantome du joueur sont stockés ici


    // Start is called before the first frame update
    void Start()
    {
        click1 = GameObject.Find("click1");
        click2 = GameObject.Find("click2");

        iniPos = new Vector2(-15, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(nbCartouche != 0)
        {
            GameObject.Find("nbCartouche").GetComponent<TextMeshPro>().text = nbCartouche.ToString();
        }
        else
        {
            GameObject.Find("nbCartouche").GetComponent<TextMeshPro>().text = "";
        }
        

        switch (Input.touchCount)
        {
            case 0:   //Le joueur ne touche pas l'écran

                clickChange = false;

                break;

            case 1:     //Le joeur a un doigt posé sur l'écran

                if (!clickChange)
                {
                    SuiviTouch(0, click1);
                }
                else
                {
                    SuiviTouch(0, click2);
                }

                break;

           default:     //Le joeur a plusieurs doigts posés sur l'écran

                SuiviTouch(0, click1);
                SuiviTouch(1, click2);

                break;
        }

    }       


    void SuiviTouch(int n, GameObject click)
    {
        Touch touch = Input.GetTouch(n);
        switch (touch.phase)
        {

            case TouchPhase.Began: //le touch prend la valeur de l'endroit où l'on clique

                click.GetComponent<touchFantome>().status = 1;

                var ray = Camera.main.ScreenPointToRay(touch.position); //récupère la position du clic
                startPos = ray.origin + ray.direction;

                click.transform.position = startPos;

                break;

            case TouchPhase.Moved: //le touch suit le doigt

                click.GetComponent<touchFantome>().status = 2;

                var ray2 = Camera.main.ScreenPointToRay(touch.position);
                currentPos = ray2.origin + ray2.direction;

                click.transform.position = currentPos;

                break;

            case TouchPhase.Stationary:

                click.GetComponent<touchFantome>().status = 3;

                var rayS = Camera.main.ScreenPointToRay(touch.position);
                currentPos = rayS.origin + rayS.direction;

                click.transform.position = currentPos;

                break;

            case TouchPhase.Ended:

                click.GetComponent<touchFantome>().status = 4;

                click.transform.position = iniPos;           
                
                if ((Input.touchCount == 2) &&(click == click1))  //si on a deux doigts posés sur l'écran et qu'on soulève le premier doigt posé
                {
                    clickChange = true;
                }else if((Input.touchCount == 2) && (click == click2))
                {
                    clickChange = false;
                }
                else
                {
                    clickChange = false;
                }

                break;
        }
    }
}
