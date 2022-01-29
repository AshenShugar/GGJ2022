using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 moveInput;
    private Vector2 rawAimInput;
    private Vector2 aimInput;

    private bool isPaused;

    private GameObject flashlight;
    private GameObject torch;

    private bool usingTorch;    //quick and dirty, if false (default) then Flashlight is active

    public float walkSpeed = 5.0f;
    public float runSpeed = 9.0f;
    private float moveSpeed;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = walkSpeed;

        flashlight = transform.Find("Flashlight").gameObject;
        torch = transform.Find("Torch").gameObject;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //movement
        rb.velocity = moveInput * moveSpeed;

        //aiming of light
        Vector3 viewportMousePos = Camera.main.ScreenToViewportPoint(rawAimInput);
        Vector3 viewportPlayerPos = Camera.main.WorldToViewportPoint(rb.position);
        Vector3 dir = viewportMousePos - viewportPlayerPos;

        float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //rb.rotation = Quaternion.AngleAxis((-rotAngle + 90), Vector3.up);

        flashlight.transform.rotation = Quaternion.Euler(0, 0, rotAngle - 90);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (!GameManager.isPaused)
        {
            Vector2 rawInput = ctx.ReadValue<Vector2>();

            moveInput = rawInput.normalized;   //this was previously missing, ensures diagonals have same speed as straight directions
        }
    }

    public void Aim(InputAction.CallbackContext ctx)
    {
        if (!GameManager.isPaused)
        {
            rawAimInput = ctx.ReadValue<Vector2>();

            aimInput = rawAimInput.normalized;   //this was previously missing, ensures diagonals have same speed as straight directions
        }
    }

    public void ToggleLight(InputAction.CallbackContext ctx)
    {
        if (!GameManager.isPaused)
        {
            if (ctx.performed)
            {
                if (!usingTorch)
                {
                    //torch.SetActive(false);
                    flashlight.SetActive(!flashlight.activeInHierarchy);
                }
                else
                {
                    //flashlight.SetActive(false);
                    torch.SetActive(!torch.activeInHierarchy);
                }
            }
        }
    }

    public void SwitchLightTypes(InputAction.CallbackContext ctx)
    {
        if (!GameManager.isPaused)
        {
            if (ctx.performed)
            {
                usingTorch = !usingTorch;

                if (usingTorch)
                {
                    torch.SetActive(true);
                    flashlight.SetActive(false);
                }
                else
                {
                    flashlight.SetActive(true);
                    torch.SetActive(false);
                }
            }
        }
    }

    public void Run(InputAction.CallbackContext ctx)
    {
        if (!GameManager.isPaused)
        {
            if (ctx.started)
            {
                moveSpeed = runSpeed;
            }
            else if (ctx.canceled)
            {
                moveSpeed = walkSpeed;
            }
        }
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            gameManager.TogglePause();
        }
    }



    //CameraShake.Instance.ShakeCamera(6.0f, 0.25f);
}