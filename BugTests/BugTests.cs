using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void InitialState_ShouldBeNew()
    {
        var bug = new Bug();
        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void Assign_FromNew_TransitionsToTriaged()
    {
        var bug = new Bug();
        bug.Assign();
        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    [TestMethod]
    public void RequestInfo_FromTriaged_TransitionsToNeedMoreInfo()
    {
        var bug = new Bug();
        bug.Assign();
        bug.RequestInfo();
        Assert.AreEqual(BugState.NeedMoreInfo, bug.State);
    }

    [TestMethod]
    public void ProvideInfo_FromNeedMoreInfo_TransitionsToTriaged()
    {
        var bug = new Bug();
        bug.Assign();
        bug.RequestInfo();
        bug.ProvideInfo();
        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    [TestMethod]
    public void MarkAsSeparateSolution_FromTriaged_TransitionsToSeparateSolution()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsSeparateSolution();
        Assert.AreEqual(BugState.SeparateSolution, bug.State);
    }

    [TestMethod]
    public void Reconsider_FromSeparateSolution_TransitionsToTriaged()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsSeparateSolution();
        bug.Reconsider();
        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    [TestMethod]
    public void MarkAsOtherProduct_FromTriaged_TransitionsToOtherProduct()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsOtherProduct();
        Assert.AreEqual(BugState.OtherProduct, bug.State);
    }

    [TestMethod]
    public void Reconsider_FromOtherProduct_TransitionsToTriaged()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsOtherProduct();
        bug.Reconsider();
        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    [TestMethod]
    public void MarkAsNoTime_FromTriaged_TransitionsToNoTime()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsNoTime();
        Assert.AreEqual(BugState.NoTime, bug.State);
    }

    [TestMethod]
    public void Reconsider_FromNoTime_TransitionsToTriaged()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsNoTime();
        bug.Reconsider();
        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    [TestMethod]
    public void StartFix_FromTriaged_TransitionsToInProgress()
    {
        var bug = new Bug();
        bug.Assign();
        bug.StartFix();
        Assert.AreEqual(BugState.InProgress, bug.State);
    }

    [TestMethod]
    public void FixComplete_FromInProgress_TransitionsToResolved()
    {
        var bug = new Bug();
        bug.Assign();
        bug.StartFix();
        bug.FixComplete();
        Assert.AreEqual(BugState.Resolved, bug.State);
    }

    [TestMethod]
    public void VerifyOk_FromResolved_TransitionsToClosed()
    {
        var bug = new Bug();
        bug.Assign();
        bug.StartFix();
        bug.FixComplete();
        bug.VerifyOk();
        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void VerifyNotOk_FromResolved_TransitionsToReopened()
    {
        var bug = new Bug();
        bug.Assign();
        bug.StartFix();
        bug.FixComplete();
        bug.VerifyNotOk();
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Reopen_FromClosed_TransitionsToReopened()
    {
        var bug = new Bug();
        bug.Assign();
        bug.StartFix();
        bug.FixComplete();
        bug.VerifyOk();
        bug.Reopen();
        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Assign_FromReopened_TransitionsToTriaged()
    {
        var bug = new Bug();
        bug.Assign();
        bug.StartFix();
        bug.FixComplete();
        bug.VerifyNotOk();
        bug.Assign();
        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    [TestMethod]
    public void MarkAsNotBug_FromTriaged_TransitionsToNotBug()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsNotBug();
        Assert.AreEqual(BugState.NotBug, bug.State);
    }

    [TestMethod]
    public void MarkAsWonTFix_FromTriaged_TransitionsToWonTFix()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsWonTFix();
        Assert.AreEqual(BugState.WonTFix, bug.State);
    }

    [TestMethod]
    public void MarkAsDuplicate_FromTriaged_TransitionsToDuplicate()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsDuplicate();
        Assert.AreEqual(BugState.Duplicate, bug.State);
    }

    [TestMethod]
    public void MarkAsCannotReproduce_FromTriaged_TransitionsToCannotReproduce()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsCannotReproduce();
        Assert.AreEqual(BugState.CannotReproduce, bug.State);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CannotStartFix_FromNew_Throws()
    {
        var bug = new Bug();
        bug.StartFix();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CannotAssign_FromClosed_Throws()
    {
        var bug = new Bug();
        bug.Assign();
        bug.StartFix();
        bug.FixComplete();
        bug.VerifyOk();
        bug.Assign();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CannotProvideInfo_WithoutRequest_Throws()
    {
        var bug = new Bug();
        bug.Assign();
        bug.ProvideInfo();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CannotReconsider_FromNew_Throws()
    {
        var bug = new Bug();
        bug.Reconsider();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CannotVerifyOk_FromInProgress_Throws()
    {
        var bug = new Bug();
        bug.Assign();
        bug.StartFix();
        bug.VerifyOk();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TerminalNotBug_NoOutgoingTransitions()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsNotBug();
        bug.Reconsider();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TerminalWonTFix_NoOutgoingTransitions()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsWonTFix();
        bug.Reconsider();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TerminalDuplicate_NoOutgoingTransitions()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsDuplicate();
        bug.Reconsider();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TerminalCannotReproduce_NoOutgoingTransitions()
    {
        var bug = new Bug();
        bug.Assign();
        bug.MarkAsCannotReproduce();
        bug.Reconsider();
    }
}
