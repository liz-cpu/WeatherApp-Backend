Imports System.Net.Http
Imports System.Security.Principal
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Web.Http.Filters

Namespace ApiNHL.Filters
    Public Class TokenAuthenticationFilter
        Inherits AuthorizationFilterAttribute

        Public Overrides Sub OnAuthorization(Context As HttpActionContext)
            Dim Manager As ITokenManager = GlobalConfiguration.Configuration.DependencyResolver.GetService(GetType(ITokenManager))
            If NeedsAuthentication(Context) Then
                If Not IsNothing(Context.Request.Headers.Authorization) Then
                    Dim TokenValue As String = Context.Request.Headers.Authorization.ToString

                    If Not Manager.VerifyToken(TokenValue) Then
                        Context.ModelState.AddModelError("Unauthorized", "You are unauthorized.")
                        Context.Response = New HttpResponseMessage(401)
                        Context.Response.Content = New StringContent("You are unauthorized")
                    End If
                End If
            End If

            MyBase.OnAuthorization(Context)
        End Sub

        Private Function NeedsAuthentication(Context As HttpActionContext) As Boolean
            Return Not (Context.ActionDescriptor.GetCustomAttributes(Of IgnoreAuthenticationFilter).Count > 0 OrElse
                (Context.ControllerContext.ControllerDescriptor.GetCustomAttributes(Of TokenAuthenticationFilter).Count.Equals(0) AndAlso
                Context.ActionDescriptor.GetCustomAttributes(Of TokenAuthenticationFilter).Count.Equals(0)))
        End Function
    End Class
End Namespace