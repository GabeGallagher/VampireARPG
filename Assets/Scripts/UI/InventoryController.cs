using System;
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

    private List<ItemData> itemsList = new List<ItemData>();
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;
    private WeaponData mainHand;
    private PlayerController player;

    public WeaponData MainHand { get => mainHand; }

    private void Awake()
    {
        graphicRaycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();

        gameObject.SetActive(false);
    }
    private void Start()
    {
        inputController.OnEquipPerformed += InputController_OnEquipPerformed;
        try
        {
            player = GetComponent<PlayerController>();
        }
        catch (NullReferenceException e)
        {
            Debug.LogError($"Player not found: {e.Message}");
        }
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

    public void AddHarvestable(HarvestableSO harvestableSO)
    {

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
                GameObject weaponObject = Instantiate(itemData.ItemSO.Prefab, rightHand);
                Weapon weapon = weaponObject.GetComponent<Weapon>();
                player.MainHand = weapon;
                mainHandSlot.ItemData = itemData;
                mainHand = (WeaponData)itemData;

                // Fix this. The local transform should allow for the refactored game object to fit in the character's hand and look good relative to that character/animation. Shouldn't need to hardcode transforms anymore.
                Vector3 localPosition = new Vector3(0.1294f, 0.0179f, -0.0453f);
                Quaternion localRotation = Quaternion.Euler(15.177f, -106.1f, 101.719f);
                weapon.transform.localPosition = localPosition;
                weapon.transform.localRotation = localRotation;

                RemoveItem(itemData);
                break;

            default:
                Debug.LogError($"{itemData.ItemType} not found");
                break;
        }
    }
}
