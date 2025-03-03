using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static string _name = "CoroutineRunner";

    private static CoroutineRunner _instance;
    
    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject runner = new GameObject(_name);
                _instance = runner.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(runner);
            }
            return _instance;
        }
    }
}
