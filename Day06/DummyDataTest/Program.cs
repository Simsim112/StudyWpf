using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyDataTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new SampleRepository();
            var customers = repository.GetCustomers();

            Console.WriteLine(JsonConvert.SerializeObject(customers, Formatting.Indented)); //formatting으로 보기 좋게 정렬해줌
            //Console.WriteLine(JsonConvert.SerializeObject(customers)); //formatting 없으면 정렬 안되고 나옴 
            //Console.WriteLine(customers);
        }
    }
}
