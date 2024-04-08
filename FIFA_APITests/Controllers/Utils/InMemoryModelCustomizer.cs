using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FIFA_APITests.Controllers.Utils
{
    internal class InMemoryModelCustomizer : RelationalModelCustomizer, IModelCustomizer
    {
        public InMemoryModelCustomizer(ModelCustomizerDependencies dependencies) : base(dependencies) { }

        public void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            base.Customize(modelBuilder, context);
            modelBuilder.Entity<VarianteCouleurProduit>()
                .Property(v => v.ImageUrls)
                .HasConversion(
                    l => string.Join('\n', l),
                    s => s.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
