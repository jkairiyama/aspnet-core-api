using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.Models.ViewModels
{
    public class CustomActionResultVM
    {
        public Exception Exception { get; set; }

        public Publisher Publisher { get; set; }

    }
}
