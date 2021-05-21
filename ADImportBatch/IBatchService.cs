using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ADImportBatch
{
    public interface IBatchService
    {
         Task WriteToEventLog(string v);
        string getElapsedTime(Stopwatch stopWatch);
        string GetDBValue(string source);
        void ExecuteNonQuery(string v);
        string getCWOPAInsertStatement();
    }
}
