using UnityEngine;

/// <summary>
/// All queues, regardless of type, must implement the IQueue interface. This allows
/// certain logic to execute when determining if a game object is a type of queue
/// regardless of which type of queue it is (ordered, unordered, etc...)
/// </summary>
public interface IQueue {
    void Enqueue(GameObject agent);
    void DequeueAll();
    string GetQueueName();
}
