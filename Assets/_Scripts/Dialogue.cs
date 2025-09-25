using UnityEngine;

[System.Serializable] // Dòng này giúp Dialogue có thể hiển thị trong Inspector
public class Dialogue
{
    public string name; // Tên của NPC

    [TextArea(3, 10)] // Giúp ô nhập liệu trong Inspector lớn hơn, dễ gõ
    public string[] sentences; // Mảng chứa các câu thoại
}