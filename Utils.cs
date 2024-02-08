using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Utils
{

    /* 2D tranform */

    /// <summary>
    /// 从xy取得2D角度
    /// </summary>
    static public float GetDegreeFromVector2D(float x, float y) {
        return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 将xy投射在半径为1的圆上
    /// </summary>
    static public Vector2 GetCircleDirection2D(float x, float y) {
        float r = Mathf.Sqrt(x * x + y * y);
        return new Vector2(x / r, y / r);
    }


    /* UGUI */

    /// <summary>
    /// 将UGUI画布上的物品跟随鼠标移动
    /// </summary>
    /// <param name="m_canvas">画布</param>
    /// <param name="follower">跟随的UGUI元素</param>
    static public void UGUIFollow(Canvas m_canvas, GameObject follower) {
        Vector2 position;
        // 获取鼠标在画布上的位置
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvas.transform as RectTransform, Input.mousePosition, m_canvas.worldCamera, out position);
        // 然后给显示面板的位置赋值
        // 这里注意的是获取后的position应该赋值给物品信息的RectTransform的anchoredPosition而不是Transform.position
        follower.GetComponent<RectTransform>().anchoredPosition = position;
    }

    /// <summary>
    /// 取得鼠标指向的UGUI物体
    /// </summary>
    /// <param name="m_canvas">画布</param>
    /// <returns></returns>
    static public GameObject GetMousePositionUI(Canvas m_canvas) {
        // 指针事件系统
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> result = new List<RaycastResult>();

        // 画布使用raycast触发指针事件
        GraphicRaycaster raycaster = m_canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(eventData, result);

        if (result.Count > 0) {
            return result[0].gameObject;
        }
        return null;
    }
}
