using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class NetworkMovement : NetworkBehaviour {
    /// <summary>
    /// Number of fixed time ticks per network tick
    /// </summary>
    [SerializeField] private int tickMultiplier = 2;

    private Rigidbody rb;
    private float TickDuration => tickMultiplier * Time.fixedDeltaTime;
    private float TickRate => 1f / TickDuration;

    private ulong fixedFrame = 0;
    private int lastTickSent = 0;

    private int lastTickReceived = 0;
    private Vector3 targetPosition;
    private Vector3 targetVelocity;
    private Vector3 initialAcceleration;
    private Vector3 jerk;

    [SerializeField] private bool physicsInterpolate = true;

    private ClientRpcParams NonOwnerClientParams =>
        new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = NetworkManager.Singleton.ConnectedClientsList.Where(c => c.ClientId != OwnerClientId)
                    .Select(c => c.ClientId).ToArray()
            }
        };

    private void Start() {
        rb = GetComponentInParent<Rigidbody>();
    }
    private void FixedUpdate() {
        fixedFrame++;

        //TODO: merge with input movement script?
        if (IsOwner) {
            IterateNetworkTick();
        }
        else if (IsClient && physicsInterpolate) {
            ApplyJerkInterpolation();
            lastTickReceived++;
        }
    }
    
    private void IterateNetworkTick() {
        if (lastTickSent == 0)
            SendData();
        lastTickSent = (lastTickSent + 1) % tickMultiplier;
    }
    private void ApplyJerkInterpolation() {
        if (lastTickReceived >= tickMultiplier)
            return;
        if (lastTickReceived == 0)
            CalculateJerkInterpolation();

        var acceleration = initialAcceleration + ((lastTickReceived + 0.5f) / tickMultiplier) * TickDuration * jerk;
        rb.AddForce(acceleration, ForceMode.Acceleration);
    }

    
    private void SendData() {
        if (IsServer) {
            SubmitStateClientRpc(
                transform.position,
                rb.velocity,
                NonOwnerClientParams
            );
        }
        else {
            SubmitStateServerRpc(transform.position, rb.velocity);
        }
    }

    //TODO: replace with inputs or actions during the last tick, add validation
    [ServerRpc]
    private void SubmitStateServerRpc(Vector3 position, Vector3 velocity, ServerRpcParams rpcParams = default) {
        if (!enabled)
            return;

        if (!IsClient) {
            // Dedicated server
            ApplyStateInternal(position, velocity);
        }

        SubmitStateClientRpc(
            position,
            velocity,
            NonOwnerClientParams
        );
    }

    [ClientRpc]
    private void SubmitStateClientRpc(Vector3 position, Vector3 velocity, ClientRpcParams rpcParams = default) {
        if (enabled) {
            ApplyStateInternal(position, velocity);
        }
    }
    
    private void ApplyStateInternal(Vector3 position, Vector3 velocity) {
        if (!enabled || IsOwner)
            return;

        lastTickReceived = 0;

        if (physicsInterpolate && IsClient) {
            targetPosition = position;
            targetVelocity = velocity;
        }
        else {
            transform.position = position;
            rb.velocity = velocity;
        }
    }

    //TODO: variable correction window? (separate timeToCorrect instead of using tickDuration)
    private void CalculateJerkInterpolation() {
        var currentVelocity = rb.velocity;
        Vector3 relativeVelocity = targetVelocity - currentVelocity;
        Vector3 relativePosition = targetPosition - (transform.position + currentVelocity * TickDuration);

        initialAcceleration = 2 * (3 * relativePosition - relativeVelocity * TickDuration) / Mathf.Pow(TickDuration, 2);
        jerk = 6 * (relativeVelocity * TickDuration - 2 * relativePosition) / Mathf.Pow(TickDuration, 3);
    }
}
