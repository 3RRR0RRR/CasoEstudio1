using LavacarDAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LavacarDAL.Data
{
    public class LavacarDbContext : DbContext
    {
        public LavacarDbContext(DbContextOptions<LavacarDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
        public DbSet<CitaLavado> CitasLavado => Set<CitaLavado>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasIndex(x => x.Identificacion)
                .IsUnique();

            modelBuilder.Entity<Vehiculo>()
                .HasIndex(x => x.Placa)
                .IsUnique();

            modelBuilder.Entity<Vehiculo>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Vehiculos)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CitaLavado>()
                .HasOne(c => c.Cliente)
                .WithMany(x => x.Citas)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CitaLavado>()
                .HasOne(c => c.Vehiculo)
                .WithMany()
                .HasForeignKey(c => c.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
