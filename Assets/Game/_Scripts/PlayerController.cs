using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] List<Color> colorList;
    [SerializeField] PlayerInput input;

    [SerializeField] float moveSpeed;
    [SerializeField] float movePower;
    [SerializeField] float maxSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] int fireCount;
    [SerializeField] int testCount;
    [SerializeField] float curSpeed;

    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float fireCoolTime;

    float lastFireTime;

    Vector3 moveDir;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            Destroy(input);
        }

        SetPlayerColor();
    }

    private void Update()
    {
        //if (!view.IsMine)       // �������� �� �������� Ȯ��
        //    return;

        //transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        Rotate();
    }

    private void FixedUpdate()
    {
        Accelate();
    }

    void Accelate()
    {
        rigid.AddForce(moveDir.z * transform.forward * movePower, ForceMode.Force);
        if (rigid.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }
        curSpeed = rigid.velocity.magnitude;
    }

    void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up, moveDir.x * rotateSpeed * Time.deltaTime);
    }

    void OnFire(InputValue value)
    {        
        //ResultCreateBullet();
        photonView.RPC("RequestCreateBullet", RpcTarget.MasterClient);      // ���� ���� ����ȭ ��� 1. ��ġ�� ������ �������ش�
    }

    [PunRPC]
    void RequestCreateBullet()
    {
        if (Time.time < lastFireTime + fireCoolTime)
            return;

        fireCount++;                                    // ���⿡�� ++�� �ϵ�
        lastFireTime = Time.time;
        photonView.RPC("ResultCreateBullet", RpcTarget.AllViaServer, );
    }

    [PunRPC]            // �Լ��� �����Ʈ��ũ�� ������ ��� (Remote Procedure Call)
    void ResultCreateBullet(Vector3 position, Quaternion rotation, PhotonMessageInfo info)                                        // ���� ���� ����ȭ ��� 1
    {
        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));                                           // ���� ���� ����ȭ ��� 3. ��Ʈ��ũ �ð��� �����ð��� ���Ͽ� �׸�ŭ ���� �� �����ش�

        //fireCount++;                                  // ���⿡�� ++�� �ϵ� ����� ����
        Bullet bullet = Instantiate(bulletPrefab, position, rotation);                                              // ���� ���� ����ȭ ��� 1
        bullet.transform.position += bullet.Velocity * lag;
    }

    void SetPlayerColor()
    {
        int playerNumber = photonView.Owner.GetPlayerNumber();
        if (colorList == null || colorList.Count == playerNumber)
            return;

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = colorList[playerNumber];

        if (photonView.IsMine)
        {
            renderer.material.color = Color.green;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Rigidbody rb = PhotonView.Find(photonView.ViewID).GetComponent<Rigidbody>();

        if (stream.IsWriting)                           // photonView.IsMine �� true
        {
            stream.SendNext(fireCount);                 // �ش� ���� �����ְ�
            //stream.SendNext(testCount);
            stream.SendNext(curSpeed);
        }
        else // if (stream.IsReading)                   // photonView.IsMine �� false 
        {
            fireCount = (int)stream.ReceiveNext();      // �ش� ���� �޾��ش�
            curSpeed = (float)stream.ReceiveNext();
        }
        // �������� ���� �������� ���������� �޴� ������ ��ġ������� �Ѵ�
        // fireCount�� ���� ���´µ� testCount�� ���� ������ ������ �����
    }
}

// �������� ����ȭ ��� 2. Photon Transform View ������Ʈ�� ����Ѵ�
