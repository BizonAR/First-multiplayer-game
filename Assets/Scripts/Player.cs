using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(ConstantForce))]
public class Player : Damagable, IPunObservable
{
	[SerializeField] private float _forwardForce;
	[SerializeField] private float _strafeForce;
	[SerializeField] private float _rollForce;
	[SerializeField] private float _sensitivityX;
	[SerializeField] private float _sensitivityY;
	[SerializeField] private Transform _weapon;
	[SerializeField] private float _damage;
	[SerializeField] private Spawner _spawner;

	private ConstantForce _force;
	private Rigidbody _rigidbody;
	private PhotonView _photonView;
	private Vector3 _startPosition;

	public override void OnDie()
	{
		Respawn();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		
	}

	public void SetStartPosition(Vector3 position)
	{
		_startPosition = position;
	}
	private void Awake()
	{
		_spawner = FindObjectOfType<Spawner>();
		FindObjectOfType<MiniMap>().Create(transform);
		_health = 100f;
		_force = GetComponent<ConstantForce>();
		_rigidbody = GetComponent<Rigidbody>();
		_photonView = GetComponent<PhotonView>();
	}

	private void Update()
    {
		if (_photonView.IsMine)
		{
			_force.relativeForce = Vector3.forward * _forwardForce * Input.GetAxis("Vertical")
				+ Vector3.right * _strafeForce * Input.GetAxis("Horizontal");
			_force.relativeTorque = Vector3.forward * _sensitivityX * Input.GetAxis("Mouse X") + 
				Vector3.right * _sensitivityY * Input.GetAxis("Mouse Y") +
				Vector3.up * _rollForce * Input.GetAxis("Roll");
			
			if (Input.GetMouseButtonDown(0)) 
			{
				Shoot();
			}
		}
    }

	private void Shoot()
	{
		if (Physics.Raycast(_weapon.position, _weapon.forward, out RaycastHit hit, 500f))
		{
			var victim = hit.rigidbody?.GetComponent<Player>();
			if (victim != null) 
			{
				victim.TakeDamage(_damage);
				_photonView.RPC("ApplyDamage", RpcTarget.Others, victim.gameObject.GetPhotonView().ViewID, _damage);
			}

			_photonView.RPC("SpawnLaser", RpcTarget.All, _weapon.position, hit.point);
		}
		else
		{
			_photonView.RPC("SpawnLaser", RpcTarget.All, _weapon.position, _weapon.position + _weapon.forward * 500f);
		}
	}

	[PunRPC]
	public void ApplyDamage(int id, float _damage)
	{
		PhotonView photonView = PhotonView.Find(id);

		if (photonView == null)
			return;

		var player = photonView.GetComponent<Player>();

		if (player == null) 
			return;
		
		player.TakeDamage(_damage);
	}

	[PunRPC]
	public void SpawnLaser(Vector3 start, Vector3 end)
	{
		_spawner.Pull(start, end);
	}
	private void Respawn()
	{
		if (_photonView.IsMine)
		{
			_health = 100f;
			transform.position = _startPosition;
		}
	}
}
