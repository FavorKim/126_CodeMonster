using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandTrackingGrab : MonoBehaviour
{
    public enum CustomHandState
    {
        None = 0,
        Grab,
        Poke,
        Release
    }

    public class CustomHand
    {
        public CustomHand(OVRHand hand, Transform indexFinger, bool isRight)
        {
            this.hand = hand;
            this.indexFinger = indexFinger;
            this.isRight = isRight;
            state = CustomHandState.None;
        }
        public OVRHand hand;
        public Transform indexFinger;
        public bool isRight;
        public CustomHandState state;
    }

    CustomHand leftHand;
    CustomHand rightHand;

    public OVRHand leftOVRHand;
    public OVRHand rightOVRHand;
    public float pinchThreshold = 0.7f;
    [SerializeField] private OVRGrabbable rightGrabbedObject;
    [SerializeField] private OVRGrabbable leftGrabbedObject;

    [SerializeField] private Transform leftIndexFinger;
    [SerializeField] private Transform rightIndexFinger;

    public InputActionReference rightGrapInput;
    public InputActionReference leftGrapInput;

    private bool rightIsGrabbing = false;
    private bool RightIsGrabbing
    {
        get { return rightIsGrabbing; }
        set
        {
            if (rightIsGrabbing != value)
            {
                rightIsGrabbing = value;

                if (value == true)
                {
                    Debug.Log("right is grabbing");
                }
                else
                {
                    Debug.Log("right is release");
                }
            }
        }
    }

    private bool leftIsGrabbing = false;
    private bool LeftIsGrabbing
    {
        get { return leftIsGrabbing; }
        set
        {
            if (leftIsGrabbing != value)
            {
                leftIsGrabbing = value;
                if (value == true)
                {
                    Debug.Log("left is grabbing");
                }
                else
                {
                    Debug.Log("left is release");
                }
            }
        }
    }


    [SerializeField] private float moveSpeed = 0.01f;

    private event Action<CustomHand> OnGrab;
    private event Action<CustomHand> OnRelease;
    private event Action<CustomHand> OnPoke;

    private void Awake()
    {
        leftHand = new CustomHand(leftOVRHand, leftIndexFinger, false);
        rightHand = new CustomHand(rightOVRHand, rightIndexFinger, true);
    }

    private void OnEnable()
    {
        /*
        leftGrapInput.action.performed += OnGrabObject_Left;
        leftGrapInput.action.canceled += OnReleaseObject_Left;

        rightGrapInput.action.performed += OnGrabObject_Right;
        rightGrapInput.action.canceled += OnReleaseObject_Right;

        rightGrapInput.action.Enable();
        leftGrapInput.action.Enable();
        */
        OnGrab += TryGrab;
        OnRelease += Release;
        OnPoke += TryPoke;
    }
    private void OnDisable()
    {
        OnPoke -= TryPoke;
        OnRelease -= Release;
        OnGrab -= TryGrab;
    }



    void Update()
    {
        MoveHand(leftOVRHand, KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
        MoveHand(rightOVRHand, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
        MoveGrabbableObject();

        GetHandState(leftHand);
        GetHandState(rightHand);

        InteractHand(leftHand);
        InteractHand(rightHand);

    }


    private void MoveHand(OVRHand hand, KeyCode up, KeyCode down, KeyCode left, KeyCode right)
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(up))
        {
            movement += Vector3.forward;
        }
        if (Input.GetKey(down))
        {
            movement += Vector3.back;
        }
        if (Input.GetKey(left))
        {
            movement += Vector3.left;
        }
        if (Input.GetKey(right))
        {
            movement += Vector3.right;
        }

        hand.transform.Translate(movement * moveSpeed * Time.deltaTime);

        /*
        if (isGrabbing)
        {
            //if (grabObj == null)
            //{
            //    TryGrab(hand, grabObj);
            //}

            Debug.Log(hand.name + " is grabbing.");
        }
        else
        {
            // 여기서 Release 동작을 호출
            //Release(isLeft);
            Debug.Log(hand.name + " is releasing.");
        }
        */
    }
    /*
    private void OnGrabObject_Left(InputAction.CallbackContext con)
    {
        TryGrab(leftOVRHand, false);
        Debug.Log("Left Grabbed");
    }
    private void OnGrabObject_Right(InputAction.CallbackContext con)
    {
        TryGrab(rightOVRHand, true);
        Debug.Log("Right Grabbed");
    }

    private void OnReleaseObject_Left(InputAction.CallbackContext con)
    {
        Debug.Log("Left Canceled");

        if (leftGrabbedObject != null)
        {
            leftGrabbedObject.transform.SetParent(null);
            leftGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            leftGrabbedObject = null;
            Debug.Log("Left Released");
        }
    }
    private void OnReleaseObject_Right(InputAction.CallbackContext con)
    {
        Debug.Log("Right Canceled");

        if (rightGrabbedObject != null)
        {
            rightGrabbedObject.transform.SetParent(null);
            rightGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            rightGrabbedObject = null;
            Debug.Log("Right Released");
        }
    }
    */
    private void TryGrab(CustomHand hand)
    {
        Debug.Log(hand + "is Grab");
        Collider[] colliders = Physics.OverlapSphere(hand.hand.PointerPose.position, 0.05f);


        foreach (var collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out OVRGrabbable grabbable))
            {
                if (hand.isRight)
                {
                    rightGrabbedObject = grabbable;
                    rightGrabbedObject.transform.SetParent(hand.hand.transform);
                    rightGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                }
                else
                {
                    leftGrabbedObject = grabbable;
                    leftGrabbedObject.transform.SetParent(hand.hand.transform);
                    leftGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                }

                break;
            }
        }
    }

    private void TryPoke(CustomHand hand)
    {


        Collider[] indexColliders = Physics.OverlapSphere(hand.indexFinger.position, 0.01f);

        foreach (var collider in indexColliders)
        {
            if (collider.CompareTag("Pokeable"))
            {
                Debug.Log("Index finger poked " + collider.name);
            }
        }

    }

    private void Release(CustomHand hand)
    {
        var grabObj = hand.isRight ? rightGrabbedObject : leftGrabbedObject;
        if (grabObj != null)
        {
            grabObj.transform.SetParent(null);
            grabObj.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void MoveGrabbableObject()
    {
        if (leftGrabbedObject != null)
        {
            leftGrabbedObject.transform.position = new Vector3(leftOVRHand.transform.position.x, leftOVRHand.transform.position.y, 0);
        }
        if (rightGrabbedObject != null)
        {
            rightGrabbedObject.transform.position = new Vector3(rightOVRHand.transform.position.x, rightOVRHand.transform.position.y, 0);
        }
    }

    private void InteractHand(CustomHand hand)
    {
        Debug.Log(hand + "'s state is" + hand.state);
        switch (hand.state)
        {
            case CustomHandState.Grab:
                OnGrab.Invoke(hand);
                break;
            case CustomHandState.Poke:
                OnPoke.Invoke(hand);
                break;
            case CustomHandState.Release:
                OnRelease.Invoke(hand);
                break;
            default:
                break;
        }
    }

    CustomHandState GetHandState(CustomHand hand)
    {
        CustomHandState handState;
        if (IsGrab(hand))
            handState = CustomHandState.Grab;
        else if (IsPoke(hand))
            handState = CustomHandState.Poke;
        else if (IsRelease(hand))
            handState = CustomHandState.Release;
        else
            handState = CustomHandState.None;

        return handState;
    }

    bool IsPoke(CustomHand hand)
    {
        bool[] fingerPinchings = new bool[(int)OVRHand.HandFinger.Max];
        for (int i = 0; i < (int)OVRHand.HandFinger.Max; i++)
        {
            fingerPinchings[i] = hand.hand.GetFingerIsPinching((OVRHand.HandFinger)i);
        }
        for (int i = 0; i < 5; i++)
        {
            if (i == 1 && fingerPinchings[i] == true)
            {
                return false;
            }
            if (fingerPinchings[i] == true)
            {
                return false;
            }
        }
        return true;
    }

    bool IsGrab(CustomHand hand)
    {
        bool[] fingerPinchings = new bool[(int)OVRHand.HandFinger.Max];

        for (int i = 0; i < (int)OVRHand.HandFinger.Max; i++)
        {
            fingerPinchings[i] = hand.hand.GetFingerIsPinching((OVRHand.HandFinger)i);
        }
        foreach (bool isPinch in fingerPinchings)
        {
            if (!isPinch)
                return false;
        }
        return true;
    }

    bool IsRelease(CustomHand hand)
    {
        bool[] fingerPinchings = new bool[(int)OVRHand.HandFinger.Max];

        for (int i = 0; i < (int)OVRHand.HandFinger.Max; i++)
        {
            fingerPinchings[i] = hand.hand.GetFingerIsPinching((OVRHand.HandFinger)i);
        }
        foreach (bool isPinch in fingerPinchings)
        {
            if (isPinch)
                return false;
        }
        return true;
    }
}



/* 조건 반복 블록 구상
 
 
 반복 - 조건(미리 정할 것), 반복할 행동.Action()

CodeBlock _block;

void 반복문(CodeBlock block)
{
    
    while(조건)
    {
        block.Action();
    }
}


조건 - 조건(미리 정해지는 것), 조건 충족시 수행할 행동

void 조건문(Func<bool> 조건, Action 수행할 행동)
{
    if(조건)
    {
        수행할행동.Invoke();
    }
}

플레이어
OnTriggerEnter(Collision other)
{
    if(other.TryGetComponent(out Monster mon)
    {
        mon
    }
}

 */


/* 코드블럭 콘테이너 등록 예시
class 코드블럭 
{
    블럭컨테이너 con;
        
    void OnEnable()
    {
        핸드트래커.Intance.OnRelease += OnRelease_CheckContatiner;
    }

    void OnRelease_CheckContainer()
    {
        if(con!=null)
            con.블럭생성();
    }

    OnTriggerEnter(collider col)
    {
        if(col.TryGetComponent(out 블럭컨테이너 con)
        {
            con = this.con;
        }
    }

    (Exit은 반대로 할당해제 con = null;)
}
---------------------------------------------------------
class 블럭컨테이너 
{
    public void 블럭생성(코드블럭 block)
    {
        Instantiate(block.gameObject);
    }
}
---------------------------------------------------------
class 핸드트래커 : 싱글톤
{
    코드블럭 들고있는블럭;

    public event void OnRelease;
    
    void 놓는기능()
    {
        놓는 로직();
        OnRelease.Invoke();
    }
}
 
 */