using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class CharacterMovementOnServer : NetworkBehaviour
{
    // private float speed;
    // private float timer;
    // private const float SERVER_TICK_RATE = 30f; 
    // private  float minTimeBetweenTicks=> 1f/SERVER_TICK_RATE;
    // private const int BUFFER_SIZE = 1024;
    //
    // private StatePayload[] stateBuffer;
    // private Queue<InputPayload> inputQueue;
    // private CharacterMovement characterMovement;
    // private static CharacterMovementOnServer playerMovementOnServer1;
    // private static CharacterMovementOnServer playerMovementOnServer2;
    //
    // public override void OnStartServer()
    // {
    //     base.OnStartServer();
    //     if (playerMovementOnServer1 == null)
    //     {
    //         playerMovementOnServer1 = this;
    //     }
    //     else
    //     {
    //         playerMovementOnServer2 = this;
    //     }
    // }
    //
    //
    // private void Start()
    // {
    //     characterMovement = GetComponent<CharacterMovement>();
    //     speed = characterMovement.speed;
    //     if (!isServer) return;
    //     stateBuffer = new StatePayload[BUFFER_SIZE];
    //     inputQueue = new Queue<InputPayload>();
    // }
    //
    //
    // private void Update()
    // {
    //     if (!isServer) return;
    //     timer += Time.deltaTime;
    //
    //     while (timer >= minTimeBetweenTicks)
    //     {
    //         timer -= minTimeBetweenTicks;
    //         HandleTick();
    //     }
    // }
    //
    // private void HandleTick()
    // {
    //     // Process the input queue
    //     int bufferIndex = -1;
    //     Vector3 pos=Vector3.zero;
    //     while(inputQueue.Count > 0)
    //     {
    //         InputPayload inputPayload = inputQueue.Dequeue();
    //         bufferIndex = inputPayload.Tick % BUFFER_SIZE;
    //         stateBuffer[bufferIndex] = ProcessMovement(inputPayload);
    //         pos = stateBuffer[bufferIndex].Position;
    //     }
    //
    //     if (bufferIndex == -1) return;
    //     MoveEnemy(pos);
    //     characterMovement.OnServerMovementState(stateBuffer[bufferIndex]);
    // }
    //
    // [Command]
    // public void OnClientInput(InputPayload inputPayload)
    // {
    //     inputQueue.Enqueue(inputPayload);
    // }
    //
    // private StatePayload ProcessMovement(InputPayload inputPayload)
    // {
    //     Vector3 inputVector = inputPayload.InputVector;
    //     transform.position += inputVector * speed * minTimeBetweenTicks;
    //
    //     return new StatePayload()
    //     {
    //         Tick = inputPayload.Tick,
    //         Position = transform.position
    //     };
    // }
    //
    // private void MoveEnemy(Vector3 pos)
    // {
    //     if (this == playerMovementOnServer1)
    //     {
    //         playerMovementOnServer2.MoveEnemyOnClient(pos);
    //     }
    //     else
    //     {
    //         playerMovementOnServer1.MoveEnemyOnClient(pos);
    //     }
    // }
    //
    // private void MoveEnemyOnClient(Vector3 pos)
    // {
    //     characterMovement.SetEnemyPos(pos);
    // }
    //
    //
    //
    //
    //

}
