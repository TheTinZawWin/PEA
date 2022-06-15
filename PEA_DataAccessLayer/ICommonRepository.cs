using PEA_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_DataAccessLayer
{
   public interface ICommonRepository
    {
        #region Connection
        bool isConnected(ref PEAEntities db, ref string returnMessage);
        bool dbCommit(PEAEntities db, ref string returnMessage);
        bool dbClose(PEAEntities db, ref string returnMessage);
        bool dbRollback(ref string returnMessage);
        #endregion

        bool SaveOperationLog(TB_OperationLog task, ref PEAEntities db, ref string returnMessage);
    }
}
