using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMethodScript : MonoBehaviour
{
    //���� ������Ʈ ���̾� ����
    public static void ChangeLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
            ChangeLayersRecursively(child, name);
    }

    //���� ������Ʈ ������ �����
    public static void RendererHidn(Transform trans, bool isHidn)
    {
        if (trans.GetComponent<Renderer>())
            trans.GetComponent<Renderer>().enabled = !isHidn;
        foreach (Transform child in trans)
            RendererHidn(child, isHidn);
    }

    //���� Ǯ�޴���
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
