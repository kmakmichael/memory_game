using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightGame : MonoBehaviour
{
    private int difficulty = 3; // start at 1 or 2, max 15s
    private string seq = "", guess = "";
    private float h, w, p;
    [SerializeField]
    private RectTransform seq_disp;
    [SerializeField]
    private GameObject button_pad;
    [SerializeField]
    public GameObject blk_red, blk_green, blk_blue, blk_yellow, blk_none;
    [SerializeField]
    private GameObject lossbox;
    [SerializeField]
    private AudioSource jingle;

    void Start() {
        lossbox.SetActive(false);
        Canvas.ForceUpdateCanvases();
        Setup();
    }

    private void Setup() {
        Cleanup();
        seq = SeqGen(difficulty);
        guess = "";
        p = seq_disp.rect.height * .15f;
        h = seq_disp.rect.height - 2*p;
        w = ((seq_disp.rect.width-p)/seq.Length) - p;
        w = w > h ? h : w;
        StartCoroutine("ShowSeq");
    }

    private IEnumerator ShowSeq() {
        yield return new WaitForSeconds(1.0f);
        DisableButtons();
        for (int i = 0; i < seq.Length; i++) {
            guess += seq[i];
            AddColorBlock(seq[i]);
            yield return new WaitForSeconds(0.70f);
        }
        float waitTime = difficulty * 0.4f;
        yield return new WaitForSeconds(waitTime < 2.0f ? 2.0f : waitTime);
        guess = "";
        EnableButtons();
        Cleanup();
    }

    private void Cleanup() {
        foreach (Transform child in seq_disp.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }


    private void AddColorBlock(char c) {
        GameObject blk = Instantiate(ColorMux(c), seq_disp);
        blk.name = "blk_" + (guess.Length-1);
        float x1 = (guess.Length-1)*(p+w) + p;
        RectTransform r = blk.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(0.0f, 0.5f);
        r.anchorMax = new Vector2(0.0f, 0.5f);
        r.pivot = new Vector2(0.0f, 0.5f);
        r.sizeDelta = new Vector2(w, h);
        r.anchoredPosition = new Vector3(x1, 0, 0);
        r.transform.localScale = Vector3.one;
        AudioSource asrc = blk.GetComponent<AudioSource>();
        asrc.Play();
    }

    private GameObject ColorMux(char c) {
        switch(c) {
            case 'r':
                return blk_red;
            case 'g':
                return blk_green;
            case 'b':
                return blk_blue;
            case 'y':
                return blk_yellow;
            default:
                return blk_none;
        }
    }


    public void ColorButton(string c) {
        guess += c[0];
        AddColorBlock(c[0]);
        CheckGuess();
    }

    private string SeqGen(int len) {
        string com = "";
        for (int i = 0; i < difficulty; i++) {
            com += RandomColor();
        }
        return com;
    }

    private char RandomColor() {
        int i = Random.Range(0,4);
        switch (i) {
            case 0: return 'r';
            case 1: return 'g';
            case 2: return 'b';
            case 3: return 'y';
            default: return 'n';
        }
    }

    private void EnableButtons() {
        var buttons = button_pad.transform.GetComponentsInChildren<Button>();
        foreach (Button b in buttons) {
            b.interactable = true;
        } 
    }

    private void DisableButtons() {
        var buttons = button_pad.transform.GetComponentsInChildren<Button>();
        foreach (Button b in buttons) {
            b.interactable = false;
        }
    }

    private void CheckGuess() {
        if (guess.Length == seq.Length) {
            if (guess.Equals(seq)) {
                DisableButtons();
                ++difficulty;
                StartCoroutine("WinRound");
            } else {
                lossbox.SetActive(true);
            }
        } else {
            if (!guess.Equals(seq.Substring(0,guess.Length))) {
                lossbox.SetActive(true);
            }
        }
    }

    private IEnumerator WinRound() {
        yield return new WaitForSeconds(0.75f);
        jingle.PlayOneShot(jingle.clip);
        Setup();
    }
}
