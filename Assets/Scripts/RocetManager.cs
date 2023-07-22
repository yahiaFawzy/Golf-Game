using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocetManager : MonoBehaviour
{
    [SerializeField] Button button0, button20, button40;
    
    public Rocet rocet;

    public static RocetManager Instance;

    private void Awake()
    {
        Instance = this;
        UseRocet(Rocet.Rocet20, button20);
    }

    private void Start()
    {
        button0.onClick.AddListener(()=> { UseRocet(Rocet.Rocet0,  button0);  });
        button20.onClick.AddListener(()=> { UseRocet(Rocet.Rocet20,button20); });
        button40.onClick.AddListener(()=> { UseRocet(Rocet.Rocet40,button40); });
    }

    public void UseRocet(Rocet rocet, Button button) {
        this.rocet = rocet;
        button0.transform.localScale = new Vector3(1, 1, 1);
        button20.transform.localScale = new Vector3(1, 1, 1);
        button40.transform.localScale = new Vector3(1, 1, 1);
        button.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        
    }

 

}

public enum Rocet{ 
 Rocet0 = 0,Rocet20 = 40,Rocet40=70
}