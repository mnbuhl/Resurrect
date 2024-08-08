namespace Resurrect
{
    public interface IFunctionResolver
    {
        object ResolveInstance(ResurrectedFunction function);
    }
}