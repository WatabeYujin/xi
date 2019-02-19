using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemove : MonoBehaviour {

    public void Move(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
