using BlazorDynamicFormDataAnnotation;
using BlazorDynamicFormSyncfusion;
using BlazorDynamicFormTest.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;                                                                                                                                     

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSyncfusionBlazor();
builder.Services.AddBlazorDynamicForm()
    .AddDataProvider((attribute, name) =>
    {
        var data = new List<FormVar>();
        data.Add(new FormVar() { Id = "id", Name = "Cipolle" });
        return data;
    })
    .SyncfusionForm();
await builder.Build().RunAsync();
