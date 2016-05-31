using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour {        

	public void LoadLevel(int _level)
    {
        if (_level <0 || _level >= SceneManager.sceneCount)
        {
            SceneManager.LoadScene(_level);
        }
    }
    public void LoadLevel(string _level)
    {
        if (SceneManager.GetSceneByName(_level) != null)
        {
            SceneManager.LoadScene(_level);
        }
    }


}
