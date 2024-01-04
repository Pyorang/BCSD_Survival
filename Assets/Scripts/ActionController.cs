using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;    //���� ������ �ִ� �Ÿ�.

    private bool pickupActivated = false; //���� ������ �� true.

    private RaycastHit hitInfo; //�浹ü ���� ����.

    //������ ���̾�� �����ϵ��� ���̾� ����ũ�� ����
    [SerializeField]
    private LayerMask layerMask;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Text actionText;

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if(pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "ȹ���߽��ϴ�.");
                Destroy(hitInfo.transform.gameObject);
                InfoDisAppear();
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            ItemInfoAppear();
        }
        else
            InfoDisAppear();
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "ȹ��" + "<color=yellow>" + "(E)" + "</color>";
    }

    private void InfoDisAppear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
