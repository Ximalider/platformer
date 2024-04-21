using UnityEngine;

[RequireComponent( typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
public class WallInteractor : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    
    public bool WallJumping { get; private set; }
    
    [Header("Wall Slide")]
    [SerializeField][Range(0.1f, 5f)] private float _wallSlideMaxSpeed = 2f;
    
    [Header("Wall Jump")]
    [SerializeField] private Vector2 _wallJumpClimb = new Vector2(4f, 12f);
    [SerializeField] private Vector2 _wallJumpBounce = new Vector2(10.7f, 10f);
    [SerializeField] private Vector2 _wallJumpLeap = new Vector2(14f, 12f);
    [Header("Wall Stick")]
    [SerializeField, Range(0.05f, 0.5f)] private float _wallStickTime = 0.25f;
    
    private CollisionDataRetriever _collisionDataRetriever;
    private Rigidbody2D _body;
    
    private Vector2 _velocity;
    private bool _onWall;
    private bool _onGround;
    private bool _desiredJump;
    private float _wallDirectionX;
    private float _wallStickCounter;

    void Start()
    {
        _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        _body = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputManager.onJump += Jump;
    }
    private void OnDisable()
    {
        inputManager.onJump -= Jump;
    }
    public void Jump()
    {
        _desiredJump = _onWall && !_onGround;
    }

    private void FixedUpdate()
    {
        _velocity = _body.velocity;
        _onWall = _collisionDataRetriever.OnWall;
        _onGround = _collisionDataRetriever.OnGround;
        _wallDirectionX = _collisionDataRetriever.ContactNormal.x;

        #region Wall Slide
        if (_onWall)
        {
            if(_velocity.y < -_wallSlideMaxSpeed)
            {
                _velocity.y = -_wallSlideMaxSpeed;
            }
        }
        #endregion
        
        #region Wall Jump

        if ((_onWall && _velocity.x == 0) || _onGround)
        {
            WallJumping = false;
        }
        
        if (_desiredJump)
        {
            if (Mathf.Approximately(-_wallDirectionX, inputManager.Move.x))
            {
                _velocity = new Vector2(_wallJumpClimb.x * _wallDirectionX, _wallJumpClimb.y);
                WallJumping = true;
                _desiredJump = false;
            }
            else if (inputManager.Move.x == 0f)
            {
                _velocity = new Vector2(_wallJumpBounce.x * _wallDirectionX, _wallJumpBounce.y);
                WallJumping = true;
                _desiredJump = false;
            }
        }
        
        #endregion
        _body.velocity = _velocity;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collisionDataRetriever.EvaluateCollision(collision);

        if(_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && WallJumping)
        {
            _body.velocity = Vector2.zero;
        }
    }
}
