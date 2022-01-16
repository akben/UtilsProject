using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindChangedPropertyNamesInList
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

            List<CustomerEntity> customerEntityList = new List<CustomerEntity>();
            customerEntityList.Add(customerEntity);

            CustomerEntity customerNewEntity = new CustomerEntity();
            customerNewEntity.Id = 1;
            customerNewEntity.a1 = true;
            customerNewEntity.a2 = "a2 value";
            customerNewEntity.a3 = 3;
            customerNewEntity.a4 = DateTime.Now;
            List<CustomerEntity> customerNewEntityList = new List<CustomerEntity>();
            customerNewEntityList.Add(customerNewEntity);

            List<string> changedProperty = ObjectUtil.FindChangedPropertyNamesInList(customerEntityList, customerNewEntityList);
            changedProperty.ForEach(i => Console.Write("{0}\t", i));
            


        }
    }
}
