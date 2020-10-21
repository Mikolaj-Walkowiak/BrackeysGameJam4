using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Yarn.Unity;
public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController instance;
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion
    
    private NPCManager npcManager;
    private RewindManager rewind;
    [SerializeField] private Light2D light;
    public Light2D weaponLight;
    public Transform weaponLightPosition;
    
    private Inventory inventory;
    public Vector2 mousePosition;
    public float mouseAngle;
    public float scrollValue;
    private PlayerShooting playerShooting;
    public GameObject weaponSpot;
    private float prevWeaponSpot; 
	public bool isInDialogue;
    public InMemoryVariableStorage npcMemory;
    
    void Start()
    {
        
        npcManager = NPCManager.instance;
        npcMemory = npcManager.GetComponent<InMemoryVariableStorage>();
        rewind = RewindManager.instance;
        
        light = GetComponentInChildren<Light2D>();
        inventory = Inventory.instance;
        playerShooting = GetComponent<PlayerShooting>();

        isInDialogue = false;
    }
    void SetMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void SetMouseAngle()
    {
        Vector2 relativeMousePos = mousePosition - (Vector2)transform.position;
        float angleRadians = Mathf.Atan2(relativeMousePos.y, relativeMousePos.x);

        mouseAngle = angleRadians * Mathf.Rad2Deg;
    }
    
	public void HandleOnDialogue()
	{
		isInDialogue = true;
	}
    
    public void HandleOffDialogue()
    {
        isInDialogue = false;
    }

    void HandleLightRotation()
    {
        light.transform.localEulerAngles = new Vector3(0, 0, mouseAngle-90);
        weaponLight.transform.position = weaponLightPosition.transform.position;
        weaponLight.transform.localEulerAngles = new Vector3(0, 0, mouseAngle-90);
    }
    
    void DropItem(Item item){
        inventory.Remove(item,mousePosition);
    }
    
    void HandleInput()
    {
        if (IsBusy())
            return;
        //shooting
        if (Input.GetButtonDown("Fire1") && 
            inventory.items.Count - 1 >= inventory.activeItem && 
            inventory.items[inventory.activeItem].itemType == ItemType.Gun && 
            inventory.items[inventory.activeItem].currentAmmo > 0)
        {
            playerShooting.Shoot();
            npcMemory.SetValue("$alarm",1);
        }
       
        //change eq
        scrollValue = Input.GetAxis("Mouse ScrollWheel");
		//drop
        if (Input.GetKeyDown("g") &&  inventory.items.Count -1 >= inventory.activeItem)
        {
            Debug.Log("DROP: " + inventory.items[inventory.activeItem]);
            DropItem(inventory.items[inventory.activeItem]);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (rewind.rewindsRemaining > 0)
            {
                rewind.rewindsRemaining -= 1;
                rewind.IsRewinding = true;
            }
        }
    }

    void HandleScroll()
    {
        if (scrollValue > 0)
        {
            //Debug.Log("HandleScroll(-1)");
            inventory.ChangeActiveItem(-1);
        }
        else if (scrollValue < 0)
        {
            //Debug.Log("HandleScroll(1)");
            inventory.ChangeActiveItem(1);
        }
        
    }

    void SetWeaponHand()
    {
        
        if (mouseAngle > 0 && prevWeaponSpot <0)
        {
            weaponSpot.transform.position = weaponSpot.transform.parent.TransformPoint(-0.348f, -0.273f, 0f);
            weaponSpot.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (mouseAngle < 0 && prevWeaponSpot > 0)
        {
            weaponSpot.transform.position = weaponSpot.transform.parent.TransformPoint(0.348f, -0.273f, 0f);
            weaponSpot.transform.localScale = new Vector3(1, -1, 1);
        }

        prevWeaponSpot = mouseAngle;
    }

    public bool IsBusy()
    {
        if (isInDialogue || rewind.IsRewinding)
            return true;
        return false;
    }
    void Update()
    {
        SetMousePosition();
        SetMouseAngle();
        HandleLightRotation();
        HandleInput();
        HandleScroll();
        SetWeaponHand();
    }
}
