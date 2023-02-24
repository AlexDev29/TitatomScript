using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class IAMechant : MonoBehaviour
{

    public string color;
    public GameObject panier;
    public GameObject[] listPanier;
    public GameObject endMarkerObject;
    public Transform endMarker;
    private NavMeshAgent agent;
    public Vector3 final;
    private float distFinal;
    float dist = 10000F;
    public bool isUpdateEnable;
    void Start()
    {
        listPanier = GameObject.FindGameObjectsWithTag("panier");
        panier = FindPanierByColor(color);
        
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        isUpdateEnable = true;

    }
	void Update()
    {
        if(isUpdateEnable)
        { 
        endMarkerObject = FindSecurity(panier.GetComponent<PanContent>().Sys);
            if (endMarkerObject != null)
            {
                endMarker = endMarkerObject.GetComponent<Transform>();
                final = endMarker.position;
                final.z = 0;
                agent.SetDestination(final);
            }
        }
    }
    
    
    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "systeme") && (other.gameObject.GetComponent<DefensePanier>().color == color))
        {
            other.gameObject.GetComponent<DefensePanier>().pointDeVie -= Time.timeScale;
            if (other.gameObject.GetComponent<DefensePanier>().pointDeVie > 0)
            {
                isUpdateEnable = false;  
            }
            else
            {
                isUpdateEnable = true;  
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "systeme") && (other.gameObject.GetComponent<DefensePanier>().color == color))
        {
            if (other.gameObject.GetComponent<DefensePanier>().pointDeVie > 0)
            {
                isUpdateEnable = false;  
            }
            else
            {
                isUpdateEnable = true;  
            }
        }
    }

    GameObject FindSecurity(GameObject[] tab)
    {
        GameObject security;
        int nbrEnnemyMin2;
        int nbrEnnemyMin = 10000;
        List<GameObject> list1 = new List<GameObject>();
        List<GameObject> list2 = new List<GameObject>();
        list1 = FindNotDestroySecurity(tab);
        if (SameNbrEnnemy(list1))
        {
            security = FindDistanceMin(list1, dist);
        }
        else
        {
            nbrEnnemyMin2 = FindNbrEnnemyMin(list1, nbrEnnemyMin);
            list2 = FindSecurityWithSameNbrEnnemy(list1, nbrEnnemyMin2);
            security = FindDistanceMin(list2, dist);
        }
        return security;
    }

    List<GameObject> FindNotDestroySecurity(GameObject[] tab)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < tab.Length; i++)
        {
            if (tab[i].GetComponent<DefensePanier>().pointDeVie > 0)
            {
                list.Add(tab[i]);
            }
        }

        return list;
    }

    Boolean SameNbrEnnemy(List<GameObject> tab)
    {
        for (int i = 0; i < tab.Count - 1; i++)
        {
            if (tab[i].GetComponent<DefensePanier>().nbrEnnemy !=
                tab[i + 1].GetComponent<DefensePanier>().nbrEnnemy)
            {
                return false;
            }
        }
        return true;
    }

    GameObject FindDistanceMin(List<GameObject> tab, float distMin)
    {
        GameObject result = null;
        float dist;
        for (int i = 0; i < tab.Count; i++)
        {
            dist = Vector3.Distance(gameObject.transform.position, tab[i].transform.position);
            if (dist < distMin)
            {
                distMin = dist;
                result = tab[i];
            }
        }
            
        return result;
    }

    int FindNbrEnnemyMin(List<GameObject> tab, int nbrEnnemyMin)
    {
        int nbrEnnemy;
        for (int i = 0; i < tab.Count; i++)
        {
            nbrEnnemy = tab[i].GetComponent<DefensePanier>().nbrEnnemy;
            if ( nbrEnnemy < nbrEnnemyMin)
            {
                nbrEnnemyMin = nbrEnnemy;
            }
        }
        return nbrEnnemyMin;
    }

    List<GameObject> FindSecurityWithSameNbrEnnemy(List<GameObject> tab, int nbrEnnemyMin)
    {
        List<GameObject> list = new List<GameObject>();
        int nbrEnnemy;
        for (int i = 0; i < tab.Count; i++)
        {
            nbrEnnemy = tab[i].GetComponent<DefensePanier>().nbrEnnemy;
            if (nbrEnnemy == nbrEnnemyMin)
            {
                list.Add(tab[i]);
            }
        }
        return list;
    }
    
    
    GameObject FindPanierByColor(string color) {
        GameObject result = null;
        for (int i = 0; i < listPanier.Length; i++)
        {
            if (listPanier[i].GetComponent<PanContent>().color == color) {
                result = listPanier[i];
                break;
            }
        }
        return result;
    }
}
