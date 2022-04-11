using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool concealed = true;
    [SerializeField]
    private GameObject top;

    public void Flip() {
        concealed = !concealed;
        top.transform.Find("back").gameObject.SetActive(concealed);
        AudioSource asrc = top.GetComponent<AudioSource>();
        asrc.PlayOneShot(asrc.clip);
    }
}