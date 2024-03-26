﻿using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface IClubManager : IRepository<Club>, IGetById<int, Club>
    {
    }
}