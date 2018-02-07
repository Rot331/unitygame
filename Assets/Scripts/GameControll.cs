using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameControll : MonoBehaviour {

    public UnityEngine.UI.Text scoreLabel;
    public GameObject winnerLabelObject;

    public void Update()
    {
        int count = GameObject.FindGameObjectsWithTag("Item").Length;
        if (count == 0)
        {
            SceneManager.LoadScene("Clear");
        }
    }
}
