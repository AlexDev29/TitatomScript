using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class touchFantome : MonoBehaviour
{
    int modeDeJeu;

    public int status = 0;     //0 = nul --- 1 = begin --- 2 = move --- 3 =  end

    Vector2 iniPos;
    Vector2 startPos;
    Vector2 currentPos;

    public bool clicFantome = false; //verif si clic sur un fantome
    public bool release = true; // verif si lache
    GameObject fantom;

    bool exploded = false;
    bool mechantTouch = false;

    public bool useMouse;  //est à true pour l'objet touch uniquement

    // Start is called before the first frame update
    void Start()
    {
        modeDeJeu = PlayerPrefs.GetInt("mode");

        iniPos = new Vector2(-15, 0);

        status = -1; //si le status ne change pas alors on est pas en mode tactile
    }

    void refreshTouch()
    {
        transform.position = iniPos;
        clicFantome = false;
        release = true;
        fantom = null;
        exploded = false;

        status = 0;
    }

    // Update is called once per frame
    void Update()
    {

            switch (status)         // gerer depuis touch => gestion touch
            {
                case 1:

                    if ((clicFantome)&& (fantom != null))
                    {
                        fantom.transform.position = transform.position;
                    }

                    release = false;

                    break;

                case 2:

                    if ((clicFantome)&&(fantom != null))
                    {
                        fantom.transform.position = transform.position;
                    }
                    exploded = true;    //si l'objet est en mouvement, alors la cartouche ne fonctionne pas 

                    if (transform.position.x < -9.5 || transform.position.x > 9.5 || transform.position.y > 4 || transform.position.y < -3.5)   //Le jouer sort de la zone
                    {
                    if ((clicFantome)&&(fantom != null))
                    {
                        if (fantom.GetComponent<fantomeScript>().isError)
                        {
                            StartCoroutine(LunchLose());
                        }

                        if ((!fantom.GetComponent<fantomeScript>().move) && ((fantom.transform.position.x < -1) || (fantom.transform.position.x > 1)))      //si le fantome est laché hors du tapis
                        {
                            if (modeDeJeu == 1)
                            {
                                StartCoroutine(LunchLose());
                            }
                            else
                            {
                                fantom.GetComponent<fantomeScript>().transformFantome();
                                fantom = null;
                            }
                        }
                        if (fantom != null) fantom.GetComponent<fantomeScript>().isCatch = false;
                    }

                    refreshTouch();

                    }

                    break;

                case 3:

                    if ((clicFantome) && (fantom != null))
                    {
                        fantom.transform.position = transform.position;
                    }

                if (transform.position.x < -9.5 || transform.position.x > 9.5 || transform.position.y > 4 || transform.position.y < -3.5)   //Le jouer sort de la zone
                {
                    if ((clicFantome)&&(fantom != null))
                    {
                        if (fantom.GetComponent<fantomeScript>().isError)
                        {
                            StartCoroutine(LunchLose());
                        }

                        if ((!fantom.GetComponent<fantomeScript>().move) && ((fantom.transform.position.x < -1) || (fantom.transform.position.x > 1)))      //si le fantome est laché hors du tapis
                        {
                            if (modeDeJeu == 1)
                            {
                                StartCoroutine(LunchLose());
                            }
                            else
                            {
                                fantom.GetComponent<fantomeScript>().transformFantome();
                                fantom = null;
                            }
                        }
                        if (fantom != null) fantom.GetComponent<fantomeScript>().isCatch = false;

                    }

                    refreshTouch();

                }

                break;

                case 4:

                    if ((clicFantome)&&(fantom != null))
                    {
                        if (fantom.GetComponent<fantomeScript>().isError)
                        {
                            StartCoroutine(LunchLose());
                        }

                        if ((!fantom.GetComponent<fantomeScript>().move) && ((fantom.transform.position.x < -1) || (fantom.transform.position.x > 1)))      //si le fantome est laché hors du tapis
                        {
                            if (modeDeJeu == 1)
                            {
                                StartCoroutine(LunchLose());
                            }
                            else
                            {
                            fantom.GetComponent<fantomeScript>().transformFantome();
                                fantom = null;
                            }
                        }
                        if(fantom != null) fantom.GetComponent<fantomeScript>().isCatch = false;
                    }

                refreshTouch();

                    break;
            }



            if (mechantTouch)   //si le joueur a utilisé une cartourhce
            {
                    GameObject.Find("player").GetComponent<gestionTouch>().nbCartouche--;
                    mechantTouch = false;
            }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "fantome") && (other.gameObject.GetComponent<fantomeScript>().move == false) && (fantom == null))          //si le joueur touche un fantome pas emprisoné, et qu'il en a pas déjà attrapé un
        {
            clicFantome = true;
            fantom = other.gameObject;
            fantom.GetComponent<fantomeScript>().isCatch = true;
        }

        if ((other.gameObject.tag == "bonus") && !exploded)         //si le joueur touche un bonus non utilisé
        {
            other.gameObject.GetComponent<bonusScript>().BonusSelected();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       if ((other.gameObject.tag == "fantomeMechant") && (GameObject.Find("player").GetComponent<gestionTouch>().nbCartouche > 0) && !release && !exploded)        //si le joueur veut utiliser une cartouche
       {
            Destroy(other.gameObject);
            mechantTouch = true;
        }
    }

    public IEnumerator LunchLose()
    {
        yield return new WaitForSeconds(1);
        PlayerPrefs.SetInt("deadCount", PlayerPrefs.GetInt("deadCount")+1);
        Debug.Log(PlayerPrefs.GetInt("deadCount"));
        SceneManager.LoadScene("menuLose");
    }
}
