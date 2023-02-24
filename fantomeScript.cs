using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class fantomeScript : MonoBehaviour
{
    public int modeDeJeu;
    public bool move = false;
    public bool isCatch = false;
    bool isEnter; //vérifie si le fantome est entré dans la scène
    int n = 30;

    public bool isError = false;

    public string color; //La couleur est définit dans ce script

    public GameObject fantomeMechant;
    private GameObject FMInstantiate;
    public GameObject parent;

    bool ajout = false;

    int valScore;

    public int speed;

    void Start()
    {
        modeDeJeu = PlayerPrefs.GetInt("mode");
    }

    void Update()
    {        

        if (move == false)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed) * Time.timeScale;
        }
        else
        {
            Move();
        }

        valScore = GameObject.Find("player").GetComponent<SpawnFantome>().getValScore();

        if (isEnter)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = !move;    //passe le isTrigger à true uniquement lorsque le fantome est dans le panier
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.gameObject.tag == "Respawn")
        {
            isEnter = true;
        }
        
        if (other.gameObject.tag == "tapis" && !move && !isCatch)
        {
            if (modeDeJeu == 2)
            {
                transformFantome();
            }
            else if (modeDeJeu == 1)
            {
                StartCoroutine(GameObject.Find("player").GetComponent<touchFantome>().LunchLose());
            }
        }

    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "panier"){
              if (other.gameObject.GetComponent<PanContent>().color == color && isCatch)
              {
                //GameObject.Find("player").GetComponent<Score>().sc -= valScore;
                move = false;
              }
              else { isError = false; }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.tag == "panier")
        {
            if (other.gameObject.GetComponent<PanContent>().color == color)
            {
                if (ajout == false && !isCatch)
                {    //accede seulement à la première case NonReorderableAttribute rempli
                    GameObject.Find("player").GetComponent<Score>().sc += valScore;
                    List<GameObject> listFant = other.gameObject.GetComponent<PanContent>().Pan;
                    listFant.Add(gameObject);      //ajoute le fantome au tableau du panier
                    gameObject.transform.parent = other.gameObject.transform;    //définit le panier du fantome comme étant son parent
                    ajout = true;
                }
                isError = false;
            }
        }
        //active la funtion Move si c'est le bon panier
        if ((other.gameObject.tag == "panier") && (other.gameObject.GetComponent<PanContent>().color == color))
        {
            move = true;
        }

        if ((other.gameObject.tag == "panier") && (other.gameObject.GetComponent<PanContent>().color != color))
        {
            isError = true;
        }
    }


    public void transformFantome()
    {
        Destroy(gameObject);
        parent = GameObject.Find("NavMesh2D");
        
        FMInstantiate = Instantiate(fantomeMechant, transform.position, transform.rotation, parent.transform);
        FMInstantiate.GetComponent<IAMechant>().color = color; //Donne la couleur du fantome obscur
    }
    
    void Move()
    {    //pour chaque n=30 le fantome change de direction
        if (move == true)
        {
            n += 1;
            if (n > 50)
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2) * Time.timeScale);
                n = 0;
            }
        }
    }
}