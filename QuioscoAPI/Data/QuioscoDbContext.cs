using Microsoft.EntityFrameworkCore;
using QuioscoAPI.Models;

namespace QuioscoAPI.Data;

public class QuioscoDbContext : DbContext
{
    public QuioscoDbContext(DbContextOptions<QuioscoDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoProducto> PedidoProductos { get; set; }
    public DbSet<Mesa> Mesas { get; set; }
    public DbSet<Pago> Pagos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Name).HasColumnName("name").IsRequired();
            entity.Property(u => u.Email).HasColumnName("email").IsRequired();
            entity.Property(u => u.Password).HasColumnName("password").IsRequired();
            entity.Property(u => u.Admin).HasColumnName("admin").HasDefaultValue(false);
            entity.Property(u => u.CreatedAt).HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnName("updated_at");
            entity.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("id");
            entity.Property(c => c.Nombre).HasColumnName("nombre").IsRequired();
            entity.Property(c => c.Icono).HasColumnName("icono").IsRequired();
            entity.Property(c => c.CreatedAt).HasColumnName("created_at");
            entity.Property(c => c.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("productos");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.Nombre).HasColumnName("nombre").IsRequired();
            entity.Property(p => p.Precio).HasColumnName("precio");
            entity.Property(p => p.Imagen).HasColumnName("imagen");
            entity.Property(p => p.Disponible).HasColumnName("disponible").HasDefaultValue(true);
            entity.Property(p => p.CategoriaId).HasColumnName("categoria_id");
            entity.Property(p => p.CreatedAt).HasColumnName("created_at");
            entity.Property(p => p.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(p => p.Categoria)
                  .WithMany(c => c.Productos)
                  .HasForeignKey(p => p.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("pedidos");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.UserId).HasColumnName("user_id");
            entity.Property(p => p.Total).HasColumnName("total");
            entity.Property(p => p.Estado).HasColumnName("estado").HasDefaultValue(false);
            entity.Property(p => p.MesaId).HasColumnName("mesa_id").IsRequired(false);
            entity.Property(p => p.CreatedAt).HasColumnName("created_at");
            entity.Property(p => p.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(p => p.User)
                  .WithMany(u => u.Pedidos)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Mesa)
                  .WithMany(m => m.Pedidos)
                  .HasForeignKey(p => p.MesaId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<PedidoProducto>(entity =>
        {
            entity.ToTable("pedido_productos");
            entity.HasKey(pp => pp.Id);
            entity.Property(pp => pp.Id).HasColumnName("id");
            entity.Property(pp => pp.PedidoId).HasColumnName("pedido_id");
            entity.Property(pp => pp.ProductoId).HasColumnName("producto_id");
            entity.Property(pp => pp.Cantidad).HasColumnName("cantidad");
            entity.Property(pp => pp.CreatedAt).HasColumnName("created_at");
            entity.Property(pp => pp.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(pp => pp.Pedido)
                  .WithMany(p => p.PedidoProductos)
                  .HasForeignKey(pp => pp.PedidoId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pp => pp.Producto)
                  .WithMany(p => p.PedidoProductos)
                  .HasForeignKey(pp => pp.ProductoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.ToTable("mesas");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).HasColumnName("id");
            entity.Property(m => m.Numero).HasColumnName("numero");
            entity.Property(m => m.Capacidad).HasColumnName("capacidad");
            entity.Property(m => m.Disponible).HasColumnName("disponible").HasDefaultValue(true);
            entity.Property(m => m.CreatedAt).HasColumnName("created_at");
            entity.Property(m => m.UpdatedAt).HasColumnName("updated_at");
            entity.HasIndex(m => m.Numero).IsUnique();
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.ToTable("pagos");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.PedidoId).HasColumnName("pedido_id");
            entity.Property(p => p.Metodo).HasColumnName("metodo").IsRequired();
            entity.Property(p => p.Monto).HasColumnName("monto");
            entity.Property(p => p.FechaPago).HasColumnName("fecha_pago");
            entity.Property(p => p.CreatedAt).HasColumnName("created_at");
            entity.Property(p => p.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(p => p.Pedido)
                  .WithOne(ped => ped.Pago)
                  .HasForeignKey<Pago>(p => p.PedidoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
