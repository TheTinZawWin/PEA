using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PEA_Common
{
    public class Utility
    {
    }
    public static class CommonFunction
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        if (obj.GetType().GetProperty(column.ColumnName).PropertyType == typeof(string))
                            pro.SetValue(obj, dr[column.ColumnName].ToString(), null);
                        else if (obj.GetType().GetProperty(column.ColumnName).PropertyType == typeof(int))
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else if (obj.GetType().GetProperty(column.ColumnName).PropertyType == typeof(Boolean))
                            pro.SetValue(obj, Convert.ToBoolean(dr[column.ColumnName]), null);
                        else
                            pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;

        }
        public static List<string[]> ConvertTable(DataTable table)
        {
            return table.Rows.Cast<DataRow>()
               .Select(row => table.Columns.Cast<DataColumn>()
                  .Select(col => Convert.ToString(row[col]))
               .ToArray())
            .ToList();
        }
        public static DataTable GetDataSourceFromFile(string fileName, char delimiter)
        {
            DataTable dt = new DataTable("DataTable");
            string[] columns = null;

            var lines = System.IO.File.ReadAllLines(fileName);

            // assuming the first row contains the columns information
            if (lines.Count() > 0)
            {
                columns = lines[0].Split(new char[] { delimiter });

                foreach (var column in columns)
                    dt.Columns.Add(column);
            }

            // reading rest of the data
            for (int i = 1; i < lines.Count(); i++)
            {
                DataRow dr = dt.NewRow();
                string[] values = lines[i].Split(new char[] { delimiter });

                for (int j = 0; j < values.Count() && j < columns.Count(); j++)
                    dr[j] = values[j];

                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static string GetValue(DataTable dt, int idx, string code)
        {
            string ret = "";
            try
            {
                DataRow[] dr = dt.Select("Code = '" + code + "'");
                if (dr.Count() >= 1)
                {
                    ret = dr[0].ItemArray[idx].ToString();
                }
            }
            catch (Exception e)
            {
                ret = e.Message;
            }

            return ret;
        }


        public enum ActionStatus
        {
            UnFinished = 0,
            Finished = 1,
            Delete = 2,

        };
    }
}
