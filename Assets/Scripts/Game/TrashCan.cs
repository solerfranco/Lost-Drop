using DG.Tweening;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField]
    private Item itemPrefab;

    [SerializeField]
    private Transform spawnPoint;

    public void SpawnItem()
    {
        Item item = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);

        item.DisableCollider();
        item.transform.DOLocalMoveX(item.transform.localPosition.x + Random.Range(-3f, 3f), .75f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            item.EnableCollider();
        });
        item.transform.DOLocalMoveY(item.transform.localPosition.y + 2, 0.25f).SetEase(Ease.OutQuad);
        item.transform.DOLocalMoveY(item.transform.localPosition.y - Random.Range(3.5f, 6f), 1f).SetDelay(0.25f).SetEase(Ease.InQuad).SetEase(Ease.OutBounce);
    }
}
