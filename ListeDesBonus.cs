using UnityEngine;
using UnityEngine.SceneManagement;

public class ListeDesBonus : MonoBehaviour{

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
