using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private SpriteRenderer cardIImage; 
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private TMP_Text cardEnergyCost;

    private Card card;

    void Start()
    {

    }

    public void Setup(Card card)
    {
        this.card = card;
        cardIImage.sprite = card.Sprite;
        cardName.text = card.Name;
        cardDescription.text = card.Description;
        cardEnergyCost.text = card.EnergyCost.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // card.PerformEffect(); DONT
    }
}
