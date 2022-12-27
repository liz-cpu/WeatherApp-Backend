Imports System.Net
Imports System.Text.Encodings.Web
Imports System.Web.Http
Imports ApiNHL.ApiNHL.Filters
Imports ApiNHL.Model
Imports ApiNHL.NHL.Requests
Imports Microsoft.VisualBasic.ApplicationServices

Namespace Controllers
    <TokenAuthenticationFilter>
    Public Class UserController
        Inherits ApiController

        Private TokenManager As ITokenManager

        Public Sub New(TokenManager As ITokenManager)
            Me.TokenManager = TokenManager
        End Sub

        <HttpPost>
        <ActionName("login")>
        <IgnoreAuthenticationFilter>
        Public Function PostLogin(Request As LoginRequest) As IHttpActionResult
            If IsNothing(Request) OrElse
                IsNothing(Request.Username) OrElse
                IsNothing(Request.Password) Then
                Return BadRequest("Zorg er voor dat alle velden zijn ingevoerd.")
            End If

            Dim Usr As Model.User = Model.User.Login(Request.Username, Request.Password)

            If IsNothing(Usr) Then
                Return BadRequest("Gebruikersnaam en wachtwoord komen niet overeen.")
            End If

            Dim Tkn As Token = Me.TokenManager.NewToken(Usr, New Dictionary(Of String, Object) From {
                {"id", Usr.Id}
            })

            Return Ok(New Dictionary(Of String, String) From {
                {"token", Tkn.Value},
                {"owmKey", DataProtector.Unprotect(ConfigurationManager.AppSettings.Item("OpenWeatherMapKey"))}
            })
        End Function

        <HttpPost>
        <ActionName("register")>
        <IgnoreAuthenticationFilter>
        Public Function PostRegister(Request As RegisterRequest) As IHttpActionResult
            If IsNothing(Request) OrElse
                IsNothing(Request.Username) OrElse
                IsNothing(Request.Password1) OrElse
                IsNothing(Request.Password2) Then
                Return BadRequest("Zorg er voor dat alle velden zijn ingevoerd.")
            ElseIf Not Request.Password1.Equals(Request.Password2) Then
                Return BadRequest("Wachtwoorden komen niet overeen.")
            ElseIf Request.Password1.Length < 10 Then
                Return BadRequest("Wachtwoord moet minimaal 10 karakters lang zijn.")
            ElseIf Request.Username.Length < 3 Then
                Return BadRequest("Gebruikernaam moet minimaal 3 karakters lang zijn.")
            ElseIf Request.Username.Length > 30 Then
                Return BadRequest("Gebruikernaam mag maximaal 30 karakters lang zijn.")
            End If

            Dim Usr As New Model.User(Request.Username, Cryptographic.HashSHA512(Request.Password1).ToString)

            If Not Usr.Save Then
                Return BadRequest("Gebruikersnaam is al in gebruik.")
            End If

            Dim Tkn As Token = Me.TokenManager.NewToken(Usr, New Dictionary(Of String, Object) From {
                {"id", Usr.Id}
            })

            Return Ok(New Dictionary(Of String, String) From {
                {"token", Tkn.Value},
                {"owm-key", DataProtector.Unprotect(ConfigurationManager.AppSettings.Item("OpenWeatherMapKey"))}
            })
        End Function

        <HttpGet>
        <ActionName("validate-token")>
        <IgnoreAuthenticationFilter>
        Public Function ValidateToken() As IHttpActionResult
            Return Ok(New Dictionary(Of String, Boolean) From {
                {"valid", Me.TokenManager.VerifyToken(HttpContext.Current.Request.Headers.Item("Authorization"))}
            })
        End Function
    End Class
End Namespace