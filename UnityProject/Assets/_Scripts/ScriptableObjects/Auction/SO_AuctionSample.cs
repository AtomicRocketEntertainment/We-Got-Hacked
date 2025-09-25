using UnityEngine;


[CreateAssetMenu(fileName = "Auction Sample", menuName = "Scriptable Objcts/Objects Info/Auction Sample")]
public class SO_AuctionSample : ScriptableObject
{
    private const int _quantity = 5;
    public bool IsCorrect;
    public int LastBid;
    public int BidMadeQuantity;
    public int BidViews;
    public string[] LineHeader = new string[_quantity];
    public string[] LineOne = new string[_quantity];
    public string[] LineTwo = new string[_quantity];
    public string[] LineThree = new string[_quantity];
    public string[] LineFour = new string[_quantity];    
}
