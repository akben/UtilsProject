using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertObjectToDatatable
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomerEntity customerEntity = new CustomerEntity();
            customerEntity.Id = 1;
            customerEntity.a1 = true;
            customerEntity.a2 = "a2 value";
            customerEntity.a3 = 3;
            customerEntity.a4 = DateTime.Now;            

            DataTable table = ConvertObjectToDatatable(customerEntity);
            Console.WriteLine(String.Join(" : ", table.Rows[0].ItemArray));

        }

        static DataTable ConvertObjectToDatatable<T>(T entity)
        {
            DataTable customer = new DataTable();
            foreach (var prop in typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    customer.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                }
                else
                {
                    customer.Columns.Add(prop.Name, prop.PropertyType);
                }
            }
            DataRow dr = customer.NewRow();
            foreach (var prop in typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                dr[prop.Name] = prop.GetValue(entity, null);
            }
            customer.Rows.Add(dr);
            return customer;
        }
    }
}
