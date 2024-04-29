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
        //if (!view.IsMine)       // 프리팹이 내 소유인지 확인
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
        photonView.RPC("RequestCreateBullet", RpcTarget.MasterClient);      // 지연 보상 동기화 방법 1. 위치와 각도를 지정해준다
    }

    [PunRPC]
    void RequestCreateBullet()
    {
        if (Time.time < lastFireTime + fireCoolTime)
            return;

        fireCount++;                                    // 여기에서 ++를 하든
        lastFireTime = Time.time;
        photonView.RPC("ResultCreateBullet", RpcTarget.AllViaServer, );
    }

    [PunRPC]            // 함수를 포톤네트워크로 보내는 방법 (Remote Procedure Call)
    void ResultCreateBullet(Vector3 position, Quaternion rotation, PhotonMessageInfo info)                                        // 지연 보상 동기화 방법 1
    {
        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));                                           // 지연 보상 동기화 방법 3. 네트워크 시간과 서버시간을 비교하여 그만큼 힘을 더 가해준다

        //fireCount++;                                  // 여기에서 ++를 하든 결과는 같다
        Bullet bullet = Instantiate(bulletPrefab, position, rotation);                                              // 지연 보상 동기화 방법 1
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

        if (stream.IsWriting)                           // photonView.IsMine 이 true
        {
            stream.SendNext(fireCount);                 // 해당 값을 보내주고
            //stream.SendNext(testCount);
            stream.SendNext(curSpeed);
        }
        else // if (stream.IsReading)                   // photonView.IsMine 이 false 
        {
            fireCount = (int)stream.ReceiveNext();      // 해당 값을 받아준다
            curSpeed = (float)stream.ReceiveNext();
        }
        // 여러가지 값을 보낼때는 보낸순서와 받는 순서를 일치시켜줘야 한다
        // fireCount를 먼저 보냈는데 testCount를 먼저 받으면 오류가 생긴다
    }
}

// 지연보상 동기화 방법 2. Photon Transform View 컴포넌트를 사용한다
