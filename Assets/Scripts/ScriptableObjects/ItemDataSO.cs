using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemData")]
public class ItemDataSO : ScriptableObject
{
    [Header("Item Properties")]
    public new string name;
    public int weight;
    public MaterialType materialType;
    public MaterialRarity materialRarity;
    public Sprite sprite;
}