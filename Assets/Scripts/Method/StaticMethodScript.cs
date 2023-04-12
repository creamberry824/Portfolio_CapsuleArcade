using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMethodScript : MonoBehaviour
{
    //하위 오브젝트 레이어 변경
    public static void ChangeLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
            ChangeLayersRecursively(child, name);
    }

    //하위 오브젝트 렌더링 숨기기
    public static void RendererHidn(Transform trans, bool isHidn)
    {
        if (trans.GetComponent<Renderer>())
            trans.GetComponent<Renderer>().enabled = !isHidn;
        foreach (Transform child in trans)
            RendererHidn(child, isHidn);
    }

    //간단 풀메니저
    public static GameObject CreatPoolObject(List<GameObject> pools, GameObject obj, Transform poolsTrans)
    {
        GameObject select = null;
        foreach (GameObject item in pools)
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        if (select == null)
        {
            select = Instantiate(obj, poolsTrans);
            pools.Add(select);
        }
        return select;
    }
}
