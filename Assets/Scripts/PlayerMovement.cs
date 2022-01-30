using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using TMPro;

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

    private Animator playerAnim;

	private BigBadController BBC;

    private GameManager gameManager;

    private bool nearItem;
    private GameObject nearbyItem;

    private Light2D globalLight;

    private TextMeshProUGUI dialog;

    private SpriteRenderer fog;

    private GameObject pickupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = walkSpeed;

        flashlight = transform.Find("Flashlight").gameObject;
        torch = transform.Find("Torch").gameObject;
		BBC = FindObjectOfType<BigBadController> ();

        playerAnim = GetComponent<Animator>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();

        dialog = GameObject.Find("Dialog Label").GetComponent<TextMeshProUGUI>();

        fog = GameObject.Find("Fog").GetComponent<SpriteRenderer>();

        pickupPrefab = Resources.Load("cfx_pickup") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //movement
        if (moveInput != Vector2.zero)
        {
            rb.velocity = moveInput * moveSpeed;
            playerAnim.SetTrigger("Walk");


            //float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            //rb.rotation = angle-90;
        }
        else
        {
            playerAnim.SetTrigger("Idle");
        }

        //aiming of light
        Vector3 viewportMousePos = Camera.main.ScreenToViewportPoint(rawAimInput);
        Vector3 viewportPlayerPos = Camera.main.WorldToViewportPoint(rb.position);
        Vector3 dir = viewportMousePos - viewportPlayerPos;

        float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //flashlight.transform.rotation = Quaternion.Euler(0, 0, rotAngle - 90);
        rb.rotation = rotAngle - 90;
    }

	public void UpdateBigBad ()
	{
		if (usingTorch) {
			if (torch.activeInHierarchy)
				BBC.Target = transform.position;
		} else
			if (flashlight.activeInHierarchy)
				BBC.Target = transform.position;

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
            if (!usingTorch)
            {
                //torch.SetActive(false);
                flashlight.SetActive(!flashlight.activeInHierarchy);
				BBC.HasTarget = flashlight.activeInHierarchy;
			}
            else
            {
                //flashlight.SetActive(false);
                torch.SetActive(!torch.activeInHierarchy);
				BBC.HasTarget = torch.activeInHierarchy;
            }
			UpdateBigBad ();	// doesn't matter if this is called when turning the light off, as the BB should already be aiming at this spot.
			// It does mean that turning on your light will get the BB moving straight away.

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
			BBC.HasTarget = true;   // As switching light types always turns one of them on at present.
			UpdateBigBad ();
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

    public void PickUpItem(InputAction.CallbackContext ctx)
    {
        if (!GameManager.isPaused)
        {
            if (ctx.performed)
            {
                if (nearItem)
                {
                    if (nearbyItem.CompareTag("EnergyDrink"))
                    {
                        dialog.enabled = false;
                        StartCoroutine(moveFaster(5.0f));
                    }
                    else if (nearbyItem.CompareTag("FogMachine"))
                    {
                        dialog.enabled = false;
                        StartCoroutine(turnOnGlobalLight(3.0f));
                    }
                    else if (nearbyItem.CompareTag("Fireworks"))
                    {
                       // do something
                    }

                    Instantiate(pickupPrefab, rb.position, Quaternion.identity);

                    Destroy(nearbyItem);
                    nearItem = false;
                }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            nearItem = true;

            nearbyItem = other.gameObject;

            if (nearbyItem.CompareTag("EnergyDrink"))
            {
                dialog.enabled = true;
                dialog.SetText("AcroHydrate. Looks like an energy drink. I didn’t have Horace Winterson pegged as a gym junkie, but no harm in trying it. <Press E to use>");
            }
            else if (nearbyItem.CompareTag("FogMachine"))
            {
                dialog.enabled = true;
                dialog.SetText(" I wonder what’s in here? Looks like a tiny black box. Could this be the black box Horace wrote about? If so, maybe it’ll help me hide from him for a little while….  <Press E to use>");
            }
            else if (nearbyItem.CompareTag("Fireworks"))
            {
                
            }
        }
        
    }

    //CameraShake.Instance.ShakeCamera(6.0f, 0.25f);



    IEnumerator turnOnGlobalLight (float duration)
    {
        globalLight.intensity = 0.5f;

        fog.material.color = Color.gray;

        yield return new WaitForSecondsRealtime(duration);

        globalLight.intensity = 0.001f;

        fog.material.color = Color.white;
    }

    IEnumerator moveFaster(float duration)
    {
        moveSpeed = runSpeed;

        yield return new WaitForSecondsRealtime(duration);

        moveSpeed = walkSpeed;
    }
}
