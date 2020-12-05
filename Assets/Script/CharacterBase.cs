using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CharacterBase : MonoBehaviour, IPoolingObject, IInteractableObject, ICameraObstacle
{
    public int StartCount { get; set; } = 8;

    public static float DEFAULT_RUN_SPEED = 5.0f;
    public static float DEFAULT_JUMP_SPEED = 5.0f;
    public static float DEFAULT_JUMP_POWER = 4.0f;

    public GameObject m_Model { get; private set; }

    /// <summary>
    /// 코스튬 관련 컴포넌트
    /// </summary>
    private CostumeComponent m_CostumeComponent;

    /// <summary>
    /// FSM 상태
    /// </summary>
    private FSMComponent m_FSMCompnent;

    /// <summary>
    /// 플레이어 캐릭터가 물건 쌓을때 쓰는 컴포넌트
    /// </summary>
    private StackComponent m_StackComponent;

    /// <summary>
    /// 터치관련 입력 컴포넌트, 옵저버 패턴으로 사용
    /// </summary>
    private InputComponent m_InputComponent;

    /// <summary>
    /// 플레이어 머리 위 네임 빌보드
    /// </summary>
    public BillBoard m_BillBoard;

    /// <summary>
    /// 주의) 이거 없이 사용하면 코스튬 갈아입고 난 뒤에 애니메이션이 재생되지 않습니다. 애니메이터가 inValid 되서 먹통됨.
    /// 코스튬 갈아입을때 이거 애니메이터에 다시 대입해주세요. 구글링에서 출처
    /// </summary>
    private RuntimeAnimatorController m_animatorController;

    Ray m_Ray = new Ray();
    RaycastHit m_RayCastHit;


    public Color m_MyPlayerColor { get; private set; }

    public System.Action OnVictory { get; set; }

    public float m_RunSpeed;
    public float m_JumpSpeed;
    public float m_JumpPower;

    public bool m_isMyCharacter { get; private set; }

    Animator m_Animator;

    Color[] color = { Color.green,Color.red, Color.blue, Color.yellow, Color.cyan, Color.magenta };

    int m_Rank;
    string m_NickName;

    public string NickName
    {
        get
        {
            return m_NickName;
        }
        set
        {
            if (value == null)
            {
                if (m_BillBoard)
                    m_BillBoard.SetName(null);
            }
            else
            {
                SetName(value);
            }
        }
    }

    public int Rank
    {
        get
        {
            return m_Rank;
        }
        set
        {
            m_Rank = value;
        }
    }


    public int StackCount
    {
        get
        {
            if (m_StackComponent == null)
                return 0;

            return m_StackComponent.StackCount;
        }
    }

    public IInteractableObject StackTop
    {
        get
        {
            if (m_StackComponent == null)
                return null;

            return m_StackComponent.Top;
        }
    }

    public float RunSpeed
    {
        get
        {
            return m_RunSpeed;
        }
        
    }

    Shader m_TrShader;
    Shader m_OriginShader;

    void Awake()
    {
        m_TrShader = Shader.Find("Custom/ToonShader_Tr");
        m_OriginShader = Shader.Find("Custom/ToonShader");
    }

    void Update()
    {
        CaculateSpeed();
    }

    public void Init(bool isMyCharacter, int idx)
    {
        m_isMyCharacter = isMyCharacter;

        m_Model = transform.Find("people_face_01").gameObject;
        m_Model.transform.localPosition = Vector3.zero;
        m_Model.transform.localRotation = Quaternion.identity;
         
        m_Animator = GetComponentInChildren<Animator>();
        m_Animator.enabled = false;

        m_animatorController = m_Animator.runtimeAnimatorController;

        m_CostumeComponent = Common.GetOrAddComponent<CostumeComponent>(gameObject);
        m_CostumeComponent.Init();

        m_StackComponent = Common.GetOrAddComponent<StackComponent>(gameObject);
        m_StackComponent.Init(this);

        m_FSMCompnent = Common.GetOrAddComponent<FSMComponent>(gameObject);
        m_FSMCompnent.Init(this);

        m_InputComponent = Common.GetOrAddComponent<InputComponent>(gameObject);
        m_InputComponent.Init(this);

        OnVictory = m_StackComponent.OnVictory;

        if(m_BillBoard == null)
            m_BillBoard = gameObject.GetComponentInChildren<BillBoard>();

        m_BillBoard.gameObject.SetActive(true);
        if (m_isMyCharacter)
        {
            m_MyPlayerColor = Color.green;

            if (PlayerPrefs.HasKey("myName"))
            {
                NickName = PlayerPrefs.GetString("myName");
            }
            else
            {
                NickName = "Runner";
            }

            m_BillBoard.InitProfileSprite(null);
        }
        else
        {
            m_MyPlayerColor = color[idx];

            
            m_BillBoard.SetName( Common.strName[Random.Range(0, Common.strName.Length)]);
            m_BillBoard.InitProfileSprite(Common.regionName[Random.Range(0, Common.regionName.Length)]);
        }

        m_BillBoard.SetColor(m_MyPlayerColor);

    }

    /// <summary>
    /// 1001 = 디폴트 코스튬
    /// </summary>
    /// <param name="ID"></param>
    public void SetCostumeByID(int ID = 1001)
    {
        if (m_CostumeComponent == null)
        {
            Common.LogError("m_CostumeComponent = null!");
        }

        CharacterManager.Instance.m_CurrentWearCostume = ID;
        m_CostumeComponent.OnLoadAllParts(ID);

        m_Animator.runtimeAnimatorController = null;
        m_Animator.enabled = false;
        m_Animator.enabled = true;
        m_Animator.runtimeAnimatorController = m_animatorController;
    }

    void SetName(string str)
    {
        m_NickName = str;

        if(m_isMyCharacter)
            PlayerPrefs.SetString("myName", m_NickName);

        if (m_BillBoard)
            m_BillBoard.SetName(m_NickName);
    }


    public void SetCostumeByRandom()
    {
        if(m_CostumeComponent == null)
        {
            Common.LogError("m_CostumeComponent = null!");
        }

        m_CostumeComponent.OnLoadRandomAllParts();

        m_Animator.runtimeAnimatorController = null;
        m_Animator.enabled = false;
        m_Animator.enabled = true;
        m_Animator.runtimeAnimatorController = m_animatorController;
    }

    public void Initialize(GameObject factory)
    {
        transform.SetParent(factory.transform);
    }

    public void OnPushToQueue()
    {
        if(m_Animator != null)
            m_Animator.enabled = false;

        m_RunSpeed = 0.0f;
        m_JumpSpeed = 0.0f;
        m_JumpPower = 0.0f;

        ChangeState(E_CHARACTER_STATE.IDLE);

        if (m_StackComponent != null)
            m_StackComponent.Init(this);

        gameObject.SetActive(false);

    }

    public void OnPopFromQueue()
    {
        if (m_Animator != null)
            m_Animator.enabled = true;

        m_RunSpeed = DEFAULT_RUN_SPEED;
        m_JumpSpeed = DEFAULT_JUMP_SPEED;
        m_JumpPower = DEFAULT_JUMP_POWER;

        ChangeState(E_CHARACTER_STATE.IDLE);

        if (m_StackComponent != null)
            m_StackComponent.Init(this);
    }

    public void ChangeState(E_CHARACTER_STATE state)
    {
        if(m_FSMCompnent)
            m_FSMCompnent.ChangeState(state);
    }

    public float GetAnimationPlayRatio()
    {
        if(m_Animator == null)
        {
            return 0.0f;
        }

        if (m_Animator.enabled)
        {
            return m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        else
            return 0.0f;
    }

    public void StopNav()
    {
        if(m_FSMCompnent)
        {
            m_FSMCompnent.StopNav();
        }
    }

    public void StartNav(Vector3 pos)
    {
        if (m_FSMCompnent)
        {
            m_FSMCompnent.StartNav(pos);
        }
    }

    public void ChangeAniamtion(string aniName)
    {
        if(aniName == "Run" && StackCount > 0)
        {
            aniName = "RunWithBox";
        }


        if(m_Animator.enabled)
            m_Animator.Play(aniName, 0);
    }


    public void OnEnterCollision(CharacterBase otherCharacter, Vector3 hitPos)
    {
        if (m_FSMCompnent.m_EnumCurrentState == E_CHARACTER_STATE.IDLE) return;

        Vector3 dir = (otherCharacter.transform.position - transform.position).normalized;

        if(Vector3.Dot(dir, transform.forward) > 0.5)
        {
            ChangeState(E_CHARACTER_STATE.IDLE);
        }
    }

    public void OnExitCollision(CharacterBase character)
    {

    }

    public void PushToStackObject(IInteractableObject interactItem)
    {
        m_StackComponent.PushToStackObject(interactItem);

        if (m_isMyCharacter)
        {
            CameraManager.Instance.PlusDistance(0.15f, 0.15f);
            UIManager.Instance.ShowStackHUD();
        }
        ChangeAniamtion("Run");


    }

    public IInteractableObject PopFromStackObject()
    {
        IInteractableObject obj = m_StackComponent.PopFromStackObject();

        if (m_isMyCharacter && obj != null)
            CameraManager.Instance.PlusDistance(-0.15f, -0.15f);

        if (StackCount == 0)
        {
            ChangeAniamtion("Run");
        }

        return obj;
    }


    public bool CheckAllLayer(int layer, out RaycastHit hit, float distance = 0.5f)
    {
        m_Ray.origin = transform.position + transform.up * distance;
        m_Ray.direction = (-transform.up);

        if (Common.CheckAllRay(m_Ray, layer, distance * 1.2f, out hit))
        {
            return true;
        }

        return false;
    }

    public bool CheckForwardLayer(int layer, out RaycastHit hit, float distance = 0.5f)
    {
        m_Ray.origin = transform.position + transform.up * 0.8f;
        m_Ray.direction = transform.forward;

        if (Common.CheckRay(m_Ray, layer, distance * 1.5f, out hit))
        {
            return true;
        }

        return false;
    }

    public bool CheckLayer(int layer,out RaycastHit hit, float distance = 0.5f)
    {
        m_Ray.origin = transform.position + transform.up * distance;
        m_Ray.direction = (-transform.up);

        if(Common.CheckRay(m_Ray, layer, distance * 1.5f, out hit))
        {
            return true;
        }

        return false;
    }

    public void PushToPool()
    {
        PoolingManager.Instance.PushToPool(gameObject);
    }

    public void CaculateSpeed()
    {
        if (m_FSMCompnent == null)
        {
            m_RunSpeed = (DEFAULT_RUN_SPEED);
        }
        else
        {
            m_RunSpeed = (DEFAULT_RUN_SPEED  * (m_FSMCompnent.m_EnumCurrentState == E_CHARACTER_STATE.SPRINT ? 1.5f : 1.0f)) - (StackCount) * 0.05f;
        }

        if(m_Animator)
        {
            m_Animator.speed = m_RunSpeed * 0.2f;
        }
    }

    public void ChangeLayerState(int layer)
    {
        switch (layer)
        {
            case 1 << (int)E_LAYER.Ground:
                m_FSMCompnent.ChangeState(E_CHARACTER_STATE.RUN);
                break;
            case 1 << (int)E_LAYER.Dead:
                m_FSMCompnent.ChangeState(E_CHARACTER_STATE.DEAD);
                break;
            case 1 << (int)E_LAYER.Victory:
                m_FSMCompnent.ChangeState(E_CHARACTER_STATE.VICTORY);
                break;
            case 1 << (int)E_LAYER.GroundItem:
                m_FSMCompnent.ChangeState(E_CHARACTER_STATE.SPRINT);
                break;
        }
    }

    public bool IsFade { get; set; }

    public void EnterCollisionToCameraRay()
    {
        if (m_isMyCharacter)
            return;

        if (IsFade)
            return;

        SkinnedMeshRenderer[] skinRender = m_Model.GetComponentsInChildren<SkinnedMeshRenderer>();

        if (skinRender != null)
        {
            foreach (var skin in skinRender)
            {
                skin.material.shader = m_TrShader;

                if (skin.material.HasProperty("_Color"))
                {
                    Color prevColor = skin.material.GetColor("_Color");
                    skin.material.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 0.4f));
                    skin.material.renderQueue += 1;
                }
            }
        }

        if (m_StackComponent != null)
            m_StackComponent.OnFade();

        IsFade = true;
    }

    public void ExitCollisionToCameraRay()
    {
        if (m_isMyCharacter)
            return;

        if (!IsFade)
            return;

        SkinnedMeshRenderer[] skinRender = m_Model.GetComponentsInChildren<SkinnedMeshRenderer>();

        if (skinRender != null)
        {
            foreach (var skin in skinRender)
            {
                skin.material.shader = m_OriginShader;

                if (skin.material.HasProperty("_Color"))
                {
                    Color prevColor = skin.material.GetColor("_Color");
                    skin.material.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 1.0f));
                    skin.material.renderQueue -= 1;
                }
            }
        }


        IsFade = false;

        if (m_StackComponent != null)
            m_StackComponent.OffFade();
    }

}
