Imports System.Net.Http
Imports System.Security.Policy
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web.Http
Imports System.Web.Http.Dispatcher

Public Class CustomDelegatingMessageHandler
    Inherits DelegatingHandler

    Protected Overrides Function SendAsync(Request As HttpRequestMessage, CancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
        Dim Body As String = Request.Content.ReadAsStringAsync().Result

        Dim Params As NameValueCollection = HttpUtility.ParseQueryString(Body)

        If Not Params.AllKeys.Contains("body") Then
            Request.Content = New StringContent("{}", Encoding.UTF8, "application/json")
        Else
            Try
                Request.Content = New StringContent(RSAEncryption.Decrypt(Params.Item("body"), DataProtector.Unprotect(ConfigurationManager.AppSettings.Item("AppPrivateKey"))), Encoding.UTF8, "application/json")
            Catch ex As Exception
                Request.Content = New StringContent("{}", Encoding.UTF8, "application/json")
            End Try
        End If

        Return MyBase.SendAsync(Request, CancellationToken)
    End Function
End Class
