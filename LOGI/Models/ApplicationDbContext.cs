using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false; 
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<FinancialReport> FinancialReports { get; set; }
        public DbSet<FinancialReportEntry> FinancialReportEntries { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<KeyIssue> KeyIssues { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<SectionVariable> SectionVariables { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<VacancyApplication> VacancyApplications { get; set; }
        public DbSet<Timeline> Timelines { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<PaymentOrder> PaymentOrders { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<InfographicCategory> InfographicCategories { get; set; }
        public DbSet<Infographic> Infographics { get; set; }
    }
}