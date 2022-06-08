using Bogus;
using System;
using System.Collections.Generic;

namespace DummyDataTest
{
    public class SampleRepository
    {
        public IEnumerable<Customer> GetCustomers()
        {
            Randomizer.Seed = new Random(123456); //씨앗
            var genCustomer = new Faker<Customer>() //아래 규칙대로 가짜 데이터 만듬
                .RuleFor(r => r.Id, Guid.NewGuid)
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Address, f => f.Address.FullAddress())
                .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber("010-####-####"))
                .RuleFor(r => r.ContactName, f => f.Name.FullName());

            return genCustomer.Generate(1000000); //가짜데이터 10개만듬
        }
    }
}
