namespace Models
{
    public class ContractSubType
    {
        public long Id { get; set; }

        public string ContractSubTypeName { get; set; }
        public string ContractNumPrefix { get; set; }
        public string MapTypeOverride { get; set; }
        public long ContractTypeId { get; set; }
        public ContractType ContractType { get; set; }
    }
}
