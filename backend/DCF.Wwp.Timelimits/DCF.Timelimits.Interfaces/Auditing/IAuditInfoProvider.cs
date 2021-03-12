namespace DCF.Core.Auditing
{
    public interface IAuditInfoProvider
    {
        void Fill(IAuditInfo auditInfo);
    }
}