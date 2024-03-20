﻿using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface ILangueManager : IRepository<Langue>, IGetByIdInt<Langue>
    {
    }
}