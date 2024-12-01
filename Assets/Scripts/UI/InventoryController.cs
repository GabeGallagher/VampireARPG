using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Transform itemStash;
    [SerializeField] private Transform equipped;
    [SerializeField] private Transform rightHand, leftHand;
    [SerializeField] private InputController inputController;

    private List<ItemSO> itemsList = new List<ItemSO>();
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;
    private WeaponSO mainHand;

    public WeaponSO MainHand { get => mainHand; }

    private void Awake()
    {
        graphicRaycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();

        gameObject.SetActive(false);
    }
    private void Start()
    {
        inputController.OnEquipPerformed += InputController_OnEquipPerformed;
    }

    private void InputController_OnEquipPerformed(object sender, System.EventArgs e)
    {
        HandleItemClick();
    }

    public void AddItem(ItemSO itemSO)
    {
        itemsList.Add(itemSO);

        UpdateStash();
    }

    public void RemoveItem(ItemSO itemSO)
    {
        itemsList.Remove(itemSO);

        UpdateStash();
    }

    private void UpdateStash()
    {
        foreach (ItemSO itemSO in itemsList)
        {
            for (int i = 0; i < itemStash.childCount; i++)
            {
                for (int j= 0; j < itemStash.GetChild(i).childCount; j++)
                {
                    Transform itemSlotTransform = itemStash.GetChild(i).GetChild(j);

                    ItemSlot itemSlot = itemSlotTransform.GetComponent<ItemSlot>();

                    if (itemSlot.ItemSO == null)
                    {
                        itemSlot.ItemSO = itemSO;
                        i = itemStash.childCount;
                        break;
                    }
                }
            }
        }
    }

    private void HandleItemClick()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem)
        {
            position = Mouse.current.position.ReadValue()
        };
        List<RaycastResult> results = new List<RaycastResult>();

        graphicRaycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            ItemSlot itemSlot = result.gameObject.GetComponent<ItemSlot>();

            if (itemSlot != null)
            {
                itemSlot.ClearSprite();

                EquipItem(itemSlot.ItemSO); break;
            }
        }
    }

    private void EquipItem(ItemSO itemSO)
    {
        Debug.Log($"Clicked on {itemSO.ItemName}");

        switch (itemSO.ItemType)
        {
            case ItemSO.EItemType.Weapon:
                ItemSlot mainHandSlot = equipped.Find("MainHand").GetComponent<ItemSlot>();
                GameObject weapon = Instantiate(itemSO.Prefab, rightHand);
                mainHandSlot.ItemSO = itemSO;
                mainHand = (WeaponSO)itemSO;

                // position coords are contained in the weapon folder in Prefabs/Items
                Vector3 localPosition = new Vector3(0.1294f, 0.0179f, -0.0453f);
                Quaternion localRotation = Quaternion.Euler(15.177f, -106.1f, 101.719f);
                weapon.transform.localPosition = localPosition;
                weapon.transform.localRotation = localRotation;

                RemoveItem(itemSO);
                break;

            default:
                Debug.LogError($"{itemSO.ItemType} not found");
                break;
        }
    }
}
