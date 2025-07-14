using AuthApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApi;
public static class AddServices
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.AddSingleton<UserService>();
        services.AddSingleton<ProductService>();
        services.AddSingleton<CategoryService>();
        services.AddSingleton<CartService>();
        services.AddSingleton<BankDetailService>();
        services.AddSingleton<AddressService>();
        services.AddSingleton<ReceiverService>();
        services.AddSingleton<PaymentService>();
        return services;
    }
}

