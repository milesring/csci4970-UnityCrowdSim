using UnityEngine;

public interface IQueue {
    void Enqueue(GameObject agent);
    void DequeueAll();
}
