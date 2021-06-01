using Filmstudion.Data.Entities;
using Filmstudion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data
{
    public interface IStudioRepository
    {
        IEnumerable<Studio> AllStudios { get; }
        Studio GetStudioByName(string name);
        Studio GetStudioById(int id);
        Studio GetStudioByPresidentEmail(string email);
        Studio CreateStudio(RegisterStudioModel model);
        bool AddStudio(Studio studiotoadd);
        bool RemoveStudio(Studio studiotoadd);
    }
}
