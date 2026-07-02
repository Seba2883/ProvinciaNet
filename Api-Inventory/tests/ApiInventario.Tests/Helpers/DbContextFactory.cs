using ApiInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiInventory.Tests.Helpers;

public static class DbContextFactory
{
    public static InventoryDbContext Create()
    {
        var options = new DbContextOptionsBuilder<InventoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InventoryDbContext(options);
    }
}