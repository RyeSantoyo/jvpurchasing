using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using jvPo.Application.Services;

namespace jvPo.Infrastructure
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IDeliveryAddress, DeliveryAddressService>();
            services.AddScoped<ISupplier, SupplierService>();
            services.AddScoped<ITerms, TermsService>();
            services.AddScoped<IPurchaseOrder, PurchaseOrderService>();
            services.AddScoped<ICompany, CompanyService>();
            services.AddScoped<LoginService>();
            services.AddScoped<JwtTokenGenerator>();

            return services;
        }
    }
}