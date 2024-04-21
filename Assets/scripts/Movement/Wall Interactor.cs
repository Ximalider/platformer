using UnityEngine;

public class WallInteractor : MonoBehaviour
{
    public bool WallJumping { get; private set; }
    [Header("Wall Jump")]
    [SerializeField] private Vector2 _wallJumpClimb = new Vector2(4f, 12f);
    [SerializeField] private Vector2 _wallJumpBounce = new Vector2(10.7f, 10f);

    private CollisionDataRetriever _collisionDataRetriever;
    private rigidBody2D _body;
    private Controller _controller;

    private Vector2 _velocity;
    private bool _onWall, _onGround, _desiredJump;
    private float _wallDirectionX;

    void Start()
    {
        _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        _body = GetConponent<RigidBody2D>();
        _controller = GetComponent<Controller>();
    }


    void Update()
    {
        if (_onWall && !_onGround)
        {
            _desiredJump |= _controller.input.RetrieveJumpInput();
        }
    }

    private void FixedUpdate()
    {
        _velocity = body.velocity;
        _onWall = _collisionDataRetriever.OnWall;
        _onGround = _collisionDataRetriever.OnGround;
        _wallDirectionX = _collisionDataRetriever.ContactNormal.x;

        #region Wall Slide
        if (_onWall)
        {
            if(_velocity.y < -_wallSlideMaxSpeed)
            {
                _velocity = -_wallSlideMaxSpeed;
            }
        }
        #endregion
        #region Wall Jump

        if ((onWall && _velocity.x == 0) || _onGround)
        {
            WallJumping = false;
        }
        if (_desiredJump)
        {
            if (-_wallDirectionX == _controller.input.RetrieveMove.Input())
            {
                _velocity = new Vector2(_wallJumpClimb.x * _wallDirectionX, _wallDirection.y);
                WallJumping = true;
                _desiredJump = false;
            }
            else if (_controller.input.RetrieveMoveInput() == 0)
            {
                _velocity = new Vector2(_wallJumpBounce.x * _wallDirectionX, _wallDirection.y);
                WallJumping = true;
                _desiredJump = false;
            }

        }
        #endregion
        _body.velocity = _velocity;
    }
}
