using Domain.Interfaces;
using Entities.Models;
using Infra.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repository
{
    public class PersonRepository : IPerson
    {
        private readonly PeopleRegistryDBContext _dBContext;

        public PersonRepository(PeopleRegistryDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<Person>> ShowPeople()
        {
            return await _dBContext.Pessoa.ToListAsync();
        }

        public async Task<Person> GetPersonById(int id)
        {
            var getPersonById = await _dBContext.Pessoa.FirstOrDefaultAsync(x => x.Id == id);
            if (getPersonById == null)
                throw new Exception($"Pessoa para o id: {id} não foi encontrado");
            return getPersonById;   
        }

        public async Task InsertPerson(Person person)
        {
            await _dBContext.Pessoa.AddAsync(person);
            await _dBContext.SaveChangesAsync();
        }

        public async Task DeletePerson(int id)
        {
            var removePersonById = await GetPersonById(id);
            if (removePersonById == null)
                throw new Exception($"Pessoa para o id: {id} não foi encontrado");
            _dBContext.Pessoa.Remove(removePersonById);
            await _dBContext.SaveChangesAsync();
        }

        public async Task UpdatePerson(Person person, int id)
        {
            var searchPersonById = await GetPersonById(id);
            if (searchPersonById == null)
                throw new Exception($"Pessoa para o id: {id} não foi encontrado");

            searchPersonById.Name = person.Name;
            searchPersonById.Email = person.Email;

            _dBContext.Pessoa.Update(searchPersonById);
            await _dBContext.SaveChangesAsync();
        }
    }
}