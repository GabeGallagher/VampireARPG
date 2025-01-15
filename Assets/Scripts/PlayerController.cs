using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour, IDamage, IDamageable
{
    public List<SkillSO> learnedSkills;
    public int strength, agility, intelligence, vitality;

    public event Action<EPlayerState> OnPlayerStateChange;

    public enum EPlayerState
    {
        Idle,
        Running,
        Attacking,
        Building,
    }

    [SerializeField] private InputController inputController;
    [SerializeField] private PhysicalAttackSO attack;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private SkillTabController skillTabController;
    [SerializeField] private BuildMenuUI buildMenuUI;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private SkillSO basicAttackSkill;

    [SerializeField] private float distanceToMoveThreshold = 0.05f;

    [SerializeField] private int pickupRange = 4;
    [SerializeField] private int currentHealth = 50;
    [SerializeField] private int level = 1;
    [SerializeField] private int experience = 0;

    private Canvas canvas;
    private NavMeshAgent navMeshAgent;
    private Vector3 moveToPosition;
    private EPlayerState playerState = EPlayerState.Idle;
    private PhysicalAttackSO skillInUse;

    private int maxHealth = 100;
    private int levelUpExperience = 10;

    private bool canMove = true;
    private bool inBuildMode = false;

    public PhysicalAttackSO Attack { get => attack; }
    public InventoryController InventoryController { get => inventoryController; }
    public WeaponSO MainHand => inventoryController.MainHand;
    public SkillSO BasicAttackSkill => basicAttackSkill;

    public int MaxHealth { get => maxHealth; }
    public int CurrentHealth { get => currentHealth; }
    public int LevelUpExperience {  get => levelUpExperience; }
    public int Experience { get => experience; }
    public int PickupRange { get => pickupRange; }

    public bool IsIdle { get => playerState == EPlayerState.Idle; }
    public bool IsRunning { get => playerState == EPlayerState.Running; }
    public bool IsAttacking { get => playerState == EPlayerState.Attacking; }
    public bool InBuildMode { get => inBuildMode; }

    public EPlayerState PlayerState
    {
        get => playerState;
        set
        {
            if (playerState != value)
            {
                playerState = value;
                OnPlayerStateChange?.Invoke(playerState);
            }
        }
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();

        inputController.OnMove += InputController_OnMove;
        inputController.OnEnemyClicked += InputController_OnDamageableClicked;
        inputController.OnOpenInventory += InputController_OnOpenInventory;
        inputController.OnHarvestableClicked += InputController_OnDamageableClicked;
        inputController.OnToggleBuildMode += InputController_OnToggleBuildMode;
        inputController.OnOpenSkillTab += InputController_OnOpenSkillTab;
    }

    private void InputController_OnOpenSkillTab(object sender, EventArgs e)
    {
        skillTabController.gameObject.SetActive(!skillTabController.gameObject.activeSelf);
    }

    private void InputController_OnToggleBuildMode(object sender, EventArgs e)
    {
        inBuildMode = !inBuildMode;

        if (inBuildMode)
        {
            canMove = false;
            PlayerState = EPlayerState.Idle; // Change this to build when build mode idle anim implemented
        }
        else
        {
            canMove = true;
        }
    }

    private void InputController_OnOpenInventory(object sender, EventArgs e)
    {
        inventoryController.gameObject.SetActive(!inventoryController.gameObject.activeSelf);
    }

    private void Update()
    {
        if (!inputController.CanMove || !canMove)
        {
            StopMoving();
        }
        else
        {
            navMeshAgent.isStopped = false;
        }

        if (PlayerState == EPlayerState.Running && HasReachedDestination())
        {
            PlayerState = EPlayerState.Idle;
        }
    }

    private bool HasReachedDestination()
    {
        float threshold = distanceToMoveThreshold;

        float deltaX = transform.position.x - moveToPosition.x;
        float deltaZ = transform.position.z - moveToPosition.z;

        return (deltaX * deltaX + deltaZ * deltaZ) < (threshold * threshold);
    }

    private void InputController_OnDamageableClicked(object sender, OnDamageableClickedEventArgs e)
    {
        float distanceToEnemy = Vector3.Distance(transform.position, e.TargetTransform.position);

        if (inventoryController.MainHand != null) // Need to refactor if punches are allowed

        {
            WeaponSO weapon = inventoryController.MainHand;
            float attackRange = MainHand.Range + e.Skill.AttackRange;

            if (distanceToEnemy <= attackRange || weapon.IsRanged)
            {
                canMove = false;
                PlayerState = EPlayerState.Attacking;
                skillInUse = (PhysicalAttackSO)e.Skill;
                animationController.TriggerAttack();
            }
        }
    }

    private IEnumerator AttackRoutine(float attackTime, List<Transform> targetTransformList)
    {
        PlayerState = EPlayerState.Attacking;

        canMove = false;

        yield return new WaitForSeconds(attackTime);

        DealDamage((int)attack.Damage, targetTransformList);

        PlayerState = EPlayerState.Idle;

        canMove = true;
    }

    private void InputController_OnMove(object sender, System.EventArgs e)
    {
        if (inputController.CanMove && canMove)
        {
            moveToPosition = inputController.MousePosition;
            navMeshAgent.destination = moveToPosition;
            PlayerState = EPlayerState.Running;
        }
        else
        {
            StopMoving();
        }
    }

    private int CalcPhysicalDamage(PhysicalAttackSO skill)
    {
        int weaponDmg = (int)Random.Range(inventoryController.MainHand.MinDamage, inventoryController.MainHand.MaxDamage);
        return (int)(weaponDmg * skill.Damage * (strength / 100));
    }

    public void DealDamage(int damage, List<Transform> targetList)
    {
        foreach (Transform target in targetList)
        {
            if (target.GetComponent<IDamageable>() != null)
            {
                target.GetComponent<IDamageable>().DamageReceived(damage, gameObject);
            }
        }
    }

    public void SpawnDamageObject()
    {
        GameObject damageObject = Instantiate(skillInUse.AttackObjectPrefab, transform, false);
        PhysicalAttackSO damageSkill = skillInUse;
        AttackController damageObjectController = damageObject.GetComponent<AttackController>();

        damageObjectController.Skill = damageSkill;
        damageObjectController.Radius = damageSkill.AttackRange + MainHand.Range;
        damageObjectController.Player = this;
    }

    public int CalcDamage(SkillSO damageSkill)
    {
        float damageFloat = ((MainHand.MinDamage + MainHand.MaxDamage) / 2) * damageSkill.Damage;
        return (int)damageFloat;
    }

    // Checks if player is holding attacking key. If yes, launch new attack, if not, set player state back to idle
    public void FinishAttackAnimation()
    {
        canMove = true;
        PlayerState = EPlayerState.Idle;
    }

    public void TargetKilled(GameObject target)
    {
        if (target.GetComponent<EnemyController>())
        {
            experience += 10;

            if (experience > levelUpExperience)
            {
                LevelUp();
            }
        }
    }

    private void LevelUp()
    {
        level += 1;
        Debug.Log("Leveled Up!");
    }

    public void StopMoving()
    {
        navMeshAgent.isStopped = true;
        moveToPosition = transform.position;
        PlayerState = EPlayerState.Idle;
    }

    public void PickUp(Item item)
    {
        inventoryController.AddItem(item.ItemSO);

        Destroy(item.gameObject);
    }

    public void HarvestResource(int resourceCount, HarvestableSO harvestableSO)
    {
        inventoryController.AddItem(harvestableSO);
    }

    public void DamageReceived(int damageReceived, GameObject damageFrom)
    {
        currentHealth -= damageReceived;

        ShowDamageText(damageReceived);

        if (currentHealth <= 0)
        {
            Debug.Log("You have died. Your deeds of valor will be remembered");
        }
    }

    public void ShowDamageText(int damageAmount)
    {
        GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, canvas.transform);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        RectTransform rectTransform = damageText.GetComponent<RectTransform>();

        rectTransform.position = screenPosition;

        TextMeshProUGUI textMesh = damageText.GetComponent<TextMeshProUGUI>();

        if (textMesh != null)
        {
            textMesh.text = damageAmount.ToString();
        }
    }

    public float GetAttackingRange()
    {
        if (inventoryController.MainHand == null) return 1f;
        else if (inventoryController.MainHand.IsRanged) return Mathf.Infinity;
        return inventoryController.MainHand.Range;
    }
}
