using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechDispatch.Models 
{
    public class TechDispatchInitializer : System.Data.Entity.CreateDatabaseIfNotExists<TechDispatchContext>
    {
        public void TrySeed(TechDispatchContext Context)
        {
            Seed(Context);
        }

        protected override void Seed (TechDispatchContext Context)
        {

            if (Context.Users.Count() == 0)
            {
                #region Install Zones, Time Slots, Schedule, claim types.
                #region Claims!
                var claims = new List<AccessClaims>{
                new AccessClaims{
                    ClaimName="Appointments",
                    ClaimValue="Read",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Appointments",
                    ClaimValue="Write",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Appointments",
                    ClaimValue="Process",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Appointments",
                    ClaimValue="Override",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="TimeSlots",
                    ClaimValue="Read",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="TimeSlots",
                    ClaimValue="Write",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="InstallZones",
                    ClaimValue="Read",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="InstallZones",
                    ClaimValue="Write",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Towers",
                    ClaimValue="Read",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Towers",
                    ClaimValue="Write",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="AccessPoints",
                    ClaimValue="Read",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="AccessPoints",
                    ClaimValue="Write",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Customer",
                    ClaimValue="Read",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Customer",
                    ClaimValue="Write",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Schedule",
                    ClaimValue="Read",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Schedule",
                    ClaimValue="Write",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Accounts",
                    ClaimValue="Read",
                    IncludedClaim=true
                },
                new AccessClaims{
                    ClaimName="Accounts",
                    ClaimValue="Write",
                    IncludedClaim=true
                }
            };
                #endregion

                claims.ForEach(d => Context.AccessClaims.Add(d));

                var installZones = new List<InstallZone> {
                
                new InstallZone { Name="East"},
                new InstallZone {Name ="West"}
            };

                installZones.ForEach(d => Context.InstallZones.Add(d));

                var timeSlots = new List<TimeSlot> {
                new TimeSlot {Name="AM"},
                new TimeSlot {Name="PM"}
            };

                timeSlots.ForEach(d => Context.TimeSlots.Add(d));

                var schedule = new Schedule
                {
                    DefaultOption = true,
                    Active = true,
                    Date = DateTime.Today
                };

                Context.Schedules.Add(schedule);

                Context.SaveChanges();
                #endregion
                #region Towers and Roles.

                //define field tech claims here.
                var fieldTechClaims = new List<AccessClaims>();

                fieldTechClaims.AddRange(claims.Where(x => x.ClaimValue == "Read"));
                fieldTechClaims.AddRange(claims.Where(x => x.ClaimName == "Appointments" && (x.ClaimValue == "Process" || x.ClaimValue == "Write")));

                var agentClaims = new List<AccessClaims>();
                agentClaims.AddRange(claims.Where(x => x.ClaimValue == "Read"));
                agentClaims.AddRange(claims.Where(x => x.ClaimName == "Appointments" && x.ClaimName == "Write"));

                var roles = new List<TechDispatchRole> {
                new TechDispatchRole{
                    Name = "Admin",
                    IncludedRole=true,
                    AccessClaims = claims,
                    TechDispatchRoleId = 3
                },
                new TechDispatchRole{
                    Name="Field Tech",
                    IncludedRole=true,
                    AccessClaims = fieldTechClaims,
                    TechDispatchRoleId = 2
                }, 
                new TechDispatchRole{
                    Name="Agent",
                    IncludedRole=true,
                    AccessClaims = agentClaims,
                    TechDispatchRoleId = 1
                }
            };

                roles.ForEach(d => Context.TechDispatchRoles.Add(d));

                var openings = new List<Openings>();
                foreach (var z in installZones)
                {
                    foreach (var t in timeSlots)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            int amount = i == 0 ? 0 : 4;
                            openings.Add(new Openings
                            {
                                Amount = amount,
                                Day = i,
                                ScheduleID = schedule.ScheduleId,
                                InstallZoneId = z.InstallZoneId,
                                TimeSlotId = t.TimeSlotID
                            });
                        }
                    }
                }

                openings.ForEach(d => Context.Openings.Add(d));

                var towers = new List<Tower> { 
                new Tower{ TowerName="East", Subnet="10.1.1", APsToCreate = 6, InstallZoneId = installZones[0].InstallZoneId},
                new Tower{ TowerName="West", Subnet="10.1.2", APsToCreate = 3, InstallZoneId = installZones[1].InstallZoneId},
            };
                towers.ForEach(d => Context.Towers.Add(d));

                Context.SaveChanges();
                #endregion

                #region APs and IPs.


                List<IP> IPs = new List<IP>();

                for (byte i = 2; i < 255; i++)
                {
                    IPs.Add(new IP
                    {
                        Subnet = "10.1.1",
                        IPAddress = i,
                        TowerId = towers[0].TowerID
                    });
                }

                for (byte i = 2; i < 255; i++)
                {
                    IPs.Add(new IP
                    {
                        Subnet = "10.1.2",
                        IPAddress = i,
                        TowerId = towers[1].TowerID
                    });
                }
                IPs.ForEach(d => Context.IPs.Add(d));

                var APs = new List<AccessPoint>{
                new AccessPoint{Name="East Tower 1", TowerId = towers[0].TowerID, CurrentState = AccessPoint.AccessPointState.Open},
                new AccessPoint{Name="East Tower 2", TowerId = towers[0].TowerID, CurrentState = AccessPoint.AccessPointState.Open},
                new AccessPoint{Name="East Tower 3", TowerId = towers[0].TowerID, CurrentState = AccessPoint.AccessPointState.Open},
                new AccessPoint{Name="East Tower 4", TowerId = towers[0].TowerID, CurrentState = AccessPoint.AccessPointState.Open},
                new AccessPoint{Name="East Tower 5", TowerId = towers[0].TowerID, CurrentState = AccessPoint.AccessPointState.Open},
                new AccessPoint{Name="East Tower 6", TowerId = towers[0].TowerID, CurrentState = AccessPoint.AccessPointState.Open},

                new AccessPoint{Name="West Tower 1", TowerId = towers[1].TowerID, CurrentState = AccessPoint.AccessPointState.Open},
                new AccessPoint{Name="West Tower 2", TowerId = towers[1].TowerID, CurrentState = AccessPoint.AccessPointState.Open},
                new AccessPoint{Name="West Tower 3", TowerId = towers[1].TowerID, CurrentState = AccessPoint.AccessPointState.Open},
                new AccessPoint{Name="West Tower 4", TowerId = towers[1].TowerID, CurrentState = AccessPoint.AccessPointState.Open},
                new AccessPoint{Name="West Tower 5", TowerId = towers[1].TowerID, CurrentState = AccessPoint.AccessPointState.Open},
                new AccessPoint{Name="West Tower 6", TowerId = towers[1].TowerID, CurrentState = AccessPoint.AccessPointState.Open}
            };

                APs.ForEach(d => Context.AccessPoints.Add(d));

                #endregion

                Context.SaveChanges();

                var AccessRoles = new List<IdentityRole>{
                new IdentityRole{
                    Name="Admin"
                },
                new IdentityRole{
                    Name="Field Tech"
                },
                new IdentityRole{
                    Name="Agent"
                }
            };

                AccessRoles.ForEach(d => Context.Roles.Add(d));

                var passwordHasher = new PasswordHasher();

                var userStore = new UserStore<ApplicationUser>(Context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var Users = new List<ApplicationUser>{
                new ApplicationUser{
                    UserName="admin@admin.com",
                    Email="admin@admin.com",
                    Name ="Admin",
                    TechDispatchRoleId = roles.First(x=>x.Name=="Admin").TechDispatchRoleId,
                    PasswordHash = passwordHasher.HashPassword("adminpassword!"),
                    SecurityStamp = "abc",
                },
                new ApplicationUser{
                    UserName="tech@tech.com",
                    Email="tech@tech.com",
                    Name="Tech",
                    TechDispatchRoleId = roles.First(x=>x.Name=="Field Tech").TechDispatchRoleId,
                    PasswordHash = passwordHasher.HashPassword("techPassword"),
                    SecurityStamp = "abc"
                },
                new ApplicationUser{
                    UserName="agent@agent.com",
                    Email="agent@agent.com",
                    Name="Agent",
                    TechDispatchRoleId = roles.First(x=>x.Name=="Agent").TechDispatchRoleId,
                    PasswordHash = passwordHasher.HashPassword("agentPassword"),
                    SecurityStamp = "abc"
                }
            };

                Users.ForEach(d => Context.Users.Add(d));
                Context.SaveChanges();

                userManager.AddToRole(Users[0].Id, "Admin");
                userManager.AddToRole(Users[1].Id, "Field Tech");
                userManager.AddToRole(Users[2].Id, "Agent");

                Context.SaveChanges();
            }
        }
    }
}