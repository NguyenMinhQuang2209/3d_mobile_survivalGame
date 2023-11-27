using UnityEngine;

public class MessageController : MonoBehaviour
{
    //UI type
    public static string OPEN_BOX = "box";
    public static string OPEN_INVENTORY = "inventory";

    //Success
    public static string COLLECTING_MESSAGE = "Có thể thu hoạch!";


    //Fail
    public static string INVENTORY_FULL = "Túi đồ đã đầy!";
    public static string QUANTITY_ITEM_END = "Túi đồ đã hết món này!";
    public static string OUT_OF_WATER = "Hết nước!";
}
