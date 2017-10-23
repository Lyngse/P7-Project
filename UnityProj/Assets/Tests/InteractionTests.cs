using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.Experimental.UIElements;
using Assets.scripts;

public class InteractionTests {

	[Test]
	public void InteractionTestsSimplePasses() {
		// Use the Assert class to test conditions.
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator InteractionTestsWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}

    [UnityTest]
    public IEnumerator FlipTest()
    {
        var obj = new GameObject().AddComponent<Flip>();
        obj.currTrans = obj.transform;

        yield return null;

        obj.FlipObject();
        Assert.AreEqual(180, obj.transform.rotation.x);
        obj.FlipObject();
        Assert.AreEqual(0, obj.transform.rotation.x);
    }

    [UnityTest]
    public IEnumerator RotateTest()
    {


        yield return null;
    }
}
