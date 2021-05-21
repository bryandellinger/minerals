using Models;

namespace Minerals.Interfaces
{
    public interface ICwopaAgencyFileRepository
    {
        CwopaAgencyFile GetByDomain(string domain);
    }
}
