﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using my_books.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (!context.Books.Any()) 
                {
                    context.Books.AddRange(new Book
                    {
                        Title = "1st Bool Title",
                        Description = "1st Book Description",
                        IsRead = true,
                        DateRead = DateTime.Now.AddDays(-10),
                        Rate = 4,
                        Genre = "Biobraphy",
                        CoverUrl = "https://...",
                        DateAdded = DateTime.Now.AddDays(-50)
                    },
                    new Book
                    {
                        Title = "2nd Bool Title",
                        Description = "2nd Book Description",
                        IsRead = false,
                        Genre = "Fiction",
                        CoverUrl = "https://...",
                        DateAdded = DateTime.Now
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
