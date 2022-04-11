using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGame : MonoBehaviour
{
    enum card_type {
    }
    private int difficulty = 2;
    private card_type[,] board;
    private (int,int) blen = (4,4);
    private GameObject active;
    [SerializeField]
    private GameObject endcard;
    [SerializeField]
    private GameObject board_cover;
    [SerializeField]
    private RectTransform game_box;
    [SerializeField]
    private AudioSource jingle;

    [SerializeField]
    private GameObject blank_card;

    void Start() {
        endcard.SetActive(false);
        Canvas.ForceUpdateCanvases();
        Setup();
    }

    private void Setup() {
        Cleanup();
        FillBoard();
        StartCoroutine("PlaceCards");
    }

    private void FillBoard() {
        board = new card_type[blen.Item1,blen.Item2];
        for (int y = 0; y < blen.Item1; y++) {
            for (int x = 0; x < blen.Item2; x++) {
                board[y,x] = RandomCard();
            }
        }
    }

    private IEnumerator PlaceCards() {
        board_cover.SetActive(true);
        for (int y = 0; y < blen.Item1; y++) {
            for (int x = 0; x < blen.Item2; x++) {
                GameObject c = CreateCard(board[y,x],(y,x));
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        board_cover.SetActive(false);
    }

    private GameObject CreateCard(card_type ctype, (int,int) pos) {
        float p;
        if (blen.Item1 > blen.Item2) {
            p = 0.2f / blen.Item1;
        } else {
            p = 0.2f / blen.Item2;
        }
        float o_h = (1.0f-p) / blen.Item2;
        float o_w = (1.0f-p) / blen.Item1;
        float c_h = o_h - p;
        float c_w = o_w - p;
        float x1 = p + o_w * pos.Item2;
        float y1 = p + o_w * pos.Item1;
        float dim = c_w > c_h ? c_h : c_w;

        GameObject c = Instantiate(blank_card, game_box.transform);
        c.name = "card_" + pos.Item1 + "-" + pos.Item2;
        RectTransform r = c.GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(dim, dim);
        r.anchorMax = new Vector2(x1+dim,y1+dim);
        r.anchorMin = new Vector2(x1,y1);
        Button b = c.transform.Find("back").gameObject.GetComponent<Button>();
        Card scr = c.GetComponent<Card>();
        scr.SetCardType((int)ctype);
        b.onClick.AddListener(delegate {ClickCard(c, pos); });
        return c;
    }

    private void ClickCard(GameObject c, (int,int) pos) {
        Debug.Log("card type is " + c.GetComponent<Card>().GetCardType());
        if (active != null) {
            Debug.Log("yo?");
            Card scr_a = active.GetComponent<Card>();
            Card scr_b = c.GetComponent<Card>();
            if (scr_a.GetCardType() == scr_b.GetCardType()) {
                Debug.Log("yo!");
            } else {
                scr_a.Flip();
                scr_b.Flip();
            }
            active = null;
        } else {
            Debug.Log("yo...");
            active = c;
        }
    }


    private void Cleanup() {
        foreach (Transform child in game_box.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void SeqGen(int len) {
        /*string com = "";
        for (int i = 0; i < difficulty; i++) {
            com += RandomCard();
        }
        return com;*/
    }

    private card_type RandomCard() {
        int i = Random.Range(0,System.Enum.GetNames(typeof(card_type)).Length);
        return (card_type)i;
    }


    private void CheckGuess() {
        /*if (guess.Length == seq.Length) {
            if (guess.Equals(seq)) {
                DisableButtons();
                ++difficulty;
                StartCoroutine("PlaceCards");
            } else {
                endcard.SetActive(true);
            }
        } else {
            if (!guess.Equals(seq.Substring(0,guess.Length))) {
                endcard.SetActive(true);
            }
        }*/
    }

    private IEnumerator WinRound() {
        yield return new WaitForSeconds(0.75f);
        // jingle.PlayOneShot(jingle.clip);
        Setup();
    }
}
