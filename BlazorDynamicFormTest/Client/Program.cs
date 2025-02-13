using BlazorDynamicForm;

//using BlazorDynamicFormSyncfusion;
using BlazorDynamicFormTest.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using Syncfusion.Blazor;
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH5ecnZXRWVeV0J0X0M=");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddSyncfusionBlazor();
builder.Services.AddBlazorDynamicForm();/*.SyncfusionForm();*/
//.AddDataProvider((attribute, name) =>
//{
//    var data = new List<FormVar>();
//    data.Add(new FormVar() { Id = "id", Name = "Cipolle" });
//    return data;
//})

await builder.Build().RunAsync();
