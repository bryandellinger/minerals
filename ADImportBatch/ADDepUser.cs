using System;
using System.DirectoryServices;
using System.Linq;

namespace ADImportBatch
{
    public class ADDepUser
    {

        public string WhenCreated { get; }
        public string WhenChanged { get; }
        public string TelephoneNumber { get; }
        public string StreetAddress { get; }
        public string Sn { get; }
        public string PostalCode { get; }
        public string L { get; }
        public string Initials { get; }
        public string GivenName { get; }
        public string ExtensionAttribute7 { get; }
        public string ExtensionAttribute4 { get; }
        public string ExtensionAttribute3 { get; }
        public string ExtensionAttribute13 { get; }
        public string Cn { get; }
        public string ExtensionAttribute1 { get; }
        public string Company { get; }
        public string ExtensionAttribute2 { get; }
        public string Description { get; }
        public string MsExchExtensionAttribute20 { get; }
        public string MsExchExtensionAttribute21 { get; }

        public ADDepUser(
          string whenCreated, string whenChanged, string telephoneNumber, string streetAddress, string sn, string postalCode,
          string l, string initials, string givenName, string extensionAttribute7, string extensionAttribute4,
          string extensionAttribute3, string extensionAttribute13, string cn, string extensionAttribute1, string company,
          string extensionAttribute2, string description, string msExchExtensionAttribute20, string msExchExtensionAttribute21)
        {
            WhenCreated = whenCreated;
            WhenChanged = whenChanged;
            TelephoneNumber = telephoneNumber;
            StreetAddress = streetAddress;
            Sn = sn;
            PostalCode = postalCode;
            L = l;
            Initials = initials;
            GivenName = givenName;
            ExtensionAttribute7 = extensionAttribute7;
            ExtensionAttribute4 = extensionAttribute4;
            ExtensionAttribute3 = extensionAttribute3;
            ExtensionAttribute13 = extensionAttribute13;
            Cn = cn;
            ExtensionAttribute1 = extensionAttribute1;
            Company = company;
            ExtensionAttribute2 = extensionAttribute2;
            Description = description;
            MsExchExtensionAttribute20 = msExchExtensionAttribute20;
            MsExchExtensionAttribute21 = msExchExtensionAttribute21;
        }

        public static ADDepUser GetADDepUserFromEntry(DirectoryEntry entry)
        {

            return new ADDepUser(entry.Properties["whenCreated"].Value?.ToString().Trim(),
               entry.Properties["whenChanged"].Value?.ToString().Trim(),
               entry.Properties["telephoneNumber"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["streetAddress"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["sn"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["postalCode"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["l"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["initials"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["givenName"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["extensionAttribute7"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["extensionAttribute4"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["extensionAttribute3"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["extensionAttribute13"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["cn"].Value?.ToString().Trim().Truncate(256),
               GetEmployeeId(entry),
               GetCompany(entry),
               entry.Properties["extensionAttribute2"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["description"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["msExchExtensionAttribute20"].Value?.ToString().Trim().Truncate(256),
               entry.Properties["msExchExtensionAttribute21"].Value?.ToString().Trim().Truncate(256));
        }

        private static string GetEmployeeId(DirectoryEntry entry)
        {
            if (entry.Properties["extensionAttribute1"] != null &&
                entry.Properties["extensionAttribute1"].Value != null &&
                !string.IsNullOrEmpty(entry.Properties["extensionAttribute1"].Value?.ToString().Trim()) &&
                !hasALetter(entry.Properties["extensionAttribute1"].Value?.ToString().Trim().Truncate(256))
                )
            {
                return entry.Properties["extensionAttribute1"].Value?.ToString().Trim().Truncate(256);
            }
            if (entry.Properties["employeeId"] != null &&
                entry.Properties["employeeId"].Value != null &&
                !string.IsNullOrEmpty(entry.Properties["employeeId"].Value?.ToString().Trim()) &&
                !hasALetter(entry.Properties["employeeId"].Value?.ToString().Trim().Truncate(256))
                )
            {
                return entry.Properties["employeeId"].Value?.ToString().Trim().Truncate(256);
            }
            return string.Empty;
        }

        private static bool hasALetter(string empNo) => empNo.Any(x => char.IsLetter(x));


        private static string GetCompany(DirectoryEntry entry)
        {
            if (entry.Properties["company"] != null && entry.Properties["company"].Value != null && !string.IsNullOrEmpty(entry.Properties["company"].Value?.ToString().Trim()))
            {
                return entry.Properties["company"].Value?.ToString().Trim().Truncate(256);
            }
            if (entry.Properties["extensionAttribute2"] != null && entry.Properties["extensionAttribute2"].Value != null && !string.IsNullOrEmpty(entry.Properties["extensionAttribute2"].Value?.ToString().Trim()))
            {
                return entry.Properties["extensionAttribute2"].Value?.ToString().Trim().Truncate(256);
            }
            return string.Empty;
        }
    }
}
