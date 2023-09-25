using Microsoft.EntityFrameworkCore;

namespace appCoffeeMachine.EF;

public partial class CoffeeMachineDbContext : DbContext
{
    public CoffeeMachineDbContext()
    {
    }

    public CoffeeMachineDbContext(DbContextOptions<CoffeeMachineDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Formula> Formulas { get; set; }

    public virtual DbSet<Money> Money { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TypeMoney> TypeMoneys { get; set; }

    public virtual DbSet<UnitResource> UnitResources { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(DATA_SOURCE.STRING_CONNECTION);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Formula>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Coffee).HasColumnName("coffee");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.RecommendedMilk).HasColumnName("recommendedMilk");
            entity.Property(e => e.RecommendedSugar).HasColumnName("recommendedSugar");
            entity.Property(e => e.Water).HasColumnName("water");
        });

        modelBuilder.Entity<Money>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Nominal).HasColumnName("nominal");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Money)
			   .HasForeignKey(d => d.Type)
			   .OnDelete(DeleteBehavior.ClientSetNull)
			   .HasConstraintName("FK_Money_TypeMoney");
        });

		modelBuilder.Entity<Resource>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.MaxCount).HasColumnName("maxCount");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Unit).HasColumnName("unit");

            entity.HasOne(d => d.UnitNavigation).WithMany(p => p.Resources)
                .HasForeignKey(d => d.Unit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resources_UnitResources");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CountMilk).HasColumnName("countMilk");
            entity.Property(e => e.CountSugar).HasColumnName("countSugar");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("dateTime");
            entity.Property(e => e.Formulas).HasColumnName("formulas");

            entity.HasOne(d => d.FormulasNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Formulas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_Formulas");
        });

        modelBuilder.Entity<TypeMoney>(entity =>
        {
            entity.ToTable("TypeMoney");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<UnitResource>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasColumnName("unit");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
