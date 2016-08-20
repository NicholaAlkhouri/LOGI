namespace LOGI.Migrations
{
    using LOGI.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LOGI.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LOGI.Models.ApplicationDbContext context)
        {

            //Seed Admin Role
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            //Seed Admin User
            string admin_username = "admin@logi.me";
            if (!context.Users.Any(u => u.UserName == admin_username))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = admin_username, Email = admin_username };

                manager.Create(user, "Password123!");
                manager.AddToRole(user.Id, "Admin");
            }

            //Seed Initial Team Members
            if (context.TeamMembers.Count() == 0)
            {
                var team_members = new List<TeamMember>
                {
                    new TeamMember { 
                            IsOnline = true,
                            Type = "cofounder",
                            ImageURL = "/Images/01_georges.jpg",
                            FullName = "GEORGES SASSINE",
                            Expertise = "Energy Policy",
                            CurrentRole = "Strategy Leader at GE Energy",
                            Career= "Deutsche Bank, Man Energy Investment Group, AEI, EESI, UNDP",
                            Education = "Master in Public Policy, Harvard University B.Eng. Mechanical Engineering, AUB"},
                     new TeamMember { 
                            IsOnline = true,
                            Type = "cofounder",
                            ImageURL = "/Images/02_karen.jpg",
                            FullName = "KAREN AYAT",
                            Expertise = "Eastern Mediterranean Energy Analyst",
                            CurrentRole = "Associate Partner & Contributor to Natural Gas Europe",
                            Career= "Credit Suisse",
                            Education = "LLM, City University Bachelor of Laws, USJ"},
                    new TeamMember { 
                            IsOnline = true,
                            Type = "cofounder",
                            ImageURL = "/Images/03_jeremy.jpg",
                            FullName = "JEREMY ARBID",
                            Expertise = "Journalist at Executive Magazine",
                            CurrentRole = "United Nations",
                            Career= "Credit Suisse",
                            Education = "Master in Public Administration, AUB Bachelor Political Science, Hamline University"},
                     new TeamMember { 
                            IsOnline = true,
                            Type = "advisory",
                            ImageURL = "/Images/05_SIMON.jpg",
                            FullName = "SIMON AYAT",
                            Expertise = "Finance",
                            CurrentRole = "Executive Vice President & Chief Financial Officer at Schlumberger",
                            Career= "Schlumberger",
                            Education = "Bachelor’s in Business Administration, University of San Francisco"},
                    new TeamMember { 
                            IsOnline = true,
                            Type = "advisory",
                            ImageURL = "/Images/06_TONJE.jpg",
                            FullName = "TONJE GORMLEY",
                            Expertise = "Petroleum Law",
                            CurrentRole = "Senior oil and gas lawyer at Armtzen de Besche",
                            Career= "Deloitte, Ministry of Labor and Social",
                            Education = "Diploma in Law, London Metropolitan University Law, University of Oslo"},
                    new TeamMember { 
                            IsOnline = true,
                            Type = "advisory",
                            ImageURL = "/Images/07_dr_Valerie.jpg",
                            FullName = "DR. VALERIE MARCEL",
                            Expertise = "Energy Governance",
                            CurrentRole = "Associate Fellow at Chatham House",
                            Career= "Institut d’etudes Politiques de Paris, University of Cairo",
                            Education = "Institut d’etudes Politiques de Paris, University of Cairo"},
                    new TeamMember { 
                            IsOnline = true,
                            Type = "advisory",
                            ImageURL = "/Images/08_LAURY.jpg",
                            FullName = "LAURY HAYTAYAN",
                            Expertise = "Oil and Gas Governance",
                            CurrentRole = "MENA Regional Associate at Natural Resource Governance Institute",
                            Career= "Arab Region Parliamentarians Against Corruption (ARPAC)",
                            Education = "Master in Middle East Politics, University of Exeter, Bachelor of Arts in Communication, Lebanese American University B.Eng. Mechanical Engineering, AUB"},
                    new TeamMember { 
                            IsOnline = true,
                            Type = "advisory",
                            ImageURL = "/Images/09_diana.jpg",
                            FullName = "DIANA KAISSY",
                            Expertise = "Oil and Gas Governance",
                            CurrentRole = "MENA and Iraq Coordinator at Publish What You Pay (PWYP)",
                            Career= "PACES",
                            Education = "B.A in Sociology and Anthropology, American University of Beirut"},
                    new TeamMember { 
                            IsOnline = true,
                            Type = "advisory",
                            ImageURL = "/Images/10_drAlan.jpg",
                            FullName = "DR. ALAN RILEY",
                            Expertise = "Energy Law, Competition Law",
                            CurrentRole = "Professor of Law at City University",
                            Career= "Institute for Statecraft",
                            Education = "PhD in European Competition Law, University of Edinburgh"},
                    new TeamMember { 
                            IsOnline = true,
                            Type = "advisory",
                            ImageURL = "/Images/11_Dr.-KOSTAS.jpg",
                            FullName = "DR. KOSTAS ANDRIOSOPOULOS",
                            Expertise = "Energy Markets, Energy Risk Management & Finance, Competition Law",
                            CurrentRole = "Associate Professor of Energy Economics at ESCP Europe",
                            Career= "DEPA Group, ESCP Europe, 3H Partners, Symmetria Web Solutions",
                            Education = "PhD in Finance & Energy Economics, Cass Business School MBA & MSc in Finance, North-eastern University"},
                };

                team_members.ForEach(s => context.TeamMembers.AddOrUpdate(t => t.FullName, s));
                context.SaveChanges();
            }

            //Seed inital financials 
            if(context.FinancialReports.Count() == 0)
            { 
                LOGI.Models.FinancialReport fr = new LOGI.Models.FinancialReport() { Year = "2015", Funding=13311, Expences = 4126.3 };
                context.FinancialReports.Add(fr);
                context.SaveChanges();

                fr.FinancialReportEntries = new List<FinancialReportEntry>();
                fr.FinancialReportEntries.Add(new LOGI.Models.FinancialReportEntry() { Type = "funding", Title = "Expert advice for the LOG&Learn app", Value = 5500,Color ="#e59b22" });
                fr.FinancialReportEntries.Add(new LOGI.Models.FinancialReportEntry() { Type = "funding", Title = "Crowdfunding campaign", Value = 7811, Color = "#fbebd4" });

                fr.FinancialReportEntries.Add(new LOGI.Models.FinancialReportEntry() { Type = "expences", Title = "Website design (in progress)", Value = 3110, Color = "#00a88f" });
                fr.FinancialReportEntries.Add(new LOGI.Models.FinancialReportEntry() { Type = "expences", Title = "Bank fees", Value = 46.3, Color = "#5d5d5d" });
                fr.FinancialReportEntries.Add(new LOGI.Models.FinancialReportEntry() { Type = "expences", Title = "Whitepaper design", Value = 500, Color = "#cbeee8" });
                fr.FinancialReportEntries.Add(new LOGI.Models.FinancialReportEntry() { Type = "expences", Title = "Translations", Value = 470, Color = "#40bdab" });

                context.SaveChanges();
            }

            //Seed initial Topics
            if(context.Topics.Count() == 0)
            {
                context.Topics.Add(new Topic() { Name = "Environmental"});
                context.Topics.Add(new Topic() { Name = "Revenue" });
                context.Topics.Add(new Topic() { Name = "Infrastructure" });
                context.Topics.Add(new Topic() { Name = "Transparency" });
                context.Topics.Add(new Topic() { Name = "Jobs" });
                context.Topics.Add(new Topic() { Name = "Security" });
            }

            //Seed initial Topics
            if (context.Sources.Count() == 0)
            {
                context.Sources.Add(new Source() { Name = "The Huffington",ImageURL = "" });
            }

            //Seed initial Types
            if (context.Types.Count() == 0)
            {
                context.Types.Add(new LOGI.Models.Type() { Name = "Reports" });
                context.Types.Add(new LOGI.Models.Type() { Name = "Press Release" });
                context.Types.Add(new LOGI.Models.Type() { Name = "Op Ed" });
            }
            if (context.Types.Where(t => t.Name == "Article").Count() == 0)
            {
                context.Types.Add(new LOGI.Models.Type() { Name = "Article" });
            }
            if (context.Types.Where(t => t.Name == "Interview").Count() == 0)
            {
                context.Types.Add(new LOGI.Models.Type() { Name = "Interview" });
            }
            if (context.Types.Where(t => t.Name == "Video").Count() == 0)
            {
                context.Types.Add(new LOGI.Models.Type() { Name = "Video" });
            }

            //Seed initial Types
            if (context.Countries.Count() == 0)
            {
                context.Countries.Add(new Country() { Name = "Cyprus" , NameAr ="قبرص"});
                context.Countries.Add(new Country() { Name = "Lebanon", NameAr = "لبنان" });
                context.Countries.Add(new Country() { Name = "Israel", NameAr = "اسرائيل" });
            }
        }
    }
}
