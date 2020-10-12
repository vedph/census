using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace Census.Ef
{
    public sealed class CensusDbContext : DbContext
    {
        public DbSet<EfAct> Acts { get; set; }
        public DbSet<EfActCategory> ActCategories { get; set; }
        public DbSet<EfActPartner> ActPartners { get; set; }
        public DbSet<EfActProfession> ActProfessions { get; set; }
        public DbSet<EfActType> ActTypes { get; set; }
        public DbSet<EfActSubtype> ActSubtypes { get; set; }
        public DbSet<EfArchive> Archives { get; set; }
        public DbSet<EfBook> Books { get; set; }
        public DbSet<EfBookType> BookTypes { get; set; }
        public DbSet<EfBookSubtype> BookSubtypes { get; set; }
        public DbSet<EfCategory> Categories { get; set; }
        public DbSet<EfCompany> Companies { get; set; }
        public DbSet<EfFamily> Families { get; set; }
        public DbSet<EfPerson> Persons { get; set; }
        public DbSet<EfPlace> Places { get; set; }
        public DbSet<EfProfession> Professions { get; set; }

        // https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext

        /// <summary>
        /// Initializes a new instance of the <see cref="CensusDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public CensusDbContext(DbContextOptions<CensusDbContext> options) :
            base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CensusDbContext" />
        /// class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="databaseType">Type of the database.</param>
        public CensusDbContext(string connectionString, string databaseType) :
            base(GetOptions(connectionString, databaseType))
        {
        }

        /// <summary>
        /// Gets the options wrapping the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="databaseType">Type of the database.</param>
        /// <returns>Options.</returns>
        public static DbContextOptions<CensusDbContext> GetOptions(
            string connectionString, string databaseType)
        {
            switch (databaseType?.ToLowerInvariant())
            {
                default:
                    return new DbContextOptionsBuilder<CensusDbContext>()
                        .UseMySQL(connectionString)
                        .Options;
                    //return new DbContextOptionsBuilder<BiblioDbContext>()
                    //    .UseSqlServer(connectionString)
                    //    .Options;
            }
        }

        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options)
        /// to be used for this context.
        /// This method is called for each instance of the context that is
        /// created.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="DbContextOptions" />
        /// may or may not have been passed to the constructor, you can use
        /// <see cref="DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic
        /// in <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify
        /// options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you
        /// to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // note that these are fake credentials for development.
                // in production they would be replaced with environment data,
                // but in any case in production we would not hit this code
                optionsBuilder.UseMySQL(
                    "Server=localhost;Database=census;Uid=root;Pwd=mysql;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Override this method to further configure the model that was
        /// discovered by convention from the entity types exposed in
        /// <see cref="DbSet`1" /> properties on your derived context.
        /// The resulting model may be cached and re-used for subsequent
        /// instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct
        /// the model for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you
        /// to configure aspects of the model that are specific to a given
        /// database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context
        /// (via <see cref="DbContextOptionsBuilder.UseModel(IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Category
            modelBuilder.Entity<EfCategory>().ToTable("category");
            modelBuilder.Entity<EfCategory>().Property(c => c.Id)
                .IsRequired()
                //.UseMySqlIdentityColumn();
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfCategory>().Property(c => c.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfCategory>().Property(c => c.Namex)
                .IsUnicode()
                .IsRequired();

            // Company
            modelBuilder.Entity<EfCompany>().ToTable("company");
            modelBuilder.Entity<EfCompany>().Property(c => c.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfCompany>().Property(c => c.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfCompany>().Property(c => c.Namex)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfCompany>().Property(c => c.PreviousId)
                .HasColumnName("previousId");
            //modelBuilder.Entity<EfCompany>()
            //    .HasOne(c => c.Previous).WithOne();

            // Family
            modelBuilder.Entity<EfFamily>().ToTable("family");
            modelBuilder.Entity<EfFamily>().Property(f => f.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfFamily>().Property(f => f.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfFamily>().Property(f => f.Namex)
                .IsUnicode()
                .IsRequired();

            // Person
            modelBuilder.Entity<EfPerson>().ToTable("person");
            modelBuilder.Entity<EfPerson>().Property(p => p.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfPerson>().Property(p => p.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfPerson>().Property(p => p.Namex)
                .IsUnicode()
                .IsRequired();

            // Place
            modelBuilder.Entity<EfPlace>().ToTable("place");
            modelBuilder.Entity<EfPlace>().Property(p => p.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfPlace>().Property(p => p.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfPlace>().Property(p => p.Namex)
                .IsUnicode()
                .IsRequired();

            // Profession
            modelBuilder.Entity<EfProfession>().ToTable("profession");
            modelBuilder.Entity<EfProfession>().Property(p => p.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfProfession>().Property(p => p.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfProfession>().Property(p => p.Namex)
                .IsUnicode()
                .IsRequired();

            // Archive
            modelBuilder.Entity<EfArchive>().ToTable("archive");
            modelBuilder.Entity<EfArchive>().Property(p => p.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfArchive>().Property(p => p.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfArchive>().Property(p => p.Namex)
                .IsUnicode()
                .IsRequired();

            // BookType
            modelBuilder.Entity<EfBookType>().ToTable("bookType");
            modelBuilder.Entity<EfBookType>().Property(t => t.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfBookType>().Property(t => t.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfBookType>().Property(t => t.Namex)
                .IsUnicode()
                .IsRequired();

            // BookSubtype
            modelBuilder.Entity<EfBookSubtype>().ToTable("bookSubtype");
            modelBuilder.Entity<EfBookSubtype>().Property(s => s.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfBookSubtype>().Property(s => s.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfBookSubtype>().Property(s => s.Namex)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfBookSubtype>().HasOne(s => s.BookType)
                .WithMany(t => t.BookSubtypes);

            // Book
            modelBuilder.Entity<EfBook>().ToTable("book");
            modelBuilder.Entity<EfBook>().Property(b => b.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfBook>().Property(b => b.Location).IsUnicode();
            modelBuilder.Entity<EfBook>().Property(b => b.Locationx).IsUnicode();
            modelBuilder.Entity<EfBook>().Property(b => b.Description).IsUnicode();
            modelBuilder.Entity<EfBook>().Property(b => b.Descriptionx).IsUnicode();
            modelBuilder.Entity<EfBook>().Property(b => b.Incipit).IsUnicode();
            modelBuilder.Entity<EfBook>().Property(b => b.StartYear)
                .IsRequired();
            modelBuilder.Entity<EfBook>().Property(b => b.EndYear)
                .IsRequired();

            modelBuilder.Entity<EfBook>()
                .HasOne(b => b.Archive).WithMany(a => a.Books);
            modelBuilder.Entity<EfBook>()
                .HasOne(b => b.Type).WithMany().IsRequired();
            modelBuilder.Entity<EfBook>()
                .HasOne(b => b.Subtype).WithMany().IsRequired();
            modelBuilder.Entity<EfBook>()
                .HasOne(b => b.WritePlace).WithMany();
            modelBuilder.Entity<EfBook>()
                .HasOne(b => b.Writer).WithMany();

            // ActType
            modelBuilder.Entity<EfActType>().ToTable("actType");
            modelBuilder.Entity<EfActType>().Property(t => t.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfActType>().Property(t => t.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfActType>().Property(t => t.Namex)
                .IsUnicode()
                .IsRequired();

            // ActSubtype
            modelBuilder.Entity<EfActSubtype>().ToTable("actSubtype");
            modelBuilder.Entity<EfActSubtype>().Property(s => s.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfActSubtype>().Property(s => s.Name)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfActSubtype>().Property(s => s.Namex)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfActSubtype>().HasOne(s => s.ActType)
                .WithMany(t => t.ActSubtypes).IsRequired();

            // Act
            modelBuilder.Entity<EfAct>().ToTable("act");
            modelBuilder.Entity<EfAct>().Property(a => a.Id)
                .IsRequired()
                .UseMySQLAutoIncrementColumn("id");
            modelBuilder.Entity<EfAct>().Property(a => a.Label)
                .IsUnicode()
                .IsRequired();
            modelBuilder.Entity<EfAct>().Property(a => a.Labelx)
                .IsUnicode()
                .IsRequired();

            modelBuilder.Entity<EfAct>().HasOne(a => a.Book).WithMany().IsRequired();
            modelBuilder.Entity<EfAct>().HasOne(a => a.Family).WithMany();
            modelBuilder.Entity<EfAct>().HasOne(a => a.Company).WithMany();
            modelBuilder.Entity<EfAct>().HasOne(a => a.Place).WithMany();

            // ActCategory
            modelBuilder.Entity<EfActCategory>().ToTable("actCategory");
            modelBuilder.Entity<EfActCategory>().HasKey(ac => new
            {
                ac.ActId,
                ac.CategoryId
            });
            modelBuilder.Entity<EfActCategory>().Property(ac => ac.Unsure)
                .IsRequired();
            modelBuilder.Entity<EfActCategory>()
                .HasOne(ac => ac.Act).WithMany().IsRequired();
            modelBuilder.Entity<EfActCategory>()
                .HasOne(ac => ac.Category).WithMany().IsRequired();

            // ActPartner
            modelBuilder.Entity<EfActPartner>().ToTable("actPartner");
            modelBuilder.Entity<EfActPartner>().HasKey(ac => new
            {
                ac.ActId,
                ac.PartnerId
            });
            modelBuilder.Entity<EfActPartner>()
                .HasOne(ac => ac.Act).WithMany().IsRequired();
            modelBuilder.Entity<EfActPartner>()
                .HasOne(ac => ac.Partner).WithMany().IsRequired();

            // ActProfession
            modelBuilder.Entity<EfActProfession>().ToTable("actProfession");
            modelBuilder.Entity<EfActProfession>().HasKey(ac => new
            {
                ac.ActId,
                ac.ProfessionId
            });
            modelBuilder.Entity<EfActProfession>().Property(ac => ac.Unsure)
                .IsRequired();
            modelBuilder.Entity<EfActProfession>()
                .HasOne(ac => ac.Act).WithMany().IsRequired();
            modelBuilder.Entity<EfActProfession>()
                .HasOne(ac => ac.Profession).WithMany().IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
