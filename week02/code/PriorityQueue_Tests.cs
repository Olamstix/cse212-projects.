using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue multiple items with different priorities and dequeue them all.
    //           Items added: "Low"(1), "High"(5), "Medium"(3)
    // Expected Result: "High" is returned first, then "Medium", then "Low"
    // Defect(s) Found: 
    //   1. Loop used `index < _queue.Count - 1` which skipped the last item, so the
    //      highest-priority item was missed if it happened to be last in the list.
    //   2. `_queue.RemoveAt(highPriorityIndex)` was never called, so the queue never
    //      shrank and the same item was returned every time.
    public void TestPriorityQueue_1()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Low", 1);
        priorityQueue.Enqueue("High", 5);
        priorityQueue.Enqueue("Medium", 3);

        Assert.AreEqual("High", priorityQueue.Dequeue());
        Assert.AreEqual("Medium", priorityQueue.Dequeue());
        Assert.AreEqual("Low", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue multiple items where some share the same priority.
    //           Items added: "First"(3), "Second"(3), "Third"(3)
    // Expected Result: "First" is returned first (FIFO order preserved among ties),
    //                  then "Second", then "Third"
    // Defect(s) Found:
    //   1. The original loop used `>=` when comparing priorities, which caused it to
    //      prefer the LAST item among ties rather than the FIRST, breaking FIFO order.
    //      Fixed by changing `>=` to `>` so the first highest-priority item wins.
    public void TestPriorityQueue_2()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("First", 3);
        priorityQueue.Enqueue("Second", 3);
        priorityQueue.Enqueue("Third", 3);

        Assert.AreEqual("First", priorityQueue.Dequeue());
        Assert.AreEqual("Second", priorityQueue.Dequeue());
        Assert.AreEqual("Third", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Try to dequeue from an empty queue.
    // Expected Result: InvalidOperationException is thrown with message "The queue is empty."
    // Defect(s) Found: No defect. The empty check was already implemented correctly.
    public void TestPriorityQueue_Empty()
    {
        var priorityQueue = new PriorityQueue();

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail(
                string.Format("Unexpected exception of type {0} caught: {1}",
                               e.GetType(), e.Message)
            );
        }
    }

    [TestMethod]
    // Scenario: Enqueue one item with a very high priority and several with low priority.
    //           Items added: "A"(1), "B"(1), "Winner"(10), "C"(1)
    // Expected Result: "Winner" comes out first since it has the highest priority,
    //                  regardless of its position in the queue (it was added third, not first)
    // Defect(s) Found:
    //   1. Same Count - 1 bug: if "Winner" had been added last, the original loop would
    //      have skipped it entirely and returned a wrong item.
    public void TestPriorityQueue_HighestPriorityWins()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 1);
        priorityQueue.Enqueue("Winner", 10);
        priorityQueue.Enqueue("C", 1);

        Assert.AreEqual("Winner", priorityQueue.Dequeue());
    }
}