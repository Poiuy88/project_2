using UnityEngine;

// Dòng này rất quan trọng, nó cho phép bạn tạo "Công thức" trong menu Create của Unity
[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Upgrade Recipe")] 
public class UpgradeRecipe : ScriptableObject 
{ // <<< Mở đầu class UpgradeRecipe

    // --- Các thông tin cần thiết cho một công thức nâng cấp ---

    public ItemData baseItem;   // Trang bị gốc cần để nâng cấp
    public ItemData resultItem; // Trang bị sẽ nhận được sau khi nâng cấp

    [Header("Requirements")]    // Tiêu đề trong Inspector cho dễ nhìn
    public int requiredLevel = 5; // Cấp độ yêu cầu của người chơi
    public int cost = 200;        // Số tiền (xu) cần để nâng cấp

}