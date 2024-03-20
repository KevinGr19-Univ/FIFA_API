﻿using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class CategorieProduitManager : BaseRepository<CategorieProduit>, ICategorieProduitManager
    {
        public CategorieProduitManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<CategorieProduit?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}