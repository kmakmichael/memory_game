using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool concealed = true;
    private int card_type = 0;
    [SerializeField]
    private GameObject top;

    public void Flip() {
        concealed = !concealed;
        top.transform.Find("back").gameObject.SetActive(concealed);
        AudioSource asrc = top.GetComponent<AudioSource>();
        asrc.PlayOneShot(asrc.clip);
    }

    public void SetCardType(int ct) {
        card_type = ct;
    }

    public int GetCardType() {
        return card_type;
    }
}