﻿using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InvoiceGenerator.Data.Repository
{
    public class SalesReportRepository : ISalesReportService
    {
        /// <summary>
        /// Dbcontext for db operations
        /// </summary>
        private readonly ApplicationDbContext _context;

        public SalesReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SalesInvoice>> GetSalesInvoiceByDateRange(DateTime fromdate, DateTime todate)
        {
            var salesdata = await _context.SalesInvoices.Where(x => x.InvoicedOn >= fromdate && x.InvoicedOn <= todate)
                .Select(x => new SalesInvoice
                {
                    SalesInvoiceId = x.SalesInvoiceId,
                    PaymentMethod = x.PaymentMethod,
                    PaymentStatus = x.PaymentStatus,
                    InvoicedOn = x.InvoicedOn,
                    InvoiceCode = x.InvoiceCode,
                    Shipping = x.Shipping,
                    Tax = x.Tax,
                    GrandTotal = x.GrandTotal,
                    DiscountPercentage = x.DiscountPercentage

                }).ToListAsync();

            return salesdata; 
           
        }

    }
}
