using System.Collections.Generic;

namespace Models
{
    public class ContractType
    {
        public long Id { get; set; }
        public string ContractTypeName { get; set; }
        public string ContractNumPrefix { get; set; }
        public string MapType { get; set; }

        public IEnumerable<ContractSubType> ContractSubTypes { get; set; }
    }
}
