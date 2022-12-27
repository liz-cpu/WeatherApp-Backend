Imports System.Runtime.CompilerServices
Imports Microsoft.Extensions.DependencyInjection

Module ServiceProviderExtensions
    <Extension()>
    Function AddControllersAsServices(ByVal services As IServiceCollection, ByVal controllerTypes As IEnumerable(Of Type)) As IServiceCollection
        For Each type In controllerTypes
            services.AddTransient(type)
        Next

        Return services
    End Function
End Module
