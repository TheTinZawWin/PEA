using PEA_Common;
using PEA_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAE_BusinessLayer
{
    public class DEPTBLL
    {
       
        static ICommonRepository commonRepository;
        static DEPTBLL()
        {
            
            commonRepository = new CommonRepository();
        }

        public static bool GetDepartmentList(ref List<DepartmentModel> departmentList, ref string returnMessage)
        {


            bool result = false;

            PEAEntities db = new PEAEntities();

            if (commonRepository.isConnected(ref db, ref returnMessage))
            {
                try
                {

                    departmentList = (from dpt in db.TB_Department 
                                      orderby dpt.DepartmentID 
                                    select new DepartmentModel
                                    {
                                        NAME=dpt.Name,
                                        ID=dpt.DepartmentID
                                      
                                    }).ToList();



                    result = true;

                }
                catch (Exception ex)
                {

                    returnMessage = ex.Message;
                }
                finally
                {
                    commonRepository.dbClose(db, ref returnMessage);
                }
            }
            return result;



        }

    }
}
