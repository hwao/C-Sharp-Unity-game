using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IKitchenObjectParent
{
    public static PlayerScript Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedArgs> onSelectedCounterChanged;

    public class OnSelectedCounterChangedArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }


    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInputScript gameInputScript;
    public bool IsWalking { get; private set; }

    private Vector3 lastInteractionDir;
    private BaseCounter selectedCounter;

    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one Player instance");
        }

        Instance = this;
    }

    private void Start()
    {
        gameInputScript.OnInteractAction += GameInputScriptOnOnInteractAction;
        gameInputScript.OnInteractAltAction += GameInputScriptOnOnInteractAltAction;
    }

    private void GameInputScriptOnOnInteractAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInputScriptOnOnInteractAltAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlt(this);
        }
    }

    private bool CanMove(Vector3 moveDir, float moveDistance)
    {
        float playerRadius = .7f;
        float playerHeight = .2f;

        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius,
            moveDir,
            moveDistance);
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        Vector2 inputVectorNormalized = gameInputScript.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVectorNormalized.x, 0f, inputVectorNormalized.y);
        float interactionDistance = 2f;

        if (moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;
        }

        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactionDistance))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter BaseCounter))
            {
                if (selectedCounter != BaseCounter)
                {
                    SetSelectedCounter(BaseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter clearCounterScript)
    {
        selectedCounter = clearCounterScript;
        onSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedArgs
        {
            selectedCounter = selectedCounter
        });
    }

    private void HandleMovement()
    {
        Vector2 inputVectorNormalized = gameInputScript.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVectorNormalized.x, 0f, inputVectorNormalized.y);

        float moveDistance = (moveSpeed * Time.deltaTime);

        bool canMove = CanMove(moveDir, moveDistance);

        if (!canMove)
        {
            // Test only X
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            if (moveDir.x != 0 && CanMove(moveDirX, moveDistance))
            {
                moveDir = moveDirX;
                canMove = true;
            }
            else
            {
                // Test only Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                if (moveDir.z != 0 && CanMove(moveDirZ, moveDistance))
                {
                    moveDir = moveDirZ;
                    canMove = true;
                }
                // no move
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        IsWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return this.kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;
    }
}