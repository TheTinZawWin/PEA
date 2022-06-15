using PEA_Common;
using PEA_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAE_BusinessLayer
{
    public class EMPBLL
    {
        static IEmployeeRepository employeeRepository;
        static ICommonRepository commonRepository;
        static EMPBLL()
        {
            employeeRepository = new EmployeeRepository();
            commonRepository = new CommonRepository();

        }
        public static bool EmployeeCreate(EMPModel empModel, ref string returnMessage)
        {


            bool result = false;

            PEAEntities db = new PEAEntities();
            TB_Employee employee = new TB_Employee();
            employee.ID = Guid.NewGuid();
            employee.EmployeeID = empModel.EmployeeID;
            employee.Name = empModel.Name;

            employee.Email = empModel.Email;
            employee.Phone = empModel.Phone;
            
            employee.DepartmentID =Guid.Parse(empModel.Department);
            employee.JointDate = empModel.JointDate;
            employee.Password = empModel.Password;

            employee.Active = empModel.Active;

            employee.CreatedAt = DateTime.Now;
            employee.CreatedBy = Convert.ToInt32(empModel.CreatedBy);

            if (commonRepository.isConnected(ref db, ref returnMessage))
            {
                try
                {

                    if (employeeRepository.SaveEmployee(employee, ref db, ref returnMessage))
                    {
                        //commit to database
                        if (commonRepository.dbCommit(db, ref returnMessage))
                        {

                            result = true;
                        }
                    }
                   
                   
                    else
                    {
                        commonRepository.dbRollback(ref returnMessage);
                    }


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
   
        public static bool EmployeeUpdate(EMPModel empModel, ref string returnMessage)
        {


            bool result = false;

            PEAEntities db = new PEAEntities();
          
                TB_Employee taskTable = new TB_Employee();
              
                var employee = (from t in db.TB_Employee
                            where t.ID == empModel.ID
                            select t).FirstOrDefault();

            employee.EmployeeID = empModel.EmployeeID;
            employee.Name = empModel.Name;
            employee.Password = empModel.Password;
            employee.Email = empModel.Email;
            employee.Phone = empModel.Phone;
            employee.DepartmentID = Guid.Parse(empModel.Department);
            employee.JointDate = empModel.JointDate;
            employee.Active = empModel.Active;
            employee.UpdatedAt = DateTime.Now;
            employee.UpdatedBy =Convert.ToInt32(empModel.UpdatedBy);
           
              
           
            if (commonRepository.isConnected(ref db, ref returnMessage))
            {
                try
                {

                    if (employeeRepository.EmployeeUpdate(employee, ref db, ref returnMessage))
                    {
                        result = true;
                    }
                    //commit to database
                    if (commonRepository.dbCommit(db, ref returnMessage))
                    {

                        result = true;
                    }
                    else
                    {

                        commonRepository.dbRollback(ref returnMessage);
                    }


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

        public static bool GetEmployeeList(ref List<EMPModel> employeeList, ref string returnMessage)
        {


            bool result = false;

            PEAEntities db = new PEAEntities();

            if (commonRepository.isConnected(ref db, ref returnMessage))
            {
                try
                {

                    employeeList = (from emp in db.TB_Employee
                                    
                                    join d in db.TB_Department on emp.DepartmentID equals d.DepartmentID into dpts
                                    from dpt in dpts.DefaultIfEmpty()
                                    join u in db.Users on emp.CreatedBy equals u.UserId into users
                                    from user in users.DefaultIfEmpty()
                                    orderby emp.ID 
                                    select new EMPModel
                                    {
                                        ID = emp.ID,
                                        EmployeeID = emp.EmployeeID,
                                        Name = emp.Name,
                                        Email=emp.Email,
                                        Phone=emp.Phone,
                                        Department=dpt.Name,
                                        JointDate =emp.JointDate,
                                        Active=emp.Active,
                                        CreatedAt=emp.CreatedAt,
                                        CreatedBy =user.UserName,
                                        UpdatedAt=emp.UpdatedAt,
                                        UpdatedBy =user.UserName

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

        public static bool GetEmployeeById(Guid id, ref UpdateEMPTModel empt, ref string returnMessage)
        {
            bool result = false;

            PEAEntities db = new PEAEntities();

            if (commonRepository.isConnected(ref db, ref returnMessage))
            {
                try
                {

                    var data = (from emp in db.TB_Employee
                                where emp.ID == id
                                select new 
                                {

                                    ID = emp.ID,
                                    EmployeeID = emp.EmployeeID,
                                    Name = emp.Name,
                                    Password=emp.Password,
                                    Email = emp.Email,
                                    Phone = emp.Phone,
                                    Department =emp.DepartmentID,
                                    JointDate = emp.JointDate,
                                    Active = emp.Active,

                                }).FirstOrDefault();
                    if (data != null)
                    {
                        empt.ID = data.ID;
                        empt.EmployeeID = data.EmployeeID;
                        empt.Name = data.Name;
                        empt.Password = data.Password;
                        empt.Email = data.Email;
                        empt.Phone = data.Phone;
                        empt.Department = data.Department.ToString();
                        empt.JointDate = data.JointDate.ToString("dd MMM yyyy");
                        empt.Active = data.Active;
                        result = true;
                    }
                   

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
        public static bool EmployeeDelete(int userid,List<string> EMT_ID_LIST, ref string returnMessage)
        {


            bool result = false;

            PEAEntities db = new PEAEntities();
            List<TB_Employee> empList = new List<TB_Employee>();

            foreach (var item in EMT_ID_LIST)
            {
                TB_Employee employee = new TB_Employee();
                var guid = Guid.Parse(item);
                var emp = (from t in db.TB_Employee
                                where t.ID == guid
                                select t).FirstOrDefault();
                if (emp != null)
                {
                    if (emp.Active == false)
                    {
                        returnMessage = "alreadyinactive";
                        return result = false;
                    }
                }
                emp.Active = false;
                emp.UpdatedAt = DateTime.Now;
                emp.UpdatedBy = userid;
                empList.Add(emp);
            }


            if (commonRepository.isConnected(ref db, ref returnMessage))
            {
                try
                {

                    if (employeeRepository.EmployeeDelete(empList, ref db, ref returnMessage))
                    {
                        //commit to database
                        if (commonRepository.dbCommit(db, ref returnMessage))
                        {

                            result = true;
                        }
                    }
                   
                    else
                    {

                        commonRepository.dbRollback(ref returnMessage);
                    }


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
        public static bool GetAllDataCount(ref int activeCount, ref int inactiveCount,ref string returnMessage)
        {

            bool result = false;

            PEAEntities db = new PEAEntities();

            if (commonRepository.isConnected(ref db, ref returnMessage))
            {
                try
                {

                   var employeeList = (from emp in db.TB_Employee

                                    join d in db.TB_Department on emp.DepartmentID equals d.DepartmentID into dpts
                                    from dpt in dpts.DefaultIfEmpty()
                                    join u in db.Users on emp.CreatedBy equals u.UserId into users
                                    from user in users.DefaultIfEmpty()
                                    orderby emp.ID
                                    select new EMPModel
                                    {
                                        ID = emp.ID,
                                        EmployeeID = emp.EmployeeID,
                                        Name = emp.Name,
                                        Email = emp.Email,
                                        Phone = emp.Phone,
                                        Department = dpt.Name,
                                        JointDate = emp.JointDate,
                                        Active = emp.Active,
                                        CreatedAt = emp.CreatedAt,
                                        CreatedBy = user.UserName,
                                        UpdatedAt = emp.UpdatedAt,
                                        UpdatedBy = user.UserName

                                    }).ToList();

                    activeCount =employeeList.Where(x=>x.Active==true).Count();
                    inactiveCount =employeeList.Where(x=>x.Active==false).Count();

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
