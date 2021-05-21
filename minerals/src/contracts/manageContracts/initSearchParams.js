export default function initSearch() {
  return [
    {
      url: './api/ContractMgrApi', column: 'contractNum', name: 'Contract', idColumn: null,
    },
    {
      url: './api/ContractTypeMgrApi', column: 'contractTypeName', name: 'Contract Type', idColumn: null,
    },
    {
      url: './api/ContractSubTypeMgrApi', column: 'contractSubTypeName', name: 'Contract Sub Type', idColumn: null,
    },
    {
      url: './api/TractMgrApi', column: 'tractNum', name: 'Tract', idColumn: null,
    },
    {
      url: './api/DistrictMgrApi', column: 'name', name: 'District', idColumn: 'districtId',
    },
  ];
}
