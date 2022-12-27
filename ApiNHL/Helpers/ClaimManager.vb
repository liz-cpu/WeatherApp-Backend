Imports System.Web.Http
Imports Google.Protobuf.WellKnownTypes

Public Class ClaimManager

    Private Shared Function GetUserToken() As Token
        Dim TknValue As String = HttpContext.Current.Request.Headers.Item("Authorization")
        If IsNothing(TknValue) Then
            Return Nothing
        End If
        Dim Manager As ITokenManager = GlobalConfiguration.Configuration.DependencyResolver.GetService(GetType(ITokenManager))
        Return Manager.GetToken(TknValue)
    End Function
    Public Shared Sub SetClaim(Key As String, Value As String)
        GetUserToken.Claims.Add(Key, Value)
    End Sub

    Public Shared Function GetClaimValue(Key As String) As Object
        Return GetUserToken.Claims.Item(Key)
    End Function
End Class
