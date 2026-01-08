using System.Collections;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetMinigame : Minigame
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private InputAction inputAction;

    private static readonly WaitForSeconds _waitForSeconds0_75 = new(0.75f);

    [SerializeField]
    [MinMaxSlider(-10, 10, true)]
    private Vector2 XRangeMinMax = new Vector2(0, 0);

    [SerializeField]
    [MinMaxSlider(-10, 10, true)]
    private Vector2 YRangeMinMax = new Vector2(0, 0);

    [SerializeField]
    private GameObject targetPrefab;

    private Vector2 randomPosition = new();

    [SerializeField]
    private AudioClip comboSFX;

    private int combo;

    private int targetsHit;

    private void Awake()
    {
        inputAction.Enable();
        inputAction.performed += CheckHit;
    }

    void OnEnable()
    {
        Reset();
        StartCoroutine(Play());
    }

    public override void Reset()
    {
        combo = 0;
        targetsHit = 0;
    }

    private void OnDisable()
    {
        inputAction.performed -= CheckHit;
        inputAction.Disable();
    }

    public override IEnumerator Play()
    {
        yield return new WaitForSeconds(0.5f);
        while (targetsHit < 4)
        {
            Vector3 newRandomPosition = GetRandomPosition();
            while(Vector2.Distance(randomPosition, newRandomPosition) < 1.5f)
            {
                newRandomPosition = GetRandomPosition();
            }

            randomPosition = newRandomPosition;
            Instantiate(targetPrefab, transform.position + newRandomPosition, Quaternion.identity);
            yield return _waitForSeconds0_75;
        }
        // Spawn bonus one

        yield return new WaitForSeconds(0.75f);
        OnGameFinished?.Invoke(true);
    }

    private void CheckHit(InputAction.CallbackContext context)
    {
        // Read mouse position from the new Input System
        Vector2 mouseScreenPos = Pointer.current.position.ReadValue();

        // Convert screen position to world position
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        // Raycast at that position (point raycast)
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            mouseWorldPos,
            Vector2.zero
        );

        TargetPrefab target = null;
        float distanceToTarget = 10;
        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.collider.TryGetComponent(out TargetPrefab currentTarget)) continue;
            float distance = Vector2.Distance(hit.collider.transform.position, mouseWorldPos);
            if(distance < distanceToTarget)
            {
                target = currentTarget;
                distanceToTarget = distance;
            }
        }
        if(target != null)
        {
            if (target.Hit())
            {
                targetsHit++;
                combo++;
                MMSoundManager.Instance.PlaySound(comboSFX, MMSoundManager.MMSoundManagerTracks.Sfx, Vector3.zero, false, 1, 0, false, 0, 1, null, false, null, null, 1.2f + (0.1f * combo));
            }
            else
            {
                combo = 0;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(XRangeMinMax.x, XRangeMinMax.y), Random.Range(YRangeMinMax.x, YRangeMinMax.y));
    }
}
