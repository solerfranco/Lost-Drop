using Sirenix.OdinInspector;
using UnityEngine;

public class CustomersQueueManager : MonoBehaviour
{
    public static CustomersQueueManager Instance;

    private Customer[] _customerSlots;

    [Range(1, 5)]
    [SerializeField]
    private int maxCustomers = 1;

    [SceneObjectsOnly]
    [ChildGameObjectsOnly]
    [SerializeField]
    private Transform[] queuePositions;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        _customerSlots = new Customer[maxCustomers];
    }

    public bool IsQueueFull => _customerSlots[maxCustomers - 1] != null;

    public void MoveTheQueueForward()
    {
        for (int i = 1; i < maxCustomers; i++)
        {
            if (_customerSlots[i] != null && _customerSlots[i - 1] == null)
            {
                _customerSlots[i].MoveToPosition(queuePositions[i - 1].position, i-1 == 0, 0);

                _customerSlots[i - 1] = _customerSlots[i];
                _customerSlots[i] = null;

                // Check again from the start in case multiple gaps exist
                i = 0;
            }
        }
    }

    public bool TryFindAvailableQueueSlot(out int availableSlotIndex)
    {
        availableSlotIndex = 0;

        for (int i = 0; i < _customerSlots.Length; i++)
        {
            if (_customerSlots[i] == null) // empty slot
            {
                availableSlotIndex = i;
                return true;
            }
        }
        return false;
    }

    public void AddCustomerToQueueAtIndex(int index, Customer customer)
    {
        if (_customerSlots[index] != null)
        {
            Debug.LogError("You are trying to put 2 customers on the same queue index");
            return;
        }
        _customerSlots[index] = customer;
        customer.MoveToPosition(queuePositions[index].position, index == 0);
    }
    

}
