using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }
        public Seller FindById(int id)
        {
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }

        public void Update(Seller obj)
        {
            if(!_context.Seller.Any(x => x.Id == obj.Id)) // Verifica se tem o ID no DB
            {
                throw new NotFoundException("Id not found"); // Caso não exista o OBJ no DB, retorna a mensagem.
            }

            try // Tentativa de atualizar o obj
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch(DbConcurrencyException e) //caso o DB retorne uma excessão
            {
                throw new DbConcurrencyException(e.Message); //Aqui pega o retorno do DB e joga no html. (Segregação de camada)
            }
        }
    }

}
