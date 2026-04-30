//using Swashbuckle.AspNetCore.SwaggerGen;
//using System.Reflection;

//namespace Restaurant.Api.Swagger;

//public static class SwaggerExtensions
//{
//    public static void AddJwtBearerSecurity(this SwaggerGenOptions options)
//    {
//        // Get the OpenApi types from the loaded assembly
//        var openApiAssembly = AppDomain.CurrentDomain.GetAssemblies()
//            .FirstOrDefault(a => a.GetName().Name == "Microsoft.OpenApi");

//        if (openApiAssembly == null)
//            return;

//        // Get OpenApiSecurityScheme type
//        var securitySchemeType = openApiAssembly.GetType("Microsoft.OpenApi.Models.OpenApiSecurityScheme");
//        var securityRequirementType = openApiAssembly.GetType("Microsoft.OpenApi.Models.OpenApiSecurityRequirement");
//        var openApiReferenceType = openApiAssembly.GetType("Microsoft.OpenApi.Models.OpenApiReference");
//        var parameterLocationType = openApiAssembly.GetType("Microsoft.OpenApi.Models.ParameterLocation");
//        var securitySchemeTypeEnum = openApiAssembly.GetType("Microsoft.OpenApi.Models.SecuritySchemeType");
//        var referenceTypeEnum = openApiAssembly.GetType("Microsoft.OpenApi.Models.ReferenceType");

//        if (securitySchemeType == null || securityRequirementType == null)
//            return;

//        // Create OpenApiSecurityScheme instance dynamically
//        var securityScheme = Activator.CreateInstance(securitySchemeType)!;
//        securitySchemeType.GetProperty("Name")!.SetValue(securityScheme, "Authorization");
//        securitySchemeType.GetProperty("Type")!.SetValue(securityScheme, Enum.Parse(securitySchemeTypeEnum!, "Http"));
//        securitySchemeType.GetProperty("Scheme")!.SetValue(securityScheme, "bearer");
//        securitySchemeType.GetProperty("BearerFormat")!.SetValue(securityScheme, "JWT");
//        securitySchemeType.GetProperty("In")!.SetValue(securityScheme, Enum.Parse(parameterLocationType!, "Header"));
//        securitySchemeType.GetProperty("Description")!.SetValue(securityScheme, "JWT Authorization header using the Bearer scheme. Enter your token below.");

//        // Create OpenApiReference dynamically
//        var reference = Activator.CreateInstance(openApiReferenceType!)!;
//        openApiReferenceType!.GetProperty("Type")!.SetValue(reference, Enum.Parse(referenceTypeEnum!, "SecurityScheme"));
//        openApiReferenceType.GetProperty("Id")!.SetValue(reference, "Bearer");

//        securitySchemeType.GetProperty("Reference")!.SetValue(securityScheme, reference);

//        // Find AddSecurityDefinition extension method
//        var swaggerGenExtensions = typeof(SwaggerGenOptionsExtensions);
//        var addSecurityDefMethod = swaggerGenExtensions.GetMethods()
//            .FirstOrDefault(m => m.Name == "AddSecurityDefinition" && m.GetParameters().Length == 3);

//        if (addSecurityDefMethod != null)
//        {
//            addSecurityDefMethod.Invoke(null, new[] { options, "Bearer", securityScheme });
//        }

//        // Create OpenApiSecurityRequirement dynamically
//        var securityRequirement = Activator.CreateInstance(securityRequirementType)!;
//        var addMethod = securityRequirementType.GetMethod("Add", new[] { securitySchemeType, typeof(IList<string>) });
//        addMethod?.Invoke(securityRequirement, new[] { securityScheme, Array.Empty<string>() });

//        // Find AddSecurityRequirement extension method
//        var addSecurityReqMethod = swaggerGenExtensions.GetMethods()
//            .FirstOrDefault(m => m.Name == "AddSecurityRequirement" && m.GetParameters().Length == 2);

//        if (addSecurityReqMethod != null)
//        {
//            addSecurityReqMethod.Invoke(null, new[] { options, securityRequirement });
//        }
//    }
//}


