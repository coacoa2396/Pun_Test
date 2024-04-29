using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviourPun
{
    [SerializeField] Rigidbody rigid;

    private void Awake()
    {
        if (photonView.InstantiationData == null)
        {
            Vector3 force = (Vector3)photonView.InstantiationData[0];
            Vector3 torque = (Vector3)photonView.InstantiationData[1];

            rigid.AddForce(force, ForceMode.Impulse);
            rigid.AddForce(torque, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (transform.position.sqrMagnitude > 40000)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.collider.gameObject == 플레이어) // 스톤은 룸오브젝트 == 서버
        {
            if (!photonView.IsMine)     // 마스터클라이언트가 아니면
                return;                 // 리턴

            충돌처리
        }


        */
    }
}
