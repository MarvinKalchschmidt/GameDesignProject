using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs;
        if (this.CompareTag("GameManager"))
        {
            objs = GameObject.FindGameObjectsWithTag("GameManager");
        }   
        else if(this.CompareTag("UI"))
        {
            objs = GameObject.FindGameObjectsWithTag("UI");
        }
        else
        {
            objs = GameObject.FindGameObjectsWithTag("SaveLoadManager");
        }

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}