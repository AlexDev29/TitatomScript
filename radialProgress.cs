using UnityEngine;
using UnityEngine.UI;

public class radialProgress : MonoBehaviour
{
    public float time;
    public Text ProgressIndicator;
    public Image LoadingBar;
    void Update()
    {
        if (time >= 0)
        {
            GameObject.Find("LoadingBar").GetComponent<Image>().enabled = true;
            GameObject.Find("Center").GetComponent<Image>().enabled = true;

            float seconds = Mathf.FloorToInt(time % 60);
            LoadingBar.fillAmount = seconds / 10;
            time -=  Time.deltaTime;
        }
        else
        {
            GameObject.Find("LoadingBar").GetComponent<Image>().enabled = false;
            GameObject.Find("Center").GetComponent<Image>().enabled = false;
        }
    }
}
