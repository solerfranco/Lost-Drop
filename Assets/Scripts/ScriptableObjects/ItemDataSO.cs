using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemData")]
public class ItemDataSO : ScriptableObject
{
    [Header("Item Properties")]
    public new string name;
    public int weight;

    [PropertySpace(SpaceBefore = 30, SpaceAfter = 30)]
    [EnumToggleButtons]
    public MaterialType materialType;

    [EnumToggleButtons]
    [PropertySpace(SpaceAfter = 30)]
    public MaterialRarity materialRarity;

    [PreviewField]
    public Sprite sprite;
}