using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItemView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{

    public Canvas m_canvas;
    public GameObject item_image;
    public ITEMTYPE item_type;

    public int index = -1;

    public GameObject drag_item_prefab;
    GameObject drag_item_view;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setItemView(ItemBase item) {
        // item_image.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.isprite);
        Addressables.LoadAssetAsync<Sprite>(item.isprite).Completed += (handle) => {
            item_image.GetComponent<Image>().sprite = handle.Result;
        };
        this.item_type = item.itemtype;
        print("oh yeah! addressable load resources");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item_type != ITEMTYPE.EMPTY && eventData.button != PointerEventData.InputButton.Right) {
            drag_item_view = Instantiate(drag_item_prefab, m_canvas.transform);
            drag_item_view.GetComponent<Image>().sprite = item_image.GetComponent<Image>().sprite;
            drag_item_view.GetComponent<ItemDragView>().m_canvas = m_canvas;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (drag_item_view != null) {
            Destroy(drag_item_view);
            if (item_type != ITEMTYPE.EMPTY) {
                InventoryControllor.Instance.swapItems(m_canvas, index);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item_type != ITEMTYPE.EMPTY && eventData.button == PointerEventData.InputButton.Right) {
            print("eat unit");
            InventoryControllor.Instance.useItem(m_canvas, index);
        }
    }
}
