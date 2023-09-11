using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductStore.Models;

namespace ProductStore.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                DataContext context = serviceScope.ServiceProvider.GetService<DataContext>();

                context.Database.EnsureCreated();

                if (!context.Addresses.Any())
                {
                    context.Addresses.AddRange(new List<Address>()
                    {
                        new Address()
                        {
                             Street = "Mihai Eminescu",
                             City = "Iasi",
                             State = "IS",
                         },
                        new Address()
                        {
                            Street = "Mihai Viteazu",
                            City = "Iasi",
                            State = "IS",
                        },
                        new Address()
                        {
                            Street = "Petru Rares",
                            City = "Piatra Neamt",
                            State = "PN"
                        },
                        new Address()
                        {
                            Street = "George Bacovia",
                            City = "Bacau",
                            State = "BC",
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Customers.Any())
                {
                    /*var orders = new List<Order>
                    {
                        new Order()
                        {
                            DateTime = new DateTime(2022, 1, 1),
                            CustomerId = 1
                        },
                         new Order()
                        {
                            DateTime = new DateTime(2023, 1, 1),
                            CustomerId = 2
                        },
                        new Order()
                        {
                            DateTime = new DateTime(2020, 11, 1),
                            CustomerId = 3
                        },
                        new Order()
                        {
                            DateTime = new DateTime(2022, 11, 11),
                            CustomerId = 4
                        }
                    };*/
                   

                    context.Customers.AddRange(new List<Customer>()
                    {
                        new Customer()
                        {
                            Name = "Popescu",
                            Surname = "Vasile",
                            Email = "eu123@gmail.com",
                            AddressId = 1,
                            //Orders = orders
                         },
                        new Customer()
                        {
                            Name = "Popescu",
                            Surname = "Marian",
                            Email = "tu123@gmail.com",
                            AddressId = 2,
                            //Orders = orders
                        },
                        new Customer()
                        {
                            Name = "Ionescu",
                            Surname = "Ion",
                            Email = "ionescu123@gmail.com",
                            AddressId = 3,
                            //Orders= orders
                        },
                        new Customer()
                        {
                            Name = "Marica",
                            Surname = "Ciprian",
                            Email = "prosop@gmail.com",
                            AddressId = 4,
                            //Orders = orders
                        }
                    });
                    context.SaveChanges();
                }

                if(!context.Orders.Any())
                {
                    context.Orders.AddRange(new List<Order>()
                    {
                        new Order()
                        {
                            DateTime = new DateTime(2022, 1, 1),
                            CustomerId = 1
                        },
                         new Order()
                        {
                            DateTime = new DateTime(2023, 1, 1),
                            CustomerId = 2
                        },
                        new Order()
                        {
                            DateTime = new DateTime(2020, 11, 1),
                            CustomerId = 3
                        },
                        new Order()
                        {
                            DateTime = new DateTime(2022, 11, 11),
                            CustomerId = 4
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.CategoryProducts.Any())
                {
                    context.CategoryProducts.AddRange(new List<CategoryProduct>()
                    {
                        new CategoryProduct()
                        {
                             NameCategory = "Book"
                         },
                        new CategoryProduct()
                        {
                            NameCategory = "Smartphone"
                        },
                        new CategoryProduct()
                        {
                            NameCategory = "Car"
                        },
                        new CategoryProduct()
                        {
                            NameCategory = "Food"
                        }
                    });
                    context.SaveChanges();
                }


                if (!context.Products.Any())
                {
                    context.Products.AddRange(new List<Product>()
                    {
                        new Product()
                        {
                             Name = "Ursul pacalit de vulpe",
                             Price = 10,
                             CategoryProductId = 1
                         },
                        new Product()
                        {
                            Name = "Samsung",
                            Price = 100,
                            CategoryProductId = 2,
                        },
                        new Product()
                        {
                            Name = "Dacia",
                            Price = 1000,
                            CategoryProductId = 3,
                        },
                        new Product()
                        {
                            Name = "Chocolate",
                            Price = 11,
                            CategoryProductId = 4,
                        }
                    });
                    context.SaveChanges();
                }


                if (!context.OrderProducts.Any())
                {
                    context.OrderProducts.AddRange(new List<OrderProduct>()
                    {
                        new OrderProduct()
                        {
                             OrderId = 1,
                             ProductId = 1,
                         },
                        new OrderProduct()
                        {
                            OrderId = 2,
                            ProductId = 2,
                        },
                        new OrderProduct()
                        {
                            OrderId = 3,
                            ProductId = 3,
                        },
                        new OrderProduct()
                        {
                            OrderId = 4,
                            ProductId = 4,
                        }
                    });
                    context.SaveChanges();
                }

            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "razvanlozonschi123@gmail.com";

                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
             
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        UserName = "RazvanLozonschi",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
/*                        Customer = new Customer()
                        {
                            Name = "Razvan",
                            Surname = "Lozonschi",
                            Email = "razvanlozonschi123@gmail.com"
                        }*/
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");

                    var admin = await userManager.FindByEmailAsync("razvanlozonschi123@gmail.com");

                    User Dbuser = context.Users.Where(s => s.Email == newAdminUser.Email).FirstOrDefault();
                    await userManager.AddToRoleAsync(Dbuser, UserRoles.Admin);
                }

                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new User()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
/*                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }*/
                    };

                    await userManager.CreateAsync(newAppUser, "Coding@1234?");

                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
