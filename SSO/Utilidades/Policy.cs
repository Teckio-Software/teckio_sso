namespace SistemaERP.IOC
{
    public static class PolicyEmpresa
    {
        public static void InyectarPolicyEmpresa(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                ///Este es para un usuario de RFacil llamese usuario programador
                options.AddPolicy("Administrador", policy =>
                        policy.RequireRole("role", "Administrador")
                        );
                ///Este es para un usuario corporativo (Abel lo llamaba visor corporativo pero visor corporativo es el rol)
                options.AddPolicy("VisorPermiso", policy =>
                        policy.RequireClaim("VisorCorporativo", "Codigo-J05tg8!"));
                
                
            });
        }
    }
}
