using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductStore.Models;

namespace ProductStore.Data
{
    public class DataContext: IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        { 

        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(o => o.Order)
                .WithMany(op => op.OrderProduct)
                .HasForeignKey(o => o.OrderId);
            
            modelBuilder.Entity<OrderProduct>()
                .HasOne(p => p.Product)
                .WithMany(op => op.OrderProduct)
                .HasForeignKey(p => p.ProductId);

           /* modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    Id = 1,
                    Street = "Mihai Eminescu",
                    City = "Iasi",
                    State = "IS",

                },

                new Address
                {
                    Id = 2,
                    Street = "Mihai Viteazu",
                    City = "Iasi",
                    State = "IS",

                },

                new Address
                {
                    Id = 3,
                    Street = "Petru Rares",
                    City = "Piatra Neamt",
                    State = "PN",

                },

                new Address
                {
                    Id = 4,
                    Street = "George Bacovia",
                    City = "Bacau",
                    State = "BC",

                },

                new Address
                {
                    Id = 5,
                    Street = "Emil Palade",
                    City = "Vaslui",
                    State = "VS",

                }
                );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Name = "Popescu",
                    Surname = "Vasile",
                    Email = "eu123@gmail.com",
                    AddressId = 1,
                    UserId = "1"
                },

                new Customer
                {
                    Id = 2,
                    Name = "Popescu",
                    Surname = "Marian",
                    Email = "tu123@gmail.com",
                    AddressId = 2,
                    UserId = "2"
                },

                new Customer
                {
                    Id = 3,
                    Name = "Ionescu",
                    Surname = "Ion",
                    Email = "ionescu123@gmail.com",
                    AddressId = 3,
                    UserId = "3"
                },

                new Customer
                {
                    Id = 4,
                    Name = "Marica",
                    Surname = "Ciprian",
                    Email = "prosop@gmail.com",
                    AddressId = 4,
                    UserId = "4"
                },

                new Customer
                {
                    Id = 5,
                    Name = "Popa",
                    Surname = "Mihai",
                    Email = "popa@gmail.com",
                    AddressId = 5,
                    UserId = "5"
                }
                );*/

            /*modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserName = "Popescu Vasile",
                    Password = "popescu123",
                    Role = Enum.UserRole.Admin,
                    CustomerId = 1,
                    Email = "popescu123@gmail.com"
                },

                new User
                {
                    UserName = "Popescu Marian",
                    Password = "marian123",
                    Role = Enum.UserRole.User,
                    CustomerId = 2,
                    Email = "marian123@gmail.com"
                },

                new User
                {
                    UserName = "Ionescu Ion",
                    Password = "ionescu123",
                    Role = Enum.UserRole.User,
                    CustomerId = 3,
                    Email = "ionescu123@gmail.com"
                },

                new User
                {
                    UserName = "Marica Ciprian",
                    Password = "prosop123",
                    Role = Enum.UserRole.User,
                    CustomerId = 4,
                    Email = "prosop@gmail.com"
                },

                new User
                {
                    UserName = "Popa Mihai",
                    Password = "popa123",
                    Role = Enum.UserRole.User,
                    CustomerId = 5,
                    Email = "popa123@gmail.com"
                }

                );

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    DateTime = new DateTime(2022, 1, 1),
                    CustomerId = 1
                },

                new Order
                {
                    Id = 2,
                    DateTime = new DateTime(2023, 1, 1),
                    CustomerId = 1
                },

                new Order
                {
                    Id = 3,
                    DateTime = new DateTime(2020, 11, 1),
                    CustomerId = 1
                },

                new Order
                {
                    Id = 4,
                    DateTime = new DateTime(2022, 11, 11),
                    CustomerId = 2
                },

                new Order
                {
                    Id = 5,
                    DateTime = new DateTime(2022, 5, 12),
                    CustomerId = 2
                },

                new Order
                {
                    Id = 6,
                    DateTime = new DateTime(2022, 7, 17),
                    CustomerId = 2
                },

                new Order
                {
                    Id = 7,
                    DateTime = new DateTime(2020, 4, 1),
                    CustomerId = 3
                },

                new Order
                {
                    Id = 8,
                    DateTime = new DateTime(2023, 5, 5),
                    CustomerId = 3
                },

                new Order
                {
                    Id = 9,
                    DateTime = new DateTime(2022, 1, 1),
                    CustomerId = 4
                },

                new Order
                {
                    Id = 10,
                    DateTime = new DateTime(2023, 5, 1),
                    CustomerId = 5
                }
                );


            modelBuilder.Entity<CategoryProduct>().HasData(
                new CategoryProduct
                {
                    Id = 1,
                    NameCategory = "Book"
                },

                new CategoryProduct
                {
                    Id = 2,
                    NameCategory = "Smartphone"
                },

                new CategoryProduct
                {
                    Id = 3,
                    NameCategory = "Car"
                },

                new CategoryProduct
                {
                    Id = 4,
                    NameCategory = "Food"
                }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Ursul pacalit de vulpe",
                    Price = 10,
                    CategoryProductId = 1,
                },

                new Product
                {
                    Id = 2,
                    Name = "Ion",
                    Price = 100,
                    CategoryProductId = 1,
                },


                new Product
                {
                    Id = 3,
                    Name = "Dacia",
                    Price = 1000,
                    CategoryProductId = 3,
                },

                new Product
                {
                    Id = 4,
                    Name = "Chocolate",
                    Price = 11,
                    CategoryProductId = 4,
                },

                new Product
                {
                    Id = 5,
                    Name = "Rice",
                    Price = 15,
                    CategoryProductId = 4,
                }

                );


            modelBuilder.Entity<OrderProduct>().HasData(
                new OrderProduct
                {
                    OrderId = 1,
                    ProductId = 1,
                },

                new OrderProduct
                {
                    OrderId = 1,
                    ProductId = 2,
                },

                new OrderProduct
                {
                    OrderId = 1,
                    ProductId = 3,
                },

                new OrderProduct
                {
                    OrderId = 2,
                    ProductId = 4,
                },

                new OrderProduct
                {
                    OrderId = 2,
                    ProductId = 5,
                },

                new OrderProduct
                {
                    OrderId = 3,
                    ProductId = 1,
                },

                new OrderProduct
                {
                    OrderId = 3,
                    ProductId = 3,
                },

                new OrderProduct
                {
                    OrderId = 4,
                    ProductId = 3,
                },

                new OrderProduct
                {
                    OrderId = 5,
                    ProductId = 1,
                },

                new OrderProduct
                {
                    OrderId = 5,
                    ProductId = 5,
                }
                );*/
        }
    }
}
