using Filmstudion.Data.Entities;
using Filmstudion.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data
{
    public class StudioRepository : IStudioRepository
    {
        private readonly AppDbContext _appDbContext;

        public StudioRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Studio> AllStudios
        {
            get
            {
                return _appDbContext.Studios;
            }
        }

        public Studio GetStudioByName(string name)
        {
            IQueryable<Studio> result = _appDbContext.Studios
              .Where(s => s.Name == name);

            return result.FirstOrDefault();
        }

        public Studio GetStudioById(int id)
        {
            IQueryable<Studio> result = _appDbContext.Studios
              .Where(s => s.StudioId == id);
            return result.FirstOrDefault();
        }

        public Studio GetStudioByPresidentEmail(string email)
        {
            IQueryable<Studio> result = _appDbContext.Studios
              .Where(s => s.PresidentEmail == email);
            return result.FirstOrDefault();
        }

        public Studio CreateStudio(RegisterStudioModel model)
        {
            var newStudio = new Studio()
            {
                Name = model.Name,
                Location = model.Location,
                PresidentName = model.PresidentName,
                PresidentEmail = model.PresidentEmail,
                PresidentPhoneNumber = model.PresidentPhoneNumber
            };
            return newStudio;
        }

        public bool AddStudio(Studio studiotoadd)
        {
            _appDbContext.Studios.Add(studiotoadd);
            //bool representerar om något ändrats i appDbContext. 
            return (_appDbContext.SaveChanges()) > 0;
        }

        public bool RemoveStudio(Studio studiotoremove)
        {
            _appDbContext.Studios.Remove(studiotoremove);
            //bool representerar om något ändrats i appDbContext. 
            return (_appDbContext.SaveChanges()) > 0;
        }

    }
}


