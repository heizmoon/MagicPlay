using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatisticLine : MonoBehaviour
{
    // Start is called before the first frame update
    public Text _name;
    public Text _damage;
    public Text _percent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetText(string a,string b,string c)
    {
        _name.text =a;
        _damage.text =b;
        _percent.text =c;
    }
}
