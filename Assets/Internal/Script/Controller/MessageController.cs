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
    public static string INVENTORY_REMOVE_ERROR = "Lỗi xóa sản phẩm khỏi túi đồ!";
    public static string QUANTITY_ITEM_END = "Túi đồ đã hết món này!";
    public static string OUT_OF_WATER = "Hết nước!";

    // Fail in building
    public static string BUILDING_ERROR = "Không thể đặt vật phẩm tại đây!";


    //Following animal mode
    public static string PROTECT = "PROTECT";
    public static string ATTACK = "ATTACK";

    // Coin
    public static string LACK_OF_COIN = "Thiếu coin!";
}
