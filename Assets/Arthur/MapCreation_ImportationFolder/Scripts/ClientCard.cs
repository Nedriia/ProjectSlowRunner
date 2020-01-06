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
    public HorizontalLayoutGroup likeable;
    public HorizontalLayoutGroup hated;
    public Text speakable;
    public Image starsDisplay;

    public GameObject textPrefab;

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

    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<GameManager>();
        //Random client
        //currentClient = manager.currentClient;

        NewClient(currentClient);

        Actualisation_StarClient(currentClient, starsDisplay);

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
            canvas.SetActive(true);
            /*if (manager.managerScore.enabled == false)
            {
                manager.managerScore.enabled = true;
            }*/
        }

        /*if (currentClient != manager.currentClient)
        {
            currentClient = manager.currentClient;
            NewClient_InGame(currentClient);
        }*/
    }

    public void NewClient(CustomerData client)
    {
        card.SetActive(true);

        name.text = client.Name;
        clientImage.sprite = client.Face;
        maxProfitText.text = client.MaxGainMoney.ToString();
        estimateText.text = client.BaseGainMoney.ToString();

        baseMutator.text = client.Mutators[0].ToString();
        foreach (TasteCustomer mutator in client.TasteLiked)
        {
            var text = Instantiate(textPrefab, likeable.transform);
            text.GetComponent<Text>().text = mutator.ToString();
        }
        foreach (TasteCustomer mutator in client.TasteHated)
        {
            var text = Instantiate(textPrefab, hated.transform);
            text.GetComponent<Text>().text = mutator.ToString();
        }

        speakable.text = client.Languages[0].ToString();
        clientSpriteCanvas.sprite = client.Face;

        Time.timeScale = 0;
    }

    public void Actualisation_StarClient(CustomerData Client, Image display)
    {
        switch (Client.NumberOfStars)
        {
            case 1:
                display.sprite = one_star;
                break;
            case 2:
                display.sprite = two_star;
                break;
            case 3:
                display.sprite = three_star;
                break;
            case 4:
                display.sprite = four_star;
                break;
            case 5:
                display.sprite = five_star;
                break;
            default:
                break;
        }
    }

    public void NewClient_InGame(CustomerData client)
    {
        clientImage.sprite = client.Face;
    }
}
