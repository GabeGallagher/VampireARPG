using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Transform itemStash, equipped, rightHandSockets, leftHandSockets;
    [SerializeField] private InputController inputController;

    private List<ItemData> itemsList = new List<ItemData>();
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;
    private WeaponData mainHand;
    private PlayerController player;
    private Transform rightHand, leftHand;

    public WeaponData MainHand => mainHand;

    private void Awake()
    {
        graphicRaycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
        rightHand = rightHandSockets.parent;
        leftHand = leftHandSockets.parent;
    }

    private void Start()
    {
        inputController.OnEquipPerformed += InputController_OnEquipPerformed;
        player = FindAnyObjectByType<PlayerController>();
        gameObject.SetActive(false);
    }

    private void InputController_OnEquipPerformed(object sender, System.EventArgs e)
    {
        HandleItemClick();
    }

    public void AddItem(Item item)
    {
        itemsList.Add(item.ItemData);
        UpdateStash();
    }

    public void AddHarvestable(int count, HarvestableSO harvestableSO)
    {
        Debug.Log("Implement AddHarvestable");
    }

    public void RemoveItem(ItemData itemData)
    {
        itemsList.Remove(itemData);
        UpdateStash();
    }

    private void UpdateStash()
    {
        foreach (ItemData itemData in itemsList)
        {
            for (int i = 0; i < itemStash.childCount; i++)
            {
                for (int j= 0; j < itemStash.GetChild(i).childCount; j++)
                {
                    Transform itemSlotTransform = itemStash.GetChild(i).GetChild(j);

                    ItemSlot itemSlot = itemSlotTransform.GetComponent<ItemSlot>();

                    if (itemSlot.ItemData == null)
                    {
                        itemSlot.ItemData = itemData;
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

                EquipItem(itemSlot.ItemData); break;
            }
        }
    }

    private void EquipItem(ItemData itemData)
    {
        Debug.Log($"Clicked on {itemData.Name}");

        switch (itemData.ItemType)
        {
            case EItemType.Weapon:
                ItemSlot mainHandSlot = equipped.Find("MainHand").GetComponent<ItemSlot>();
                GameObject weaponObject = Instantiate(itemData.ItemSO.Prefab);
                Weapon weapon = weaponObject.GetComponent<Weapon>();
                ItemSocketContainer rightHandSocketContainer = 
                    rightHandSockets.GetComponent<ItemSocketContainer>();
                rightHandSocketContainer.EquipItem(weaponObject);
                mainHandSlot.ItemData = itemData;
                mainHand = (WeaponData)itemData;

                RemoveItem(itemData);
                break;

            default:
                Debug.LogError($"{itemData.ItemType} not found");
                break;
        }
    }
}
