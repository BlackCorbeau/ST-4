using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    Triaged,
    NeedMoreInfo,
    SeparateSolution,
    OtherProduct,
    NoTime,
    InProgress,
    Resolved,
    Closed,
    Reopened,
    NotBug,
    WonTFix,
    Duplicate,
    CannotReproduce
}

public enum BugTrigger
{
    Assign,
    RequestInfo,
    ProvideInfo,
    MarkAsSeparateSolution,
    MarkAsOtherProduct,
    MarkAsNoTime,
    Reconsider,
    StartFix,
    FixComplete,
    VerifyOk,
    VerifyNotOk,
    Reopen,
    MarkAsNotBug,
    MarkAsWonTFix,
    MarkAsDuplicate,
    MarkAsCannotReproduce
}

public class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _machine;

    public BugState State => _machine.State;

    public Bug()
    {
        _machine = new StateMachine<BugState, BugTrigger>(BugState.New);

        _machine.Configure(BugState.New)
            .Permit(BugTrigger.Assign, BugState.Triaged);

        _machine.Configure(BugState.Triaged)
            .Permit(BugTrigger.RequestInfo, BugState.NeedMoreInfo)
            .Permit(BugTrigger.MarkAsSeparateSolution, BugState.SeparateSolution)
            .Permit(BugTrigger.MarkAsOtherProduct, BugState.OtherProduct)
            .Permit(BugTrigger.MarkAsNoTime, BugState.NoTime)
            .Permit(BugTrigger.StartFix, BugState.InProgress)
            .Permit(BugTrigger.MarkAsNotBug, BugState.NotBug)
            .Permit(BugTrigger.MarkAsWonTFix, BugState.WonTFix)
            .Permit(BugTrigger.MarkAsDuplicate, BugState.Duplicate)
            .Permit(BugTrigger.MarkAsCannotReproduce, BugState.CannotReproduce);

        _machine.Configure(BugState.NeedMoreInfo)
            .Permit(BugTrigger.ProvideInfo, BugState.Triaged);

        _machine.Configure(BugState.SeparateSolution)
            .Permit(BugTrigger.Reconsider, BugState.Triaged);
        _machine.Configure(BugState.OtherProduct)
            .Permit(BugTrigger.Reconsider, BugState.Triaged);
        _machine.Configure(BugState.NoTime)
            .Permit(BugTrigger.Reconsider, BugState.Triaged);

        _machine.Configure(BugState.InProgress)
            .Permit(BugTrigger.FixComplete, BugState.Resolved);

        _machine.Configure(BugState.Resolved)
            .Permit(BugTrigger.VerifyOk, BugState.Closed)
            .Permit(BugTrigger.VerifyNotOk, BugState.Reopened);

        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.Assign, BugState.Triaged);

        _machine.Configure(BugState.NotBug);
        _machine.Configure(BugState.WonTFix);
        _machine.Configure(BugState.Duplicate);
        _machine.Configure(BugState.CannotReproduce);
    }

    public void Assign() => _machine.Fire(BugTrigger.Assign);
    public void RequestInfo() => _machine.Fire(BugTrigger.RequestInfo);
    public void ProvideInfo() => _machine.Fire(BugTrigger.ProvideInfo);
    public void MarkAsSeparateSolution() => _machine.Fire(BugTrigger.MarkAsSeparateSolution);
    public void MarkAsOtherProduct() => _machine.Fire(BugTrigger.MarkAsOtherProduct);
    public void MarkAsNoTime() => _machine.Fire(BugTrigger.MarkAsNoTime);
    public void Reconsider() => _machine.Fire(BugTrigger.Reconsider);
    public void StartFix() => _machine.Fire(BugTrigger.StartFix);
    public void FixComplete() => _machine.Fire(BugTrigger.FixComplete);
    public void VerifyOk() => _machine.Fire(BugTrigger.VerifyOk);
    public void VerifyNotOk() => _machine.Fire(BugTrigger.VerifyNotOk);
    public void Reopen() => _machine.Fire(BugTrigger.Reopen);
    public void MarkAsNotBug() => _machine.Fire(BugTrigger.MarkAsNotBug);
    public void MarkAsWonTFix() => _machine.Fire(BugTrigger.MarkAsWonTFix);
    public void MarkAsDuplicate() => _machine.Fire(BugTrigger.MarkAsDuplicate);
    public void MarkAsCannotReproduce() => _machine.Fire(BugTrigger.MarkAsCannotReproduce);
}

class Program
{
    static void Main()
    {
        var bug = new Bug();
        Console.WriteLine($"Initial state: {bug.State}");

        bug.Assign();
        Console.WriteLine($"After Assign: {bug.State}");

        bug.StartFix();
        Console.WriteLine($"After StartFix: {bug.State}");

        bug.FixComplete();
        Console.WriteLine($"After FixComplete: {bug.State}");

        bug.VerifyOk();
        Console.WriteLine($"After VerifyOk: {bug.State}");

        Console.WriteLine("\nПопытка переоткрыть закрытый баг:");
        bug.Reopen();
        Console.WriteLine($"After Reopen: {bug.State}");
    }
}
