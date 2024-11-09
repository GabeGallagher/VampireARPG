using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject itemStash;

    [SerializeField] private InputController inputController;

    private List<ItemSO> itemsList = new List<ItemSO>();

    private GraphicRaycaster graphicRaycaster;

    private EventSystem eventSystem;

    private void Awake()
    {
        graphicRaycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
    }
    private void Start()
    {
        inputController.OnEquipPerformed += InputController_OnEquipPerformed;
    }

    private void InputController_OnEquipPerformed(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public void AddItem(ItemSO item)
    {
        itemsList.Add(item);

        UpdateStash();
    }

    private void UpdateStash()
    {
        foreach (ItemSO item in itemsList)
        {
            for (int i = 0; i < itemStash.transform.childCount; i++)
            {
                for (int j= 0; j < itemStash.transform.GetChild(i).childCount; j++)
                {
                    Transform itemSlotTransform = itemStash.transform.GetChild(i).GetChild(j);

                    ItemSlot itemSlot = itemSlotTransform.GetComponent<ItemSlot>();

                    if (itemSlot.InventorySprite.GetComponent<Image>().sprite == null)
                    {
                        itemSlot.SetSprite(item.Sprite);
                        i = itemStash.transform.childCount;
                        break;
                    }
                }
            }
        }
    }

    private void EquipItem()
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
                Debug.Log($"Clicked on {itemSlot.gameObject.name}"); break;
            }
        }
    }
}
