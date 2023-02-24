using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bonusScript : MonoBehaviour
{

    public int n;   //type du bonus indiquant la fonction � utiliser
    public int proba;

    GameObject spawn;

    public int speed;

    bool isEffected = false; //indique si le bonus a �t� utilis� 
    
    private int timeRemaining = 11;

    Sprite bonusSprite;

    // Start is called before the first frame update
    void Start()
    {
        spawn = GameObject.Find("player");
        bonusSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
    }


    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed) * Time.timeScale;
        if((this.transform.position.y <= -10)&&(!isEffected))
        {
            Destroy(this.gameObject);
        }
    }

    // Demarre la progress bar sur le canva
    void DisplayTime(float time)
    {
        GameObject.Find("Center").GetComponent<Image>().sprite = bonusSprite;
        GameObject.Find("ProgressIndicator").GetComponent<radialProgress>().time = time;       
    }

    // Lance l'action en fonction du bonus séléctionné
    public void BonusSelected()
    {
        if (!isEffected)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = null;       //Fait diparaitre le bonus
            if (n == 0)
            {
                MelangePanier();
            }
            else if (n == 1)
            {
                StartCoroutine(SpeedUp());
                
            }
            else if (n == 2)
            {
                StartCoroutine(SpeedDown());
            }
            else if (n == 3)
            {
                AddCartouche();
            }
            else if (n == 4)
            {
                StartCoroutine(Combo());
            }else if(n == 5)
            {
                Bombe();
            }
        }

        isEffected = true;  //empeche le joueur de cliquer deux fois sur le bonus
    }


    void MelangePanier()
    {
        Transform vert = GameObject.Find("PanierVert").transform;
        Vector3 x = vert.transform.position;
        Vector3 y = vert.transform.localScale;

        if(GameObject.Find("PanierVert").transform.position.x != GameObject.Find("PanierRouge").transform.position.x)
        {
            GameObject.Find("PanierVert").transform.localScale = new Vector2(-GameObject.Find("PanierVert").transform.localScale.x, GameObject.Find("PanierVert").transform.localScale.y);
            GameObject.Find("PanierVert").GetComponent<PanContent>().resetRotation();   //repasse la rotation � 0 pour les fantomes dans les paniers
        }
        else
        {
            GameObject.Find("PanierVert").transform.Find("SystemAngle").transform.localPosition = new Vector2(GameObject.Find("PanierVert").transform.Find("SystemAngle").transform.localPosition.x, -GameObject.Find("PanierVert").transform.Find("SystemAngle").transform.localPosition.y);           
        }
        GameObject.Find("PanierVert").transform.position = GameObject.Find("PanierRouge").transform.position; 
        

        if (GameObject.Find("PanierRouge").transform.position.x != GameObject.Find("PanierJaune").transform.position.x)
        {
            GameObject.Find("PanierRouge").GetComponent<PanContent>().resetRotation();   //repasse la rotation � 0 pour les fantomes dans les paniers
            GameObject.Find("PanierRouge").transform.localScale = new Vector2(-GameObject.Find("PanierRouge").transform.localScale.x, GameObject.Find("PanierRouge").transform.localScale.y);
        }
        else
        {
            GameObject.Find("PanierRouge").transform.Find("SystemAngle").transform.localPosition = new Vector2(GameObject.Find("PanierRouge").transform.Find("SystemAngle").transform.localPosition.x, -GameObject.Find("PanierRouge").transform.Find("SystemAngle").transform.localPosition.y);
        }
        GameObject.Find("PanierRouge").transform.position = GameObject.Find("PanierJaune").transform.position;

        if (GameObject.Find("PanierJaune").transform.position.x != GameObject.Find("PanierBleu").transform.position.x)
        {
            GameObject.Find("PanierJaune").transform.localScale = new Vector2(-GameObject.Find("PanierJaune").transform.localScale.x, GameObject.Find("PanierJaune").transform.localScale.y);
            GameObject.Find("PanierJaune").GetComponent<PanContent>().resetRotation();   //repasse la rotation � 0 pour les fantomes dans les paniers
        }
        else
        {
            GameObject.Find("PanierJaune").transform.Find("SystemAngle").transform.localPosition = new Vector2(GameObject.Find("PanierJaune").transform.Find("SystemAngle").transform.localPosition.x, -GameObject.Find("PanierJaune").transform.Find("SystemAngle").transform.localPosition.y);
        }
        GameObject.Find("PanierJaune").transform.position = GameObject.Find("PanierBleu").transform.position;

        if (GameObject.Find("PanierBleu").transform.position.x != x.x)
        {
            GameObject.Find("PanierBleu").transform.localScale = new Vector2(-GameObject.Find("PanierBleu").transform.localScale.x, GameObject.Find("PanierBleu").transform.localScale.y);
            GameObject.Find("PanierBleu").GetComponent<PanContent>().resetRotation();   //repasse la rotation � 0 pour les fantomes dans les paniers
        }
        else
        {
            GameObject.Find("PanierBleu").transform.Find("SystemAngle").transform.localPosition = new Vector2(GameObject.Find("PanierBleu").transform.Find("SystemAngle").transform.localPosition.x, -GameObject.Find("PanierBleu").transform.Find("SystemAngle").transform.localPosition.y);
        }
        GameObject.Find("PanierBleu").transform.position = x;


        Destroy(this.gameObject);

        GameObject[]  objs = GameObject.FindGameObjectsWithTag("fantomeMechant");
        foreach (var obj in objs)
        {
            obj.GetComponent<IAMechant>().isUpdateEnable = true;
        }

    }


    IEnumerator SpeedUp()
    {
        spawn.GetComponent<SpawnFantome>().SetSpeedVariation(-50);
        DisplayTime(timeRemaining);
        yield return new WaitForSeconds (timeRemaining);
        Debug.Log("timeEnd");

        spawn.GetComponent<SpawnFantome>().SetSpeedVariation(50);  //fin de la variation de la vitesse
        Destroy(this.gameObject);

    }

    IEnumerator SpeedDown()
    {
        spawn.GetComponent<SpawnFantome>().SetSpeedVariation(50);
        DisplayTime(timeRemaining);
        yield return new WaitForSeconds(timeRemaining);
        Debug.Log("timeEnd");

        spawn.GetComponent<SpawnFantome>().SetSpeedVariation(-50);  //fin de la variation de la vitesse
        Destroy(this.gameObject);

    }


    void AddCartouche()
    {
        spawn.GetComponent<gestionTouch>().nbCartouche++;
        Destroy(this.gameObject);
    }

    
    IEnumerator Combo()
    {
        spawn.GetComponent<SpawnFantome>().setValScore(spawn.GetComponent<SpawnFantome>().getValScore() * 2);
        spawn.GetComponent<SpawnFantome>().SetSpeedVariation(-80);
        spawn.GetComponent<SpawnFantome>().appVariation = 1;
        DisplayTime(timeRemaining);
        yield return new WaitForSeconds(timeRemaining);
        Debug.Log("timeEnd");

        spawn.GetComponent<SpawnFantome>().setValScore(spawn.GetComponent<SpawnFantome>().getValScore() / 2);
        spawn.GetComponent<SpawnFantome>().SetSpeedVariation(80);
        spawn.GetComponent<SpawnFantome>().appVariation = 3;
        Destroy(this.gameObject);

    }


    void Bombe()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("fantomeMechant");
        foreach (var obj in objs)   //énumère les fantomes obscurs
        {
            Destroy(obj);
        }

        Destroy(this.gameObject);

    }

}
