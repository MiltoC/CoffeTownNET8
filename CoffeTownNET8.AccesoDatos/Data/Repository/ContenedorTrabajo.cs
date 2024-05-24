﻿using CoffeTownNET8.AccesoDatos.Data.Repository.IRepository;
using CoffeTownNET8.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeTownNET8.AccesoDatos.Data.Repository
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _db;

        public ContenedorTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Categoria = new CategoriaRepository(_db);
            Producto = new ProductoRepository(_db);
            Slider = new SliderRepository(_db);
        }

        public ICategoriaRepository Categoria { get; private set; }

        public IProductoRepository Producto { get; private set; }
        public ISliderRepository Slider { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
