using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientCard : MonoBehaviour
{
    [Header("Client")]
    public CustomerData[] client;
    public CustomerData currentClient;

    [Header("Data To Fill")]
    public Image clientImage;
    public Text name;
    public Text maxProfitText;
    public Text estimateText;
    public Text baseMutator;
    public Text likeable;
    public Text hated;
    public Text speakable;
    public Image starsDisplay;

    [Header("GameObject Card")]
    public GameObject card;

    [Header("Sprite stars")]
    public Sprite one_star;
    public Sprite two_star;
    public Sprite three_star;
    public Sprite four_star;
    public Sprite five_star;

    [Header("Canvas")]
    public GameObject canvas;
    public Image clientSpriteCanvas;


    // Start is called before the first frame update
    void Awake()
    {
        int clientNum = Random.Range(0, client.Length);
        currentClient = client[clientNum];
        name.text = currentClient.Name;
        clientImage.sprite = currentClient.Face;
        maxProfitText.text = currentClient.MaxGainMoney.ToString();
        estimateText.text = currentClient.BaseGainMoney.ToString();
        baseMutator.text = currentClient.Mutators[0].ToString();
        likeable.text = currentClient.TasteLiked[0].ToString();
        hated.text = currentClient.TasteHated[0].ToString();
        speakable.text = currentClient.Languages[0].ToString();
        clientSpriteCanvas.sprite = currentClient.Face;

        switch (currentClient.NumberOfStars)
        {
            case 1:
                starsDisplay.sprite = one_star;
                break;
            case 2:
                starsDisplay.sprite = two_star;
                break;
            case 3:
                starsDisplay.sprite = three_star;
                break;
            case 4:
                starsDisplay.sprite = four_star;
                break;
            default:
                break;
        }

        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            card.SetActive(false);
            canvas.SetActive(true);
            Time.timeScale = 1;
        }
    }
}
