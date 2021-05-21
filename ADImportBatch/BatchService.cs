using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ADImportBatch
{
    class BatchService : IBatchService
    {
        private string connectionString;

        public BatchService()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            connectionString = configuration["DefaultConnection"];
        }

        public void ExecuteNonQuery(string v)
        {
            SqlCommand cmd = new SqlCommand(v, new SqlConnection(connectionString));
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public string getCWOPAInsertStatement() =>
        @"INSERT INTO CwopaAgencyFiles
            select REPLICATE('0', 8-LEN([extensionAttribute1])) + [extensionAttribute1] AS [extensionAttribute1_padded]
            -- old method:
            --        case len(extensionAttribute1)        --employee_num contains employeeID from active directory
             --         when 6 then '00' + substring(extensionAttribute1 , 1 ,6) 
             --         when 8 then '00' + substring(extensionAttribute1 , 3 ,6) 
             --         end
            ,substring(givenName , 1, 50)         --first_name  
                  ,substring(sn , 1, 50)               --last_name
                  ,substring(initials , 1, 30)         --name_middle
                  ,substring(extensionAttribute13 , 1, 50) --email_address
                  ,substring(cn , 1, 20)                   --domain_name
                  ,substring(telephoneNumber , 1, 30)      --work_phone
                  ,substring(streetAddress , 1, 100)       --work_address
                  ,substring(l  , 1, 100)                  --work_city
                  ,substring(postalCode , 1, 15)           --work_zip
                  ,substring(extensionAttribute3  , 1, 255)  --deputate
                  ,substring(extensionAttribute4 , 1, 255)   --bureau
                  ,substring(extensionAttribute7 , 1, 255)   --division
                  ,substring(company , 1, 255) --company
                  ,substring(description , 1, 255) --description
                  ,substring(msExchExtensionAttribute20 , 1, 255) --msExchExtensionAttribute20
                  ,substring(msExchExtensionAttribute21 , 1, 255) --msExchExtensionAttribute21

    from  ADDepUsers
    -- where extensionAttribute1 is  not null
    -- and REPLICATE('0', 8-LEN([extensionAttribute1])) + [extensionAttribute1] is not null
     -- and extensionAttribute1 like REPLICATE('0', 8-LEN([extensionAttribute1])) + [extensionAttribute1]   
    --     and extensionAttribute1 not like 'X%'
--    and extensionAttribute1 in (select  PERNR from    
    -- EIS_SAP_VIEW)
     ;
";

        public string GetDBValue(string source) => (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source)) ? null : source;

        public string getElapsedTime(Stopwatch stopWatch) => 
            String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds / 10);
      
        public async Task WriteToEventLog(string v)
        {
            using (var context = new Context())
            {
                var eventLog = new EventLog
                {
                    EventMessage = v,
                    Type = "Batch",
                    Source = "ADExtract",
                    StatusCode = "Information",
                    CreateDate = DateTime.Now
                };
                await context.EventLogs.AddAsync(eventLog);
                int records = await context.SaveChangesAsync();
            }
        }
    }
}
