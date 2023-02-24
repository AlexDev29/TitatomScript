using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFantome : MonoBehaviour
{
    int nb;
    int somme;
    public Vector3 spawnPos;
    List<Vector3> position;
    public GameObject fantR;
    public GameObject fantB;
    public GameObject fantJ;
    public GameObject fantV;
    public GameObject parent;

    GameObject[] fantArray;
    public GameObject[] tabBonus;   //tableau contenant les différents bonus

    public int valApp; //fréquance d'apparition des fantomes
    public int nbApp = 1; //nombre de fanrtomes par apparitions
    int proba;  //probabilité d'avoir un fantome en plus en fonction du score

    int x, y;  //variable pour récupérer une valeur dans la matrice

    int GameModeBonus; //=2 si c'est le mode normal sans fantome noir


    public int[,] matriceApparition =       //matrice de taux d'apparition
    {
        {150, 140, 130, 120, 110},
        {130, 120, 110, 100, 90},
        {110, 100, 90, 80, 70},
        {90, 80, 70, 60, 50},
        {70, 60, 50, 50, 50},
        {50, 50, 50, 50, 50},
      //  {90, 80, 70, 60, 60},
      //  {70, 60, 60, 60, 60},
      //  {60, 60, 60, 60, 60},
    };

    int n;
    int tauxAppBonus = 20; //plus ctte variable est grande, moins il y a de chance d'avoir un bonus
    int valAppBonus = 0;  // si un bonus doit apparaitre, temps en ms après le dernier fantome

    public int speedVariation = 0; //agmentation ou diminution de la vitesse si bonus/malus 
    int appMin = 50;
    public int appVariation;
    public int scoreMax;

    int score;
    int valScore;  //combien de point par fantome ajouté

    public int appBonus;

    // Start is called before the first frame update
    void Start()
    {
        fantArray = new GameObject[] { fantR, fantB, fantJ, fantV };
        appVariation = 3;

        valApp = GetValFromMatrice(score, matriceApparition);   //première ValApp
        valScore = 1;
        
        if(PlayerPrefs.GetInt("mode") == 1)
        {
            GameModeBonus = 2;
        }
        else
        {
            GameModeBonus = 0;
        }
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        score = gameObject.GetComponent<Score>().sc;    //récupère le score


        n += (int)Time.timeScale;

        if ((n >= valApp + speedVariation)&&(n >= appMin))   //speed variation : modifie la vitesse d'apparition des fantomes
        {
            tauxAppBonus = 4;
            /* if(score >= 10)
             {
                 tauxAppBonus = score / 10 * 2;
             }*/
            appBonus = UnityEngine.Random.Range(0, tauxAppBonus);

            if (appBonus == valAppBonus)
            {
                int nb = UnityEngine.Random.Range(1, 100);
                spawnBonus(nb);
                n = 0;
            }
            else
            {
                for (int i = 0; i < appVariation; i++)    //Pour faire apparaitre plusieurs fantomes
                {
                    proba = i * (scoreMax / (score+1));
                    int probaFantomenPlus = UnityEngine.Random.Range(0, Mathf.Abs(proba));
                    if (probaFantomenPlus == 0)
                    {
                         nb = UnityEngine.Random.Range(0, 4);
                         somme += 1;
                    }
                    n = 0;
                }
                spawnFantome(somme, nb);
                somme = 0;
            }
            valApp = GetValFromMatrice(score, matriceApparition);
        }
    }

    
    private int GetValFromMatrice(int score, int[,] mat)
    {

        //int x = nbApp - 1;
        x = score / 50;
        if(score >= 50)
        {
            y = (score / 10) % 5;
        }
        else
        {
            y = score / 10;
        }
        if (x > 5)
        {
            x = 5;
            y = 4;
        }

        return mat[x, y];

    }

    void spawnFantome(int somme, int color)
    {
        switch (somme)
        {
            case 1:
                position =  new List<Vector3>{new (0f, 5.5f, 0f)};
                InstantiateWithPostition(position, color);
                break;
            case 2:
                position =  new List<Vector3>{
                    new (-0.5f, 5.5f, 0f),
                    new (0.5f, 5.5f, 0f)};
                InstantiateWithPostition(position, color);
                break;
            case 3:
                position =  new List<Vector3>{
                    new (-0.8f, 5.5f, 0f),
                    new (0f, 5.5f, 0f),
                    new (0.8f, 5.5f, 0f)
                };
                InstantiateWithPostition(position, color);
                break;
        }
    }

    void InstantiateWithPostition(List<Vector3> position, int color)
    {
        for (var i = 0; i < position.Count; i++)
        {
            Instantiate(fantArray[color], position[i], Quaternion.identity, parent.transform);
        }
    }

    void spawnBonus(int p)
    {
        for (int i = 0; i < tabBonus.Length - GameModeBonus; i++)
        {
            if(p <= tabBonus[i].GetComponent<bonusScript>().proba)
            {
                Instantiate(tabBonus[i], spawnPos, Quaternion.identity);
                break;
            }
        }
    }

    public void SetSpeedVariation(int a)
    {
        speedVariation += a;
    }

    public void setValScore(int a)
    {
        valScore = a;
    }

    public int getValScore()
    {
        return valScore;
    }

}
